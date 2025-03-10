using System.Collections.Generic;
using System.Windows;
using MarieTeamBrochure.Models;
using MarieTeamBrochure.Services;

namespace MarieTeamBrochure
{
    public partial class MainWindow : Window
    {
        private DatabaseService dbService = new DatabaseService();
        private List<BateauVoyageur> bateaux;

        public MainWindow()
        {
            InitializeComponent();
            ChargerBateaux(); // Charger les bateaux au démarrage
        }


        private void ChargerBateaux()
        {
            bateaux = dbService.GetBateauxVoyageurs();
            BateauListBox.ItemsSource = bateaux;
            BateauListBox.DisplayMemberPath = "Nom";
        }

        private void BateauListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (BateauListBox.SelectedItem is BateauVoyageur bateau)
            {
                EquipementsListBox.ItemsSource = bateau.Equipements;
            }
        }

        private void GenererPDF_Click(object sender, RoutedEventArgs e)
        {
            PDFGenerator.GenerateBrochure(bateaux);
            MessageBox.Show("PDF généré avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
