using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MarieTeamBrochure.Models;

namespace MarieTeamBrochure.Services
{
    public class PDFGenerator
    {
        public static void GenerateBrochure(List<BateauVoyageur> bateaux)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "BateauVoyageur.pdf");
            Document document = new Document();

            try
            {
                PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                // Définition des polices
                Font titleFont = new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD);
                Font boldFont = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);

                // Ajouter le titre centré en gras et grand
                Paragraph title = new Paragraph("Brochure des Bateaux Voyageurs", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);
                document.Add(new Paragraph("\n"));

                foreach (var bateau in bateaux)
                {
                    try
                    {
                        string tempImagePath = DownloadImage(bateau.image_url);
                        if (!string.IsNullOrEmpty(tempImagePath))
                        {
                            Image img = Image.GetInstance(tempImagePath);
                            img.ScaleToFit(400f, 400f);
                            document.Add(img);
                            File.Delete(tempImagePath);
                        }
                        else
                        {
                            document.Add(new Paragraph("Image non disponible"));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de l'ajout de l'image : {ex.Message}");
                    }

                    // Nom du bateau en gras avec espaces avant et après
                    document.Add(new Paragraph("\n"));
                    document.Add(new Paragraph($"Nom: {bateau.Nom}", boldFont));
                    document.Add(new Paragraph("\n"));

                    document.Add(new Paragraph($"Longueur: {bateau.Longueur} mètres"));
                    document.Add(new Paragraph($"Largeur: {bateau.Largeur} mètres"));
                    document.Add(new Paragraph($"Vitesse: {bateau.Vitesse} noeuds"));
                    document.Add(new Paragraph("Équipements: "));

                    foreach (var equip in bateau.Equipements)
                    {
                        document.Add(new Paragraph($"- {equip}"));
                    }

                    // Ligne de séparation avec espaces avant et après
                    document.Add(new Paragraph("\n"));
                    document.Add(new Paragraph("---------------------------"));
                    document.Add(new Paragraph("\n"));
                }
            }
            finally
            {
                document.Close();
            }

            OpenPDF(filePath);
        }

        private static string DownloadImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return null;

            string uniqueFileName = Guid.NewGuid().ToString() + ".jpg";
            string tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), uniqueFileName);

            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(imageUrl, tempFilePath);
                }
                return tempFilePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du téléchargement de l'image: {ex.Message}");
                return null;
            }
        }

        private static void OpenPDF(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                }
                else
                {
                    Console.WriteLine("Le fichier PDF n'a pas été trouvé.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ouverture du fichier PDF : {ex.Message}");
            }
        }
    }
}
