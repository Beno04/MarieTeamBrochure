using System.Collections.Generic;
using System.Windows;
using MarieTeamBrochure.Models;
using MarieTeamBrochure.Services;

namespace MarieTeamBrochure
{
    public partial class WindowClient : Window
    {
        private DatabaseService dbService = new DatabaseService();
        private List<BateauVoyageur> bateaux;

        public WindowClient()
        {
            InitializeComponent();
            ChargerBateaux(); // Charger les bateaux au démarrage
        }

        private void ChargerBateaux()
        {
            bateaux = dbService.GetBateauxVoyageurs();
            BateauListBox.ItemsSource = bateaux; // Lier la liste de bateaux au ListBox
            BateauListBox.DisplayMemberPath = "Nom"; // Afficher le nom du bateau dans la liste
        }

        private void BateauListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (BateauListBox.SelectedItem is BateauVoyageur bateau)
            {
                EquipementsListBox.ItemsSource = bateau.Equipements; // Afficher les équipements du bateau sélectionné
            }
        }

        private void GenererPDF_Click(object sender, RoutedEventArgs e)
        {
            PDFGenerator.GenerateBrochure(bateaux); // Générer la brochure PDF
            MessageBox.Show("PDF généré avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
