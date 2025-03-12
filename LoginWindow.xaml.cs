using System;
using System.Windows;
using MySql.Data.MySqlClient;
using BCrypt.Net;  // Assurez-vous que vous avez installé le package BCrypt.Net-Next

namespace MarieTeamBrochure
{
    public partial class LoginWindow : Window
    {
        private string connectionString = "Server=localhost;Port=3306;Database=marieteam;Uid=root;Pwd=;";

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            var userType = AuthenticateUser(username, password);

            if (userType != null)
            {
                // Si l'utilisateur est authentifié, ouvrir la fenêtre principale avec le type d'utilisateur
                MainWindow mainWindow = new MainWindow(userType);
                this.Close(); // Ferme la fenêtre de connexion
            }
            else
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string AuthenticateUser(string username, string password)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT mdp_user, typer_user FROM utilisateur WHERE mail_user = @username"; // On récupère aussi le type d'utilisateur

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);

                try
                {
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string storedHashedPassword = reader["mdp_user"].ToString();
                        string userType = reader["typer_user"].ToString(); // Récupère le type d'utilisateur

                        // Vérifier si le mot de passe saisi correspond au mot de passe haché
                        if (BCrypt.Net.BCrypt.Verify(password, storedHashedPassword))
                        {
                            return userType; // Retourne le type d'utilisateur
                        }
                    }

                    return null; // Si l'utilisateur n'existe pas ou le mot de passe est incorrect
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la connexion à la base de données: " + ex.Message);
                    return null;
                }
            }
        }
    }
}
