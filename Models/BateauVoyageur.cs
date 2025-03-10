namespace MarieTeamBrochure.Models
{
    public class BateauVoyageur : Bateau
    {
        public string Vitesse { get; set; }
        public List<string> Equipements { get; set; }
        public string image_url { get; set; } // Chemin de l'image associée au bateau

        public BateauVoyageur(string id_bateau, string nom_bateau, double longueur_bateau, double largeur_bateau, string vitesse_bateau, List<string> equipements, string image_url)
            : base(id_bateau, nom_bateau, longueur_bateau, largeur_bateau)
        {
            Vitesse = vitesse_bateau;
            Equipements = equipements;
            image_url = image_url;  // Stocker le chemin de l'image
        }
    }
}
