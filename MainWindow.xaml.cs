using System;
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
        private string userType; // Variable pour stocker le type d'utilisateur

        public MainWindow(string userType)
        {
            InitializeComponent();
            this.userType = userType; // On récupère le type d'utilisateur passé lors de l'ouverture de la fenêtre

            // Si l'utilisateur est un "Client", ouvrir MainWindowClient.xaml, sinon MainWindow.xaml
            OuvrirFenetreAppropriee();
        }

        private void OuvrirFenetreAppropriee()
        {
            if (userType == "Gestionnaire")
            {
                WindowAdmin adminWindows = new WindowAdmin();
                adminWindows.Show();
                this.Close(); // Ferme la fenêtre actuelle (MainWindow)

            }
            else if (userType == "Client")
            {
                // Si l'utilisateur est un client, on ouvre la fenêtre dédiée pour les clients
                WindowClient clientWindow = new WindowClient();
                clientWindow.Show();
                this.Close(); // Ferme la fenêtre actuelle (MainWindow)
            }
            else
            {
                MessageBox.Show("Type d'utilisateur non reconnu.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}