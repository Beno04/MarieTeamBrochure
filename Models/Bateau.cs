namespace MarieTeamBrochure.Models
{
    public class Bateau
    {
        public string Id { get; set; }
        public string Nom { get; set; }
        public double Longueur { get; set; }
        public double Largeur { get; set; }

        public Bateau(string id_bateau, string nom_bateau, double longueur_bateau, double largeur_bateau)
        {
            Id = id_bateau;
            Nom = nom_bateau;
            Longueur = longueur_bateau;
            Largeur = largeur_bateau;
        }
    }
}
