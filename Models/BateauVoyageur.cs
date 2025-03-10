using System.Collections.Generic;

namespace MarieTeamBrochure.Models
{
    public class BateauVoyageur : Bateau
    {
        public double Vitesse { get; set; }
        public List<string> Equipements { get; set; }

        public BateauVoyageur(string id_bateau, string nom_bateau, double longueur_bateau, double largeur_bateau, double vitesse_bateau, List<string> equipements)
            : base(id_bateau, nom_bateau, longueur_bateau, largeur_bateau)
        {
            Vitesse = vitesse_bateau;
            Equipements = equipements;
        }
    }
}
