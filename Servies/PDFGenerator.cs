using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MarieTeamBrochure.Models;

namespace MarieTeamBrochure.Services
{
    public class PDFGenerator
    {
        public static void GenerateBrochure(List<BateauVoyageur> bateaux)
        {
            // Définir le chemin où enregistrer le PDF (par exemple dans le dossier de l'application)
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "BateauVoyageur.pdf");

            // Créer un document PDF
            Document document = new Document();

            try
            {
                // Créer l'instance PdfWriter et ouvrir le fichier
                PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                // Ajouter le contenu au PDF
                document.Add(new Paragraph("Brochure des Bateaux Voyageurs"));
                document.Add(new Paragraph("\n"));

                foreach (var bateau in bateaux)
                {
                    document.Add(new Paragraph($"Nom: {bateau.Nom}"));
                    document.Add(new Paragraph($"Longueur: {bateau.Longueur} mètres"));
                    document.Add(new Paragraph($"Largeur: {bateau.Largeur} mètres"));
                    document.Add(new Paragraph($"Vitesse: {bateau.Vitesse} noeuds"));
                    document.Add(new Paragraph("Équipements: "));

                    foreach (var equip in bateau.Equipements)
                    {
                        document.Add(new Paragraph($"- {equip}"));
                    }
                    document.Add(new Paragraph("---------------------------"));
                }
            }
            finally
            {
                // Fermer le document
                document.Close();
            }

            // Ouvrir le fichier PDF généré automatiquement
            OpenPDF(filePath);
        }

        // Méthode pour ouvrir le fichier PDF avec le lecteur par défaut
        private static void OpenPDF(string filePath)
        {
            try
            {
                // Vérifier si le fichier existe avant de tenter de l'ouvrir
                if (File.Exists(filePath))
                {
                    // Ouvrir le PDF avec l'application par défaut associée
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                }
                else
                {
                    // Afficher un message d'erreur si le fichier n'existe pas
                    Console.WriteLine("Le fichier PDF n'a pas été trouvé.");
                }
            }
            catch (Exception ex)
            {
                // Gérer les exceptions si l'ouverture échoue
                Console.WriteLine($"Erreur lors de l'ouverture du fichier PDF : {ex.Message}");
            }
        }
    }
}
