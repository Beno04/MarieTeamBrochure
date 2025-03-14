using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MarieTeamBrochure.Models;
using MarieTeamBrochure.Services;

namespace MarieTeamBrochure
{
    public partial class WindowAdmin : Window
    {
        private DatabaseService dbService = new DatabaseService();
        private List<BateauVoyageur> bateaux;
        private List<string> equipementsDisponibles;

        public WindowAdmin()
        {
            InitializeComponent();
            ChargerBateaux();
            ChargerEquipements();
        }

        private void ChargerBateaux()
        {
            bateaux = dbService.GetBateauxVoyageurs();
            BateauListBox.ItemsSource = bateaux;
            BateauListBox.DisplayMemberPath = "Nom";
        }

        private void ChargerEquipements()
        {
            equipementsDisponibles = dbService.GetAllEquipements();
            EquipementComboBox.ItemsSource = equipementsDisponibles;
        }

        private void BateauListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (BateauListBox.SelectedItem is BateauVoyageur bateau)
            {
                txtNom.Text = bateau.Nom;
                txtLongueur.Text = bateau.Longueur.ToString();
                txtLargeur.Text = bateau.Largeur.ToString();
                txtVitesse.Text = bateau.Vitesse;
                txtImage.Text = bateau.image_url;
                EquipementsListBox.ItemsSource = bateau.Equipements;
                MettreAJourEquipementsDisponibles(bateau);
            }
        }

        private void BtnAjouterEquipement_Click(object sender, RoutedEventArgs e)
        {
            if (BateauListBox.SelectedItem is BateauVoyageur bateau && EquipementComboBox.SelectedItem is string equipement)
            {
                if (dbService.AddEquipementToBateau(bateau.Id, equipement))
                {
                    bateau.Equipements.Add(equipement);
                    EquipementsListBox.Items.Refresh();
                    MettreAJourEquipementsDisponibles(bateau);
                    MessageBox.Show("Équipement ajouté avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout de l'équipement.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnSupprimerEquipement_Click(object sender, RoutedEventArgs e)
        {
            if (BateauListBox.SelectedItem is BateauVoyageur bateau && EquipementsListBox.SelectedItem is string equipement)
            {
                if (dbService.RemoveEquipementFromBateau(bateau.Id, equipement))
                {
                    bateau.Equipements.Remove(equipement);
                    EquipementsListBox.Items.Refresh();
                    MettreAJourEquipementsDisponibles(bateau);
                    MessageBox.Show("Équipement supprimé avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression de l'équipement.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void MettreAJourEquipementsDisponibles(BateauVoyageur bateau)
        {
            if (bateau != null)
            {
                var equipementsDisponibles = dbService.GetAllEquipements().Except(bateau.Equipements).ToList();
                EquipementComboBox.ItemsSource = equipementsDisponibles;
            }
            else
            {
                EquipementComboBox.ItemsSource = dbService.GetAllEquipements();
            }
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier si les champs sont remplis
            if (string.IsNullOrWhiteSpace(txtNom.Text) ||
                string.IsNullOrWhiteSpace(txtLongueur.Text) ||
                string.IsNullOrWhiteSpace(txtLargeur.Text) ||
                string.IsNullOrWhiteSpace(txtVitesse.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs avant d'ajouter un bateau.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var bateau = new BateauVoyageur(
                id_bateau: Guid.NewGuid().ToString(), // Génère un ID unique
                nom_bateau: txtNom.Text,
                longueur_bateau: double.Parse(txtLongueur.Text),
                largeur_bateau: double.Parse(txtLargeur.Text),
                vitesse_bateau: txtVitesse.Text,
                equipements: new List<string>(),  // Tu peux laisser la liste vide ou la remplir plus tard
                image_url: txtImage.Text
                );

                if (dbService.AddBateauVoyageur(bateau))
                {
                    bateaux.Add(bateau);
                    BateauListBox.Items.Refresh();
                    MessageBox.Show("Bateau ajouté avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout du bateau.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de saisie : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (BateauListBox.SelectedItem is BateauVoyageur selectedBateau)
            {
                // Vérifier si les champs sont remplis
                if (string.IsNullOrWhiteSpace(txtNom.Text) ||
                    string.IsNullOrWhiteSpace(txtLongueur.Text) ||
                    string.IsNullOrWhiteSpace(txtLargeur.Text) ||
                    string.IsNullOrWhiteSpace(txtVitesse.Text))
                {
                    MessageBox.Show("Veuillez remplir tous les champs avant de modifier le bateau.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    selectedBateau.Nom = txtNom.Text;
                    selectedBateau.Longueur = double.Parse(txtLongueur.Text);
                    selectedBateau.Largeur = double.Parse(txtLargeur.Text);
                    selectedBateau.Vitesse = txtVitesse.Text;
                    selectedBateau.image_url = txtImage.Text;

                    if (dbService.UpdateBateauVoyageur(selectedBateau))
                    {
                        BateauListBox.Items.Refresh();
                        MessageBox.Show("Bateau modifié avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification du bateau.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur de saisie : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un bateau à modifier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void GenererPDF_Click(object sender, RoutedEventArgs e)
        {
            PDFGenerator.GenerateBrochure(bateaux);
            MessageBox.Show("PDF généré avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
