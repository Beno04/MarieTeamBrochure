using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using MarieTeamBrochure.Models;
using System.Text.RegularExpressions;

namespace MarieTeamBrochure.Services
{
    public class DatabaseService
    {
        private string connectionString = "Server=localhost;Port=3306;Database=marieteam;Uid=root;Pwd=;";

        // Méthode de validation des données d'un bateau
        private bool IsValidBateau(BateauVoyageur bateau)
        {
            // Vérifie que le nom contient uniquement des lettres et caractères spéciaux
            if (!Regex.IsMatch(bateau.Nom, @"^[a-zA-ZÀ-ÿ\s\-']+$"))
            {
                MessageBox.Show("Le nom du bateau ne doit contenir que des lettres et des caractères spéciaux !");
                return false;
            }

            // Vérifie que longueur, largeur et vitesse sont des nombres
            if (!double.TryParse(bateau.Longueur.ToString(), out _))            
            {
                MessageBox.Show("Le champ longeur doit contenir uniquement des nombres !");
                return false;
            }
            if (!double.TryParse(bateau.Largeur.ToString(), out _))
            {
                MessageBox.Show("Le champ largeur doit contenir uniquement des nombres !");
                return false;
            }
            if (!double.TryParse(bateau.Vitesse, out _))
            {
                MessageBox.Show("Le champ vitesse doit contenir uniquement des nombres !");
                return false;
            }

            // Vérifie que l'URL de l'image est valide
            if (!Uri.IsWellFormedUriString(bateau.image_url, UriKind.Absolute))
            {
                MessageBox.Show("L'URL de l'image n'est pas valide !");
                return false;
            }

            return true;
        }


        // Méthode pour récupérer tous les bateaux
        public List<BateauVoyageur> GetBateauxVoyageurs()
        {
            List<BateauVoyageur> bateaux = new List<BateauVoyageur>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT id_bateau, nom_bateau, long_bateau, larg_bateau, vitesse_bateau, image_url FROM bateau";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string id_bateau = reader["id_bateau"].ToString();
                    string nom_bateau = reader["nom_bateau"].ToString();
                    double longueur_bateau = Convert.ToDouble(reader["long_bateau"]);
                    double largeur_bateau = Convert.ToDouble(reader["larg_bateau"]);
                    string vitesse_bateau = reader["vitesse_bateau"].ToString();
                    string image_url = reader["image_url"].ToString();
                    List<string> equipements = GetEquipements(id_bateau);
                    bateaux.Add(new BateauVoyageur(id_bateau, nom_bateau, longueur_bateau, largeur_bateau, vitesse_bateau, equipements, image_url));
                }
                reader.Close();
            }
            return bateaux;
        }

        // Récupérer tous les équipements disponibles
        public List<string> GetAllEquipements()
        {
            List<string> equipements = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT desc_equip FROM equipement";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    equipements.Add(reader["desc_equip"].ToString());
                }
                reader.Close();
            }
            return equipements;
        }

        // Récupérer les équipements d'un bateau spécifique
        private List<string> GetEquipements(string bateauId)
        {
            List<string> equipements = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT e.desc_equip FROM equipement e JOIN `être_équipé` ee ON e.id_equip = ee.id_equip WHERE ee.id_bateau = @BateauId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@BateauId", bateauId);
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    equipements.Add(reader["desc_equip"].ToString());
                }
                reader.Close();
            }
            return equipements;
        }

        // Ajouter un équipement à un bateau
        public bool AddEquipementToBateau(string bateauId, string equipement)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO `être_équipé` (id_bateau, id_equip) SELECT @BateauId, id_equip FROM equipement WHERE desc_equip = @Equipement";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@BateauId", bateauId);
                cmd.Parameters.AddWithValue("@Equipement", equipement);
                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de l'ajout de l'équipement : " + ex.Message);
                    return false;
                }
            }
        }

        // Supprimer un équipement d'un bateau
        public bool RemoveEquipementFromBateau(string bateauId, string equipement)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM `être_équipé` WHERE id_bateau = @BateauId AND id_equip = (SELECT id_equip FROM equipement WHERE desc_equip = @Equipement)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@BateauId", bateauId);
                cmd.Parameters.AddWithValue("@Equipement", equipement);
                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la suppression de l'équipement : " + ex.Message);
                    return false;
                }
            }
        }
        // Méthode pour ajouter un bateau
        // Ajouter un bateau
        public bool AddBateauVoyageur(BateauVoyageur bateau)
        {
            if (!IsValidBateau(bateau)) return false;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO bateau (id_bateau, nom_bateau, long_bateau, larg_bateau, vitesse_bateau, image_url) VALUES (@Id, @Nom, @Longueur, @Largeur, @Vitesse, @ImageUrl)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", bateau.Id);
                cmd.Parameters.AddWithValue("@Nom", bateau.Nom);
                cmd.Parameters.AddWithValue("@Longueur", bateau.Longueur);
                cmd.Parameters.AddWithValue("@Largeur", bateau.Largeur);
                cmd.Parameters.AddWithValue("@Vitesse", bateau.Vitesse);
                cmd.Parameters.AddWithValue("@ImageUrl", bateau.image_url);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de l'ajout du bateau: " + ex.Message);
                    return false;
                }
            }
        }

        // Modifier un bateau
        public bool UpdateBateauVoyageur(BateauVoyageur bateau)
        {
            if (!IsValidBateau(bateau)) return false;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "UPDATE bateau SET nom_bateau = @Nom, long_bateau = @Longueur, larg_bateau = @Largeur, vitesse_bateau = @Vitesse, image_url = @ImageUrl WHERE id_bateau = @Id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", bateau.Id);
                cmd.Parameters.AddWithValue("@Nom", bateau.Nom);
                cmd.Parameters.AddWithValue("@Longueur", bateau.Longueur);
                cmd.Parameters.AddWithValue("@Largeur", bateau.Largeur);
                cmd.Parameters.AddWithValue("@Vitesse", bateau.Vitesse);
                cmd.Parameters.AddWithValue("@ImageUrl", bateau.image_url);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la modification du bateau: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
