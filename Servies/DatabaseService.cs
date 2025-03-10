﻿using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using MarieTeamBrochure.Models;

namespace MarieTeamBrochure.Services
{
    public class DatabaseService
    {
        private string connectionString = "Server=localhost;Port=3306;Database=marieteam;Uid=root;Pwd=;";

        // Méthode pour récupérer les bateaux
        public List<BateauVoyageur> GetBateauxVoyageurs()
        {
            List<BateauVoyageur> bateaux = new List<BateauVoyageur>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT id_bateau, nom_bateau, long_bateau, larg_bateau, vitesse_bateau FROM bateau";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string id_bateau = reader["id_bateau"].ToString();
                    string nom_bateau = reader["nom_bateau"].ToString();
                    double longueur_bateau = Convert.ToDouble(reader["long_bateau"]);
                    double largeur_bateau = Convert.ToDouble(reader["larg_bateau"]);

                    // Conversion de la vitesse_bateau en double
                    double vitesse_bateau = 0;
                    if (double.TryParse(reader["vitesse_bateau"].ToString(), out double result))
                    {
                        vitesse_bateau = result;
                    }
                    else
                    {
                        // Si la conversion échoue, on peut assigner une valeur par défaut ou gérer autrement
                        Console.WriteLine($"[WARNING] Impossible de convertir la vitesse du bateau {nom_bateau} en double.");
                    }

                    List<string> equipements = GetEquipements(id_bateau);

                    Console.WriteLine($"[DEBUG] Bateau trouvé : {nom_bateau}, Longueur: {longueur_bateau}, Largeur: {largeur_bateau}, Vitesse: {vitesse_bateau}");

                    bateaux.Add(new BateauVoyageur(id_bateau, nom_bateau, longueur_bateau, largeur_bateau, vitesse_bateau, equipements));
                }
                reader.Close();
            }

            Console.WriteLine($"[DEBUG] Nombre total de bateaux récupérés : {bateaux.Count}");
            return bateaux;
        }

        // Méthode pour récupérer les équipements d'un bateau
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
    }
}
