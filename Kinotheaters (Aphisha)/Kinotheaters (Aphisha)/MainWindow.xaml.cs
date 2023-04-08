using System.Windows;
using System.Data.OleDb;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace Kinotheaters__Aphisha_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string sqlQuery;
        string connectString;
        List<string> photoPaths = new List<string>();
        int index = 1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            try
            {
                connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=LoginDatabase.mdb";
                OleDbConnection connection = new OleDbConnection(connectString);
                if(UserCheckBox.IsChecked == true)
                {
                    sqlQuery = $"INSERT INTO DataForLogin (Логин, Пароль, Роль) VALUES ('{username_tb.Text}', '{pass_tb.Password}', 'user')";
                }
                else if(AdminCheckBox.IsChecked == true)
                {
                    sqlQuery = $"INSERT INTO DataForLogin (Логин, Пароль, Роль) VALUES ('{username_tb.Text}', '{pass_tb.Password}', 'admin')";
                }
                OleDbCommand command = new OleDbCommand(sqlQuery, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Вы зарегистрированы!");
                auth_view.Visibility = Visibility.Hidden;
                login_view.Visibility = Visibility.Visible;
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private void LoginToAuth(object sender, RoutedEventArgs e)
        {
            login_view.Visibility = Visibility.Hidden;
            auth_view.Visibility = Visibility.Visible;
        }

        private void Registration(object sender, RoutedEventArgs e)
        {
            connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=LoginDatabase.mdb";
            OleDbConnection connection = new OleDbConnection(connectString);
            sqlQuery = "SELECT * FROM DataForLogin";
            OleDbCommand command = new OleDbCommand(sqlQuery, connection);
            connection.Open();
            OleDbDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                if (login_tb.Text.ToLower() == reader["Логин"].ToString().ToLower() && password_tb.Password.ToLower() == reader["Пароль"].ToString().ToLower())
                {
                    switch (reader["Роль"].ToString().ToLower())
                    {
                        case "user":
                            login_view.Visibility = Visibility.Hidden;
                            user_view.Visibility = Visibility.Visible;
                            break;
                        case "admin":
                            login_view.Visibility = Visibility.Hidden;
                            admin_view.Visibility = Visibility.Visible;
                            break;
                    }
                }
            }
            connection.Close();
        }

        private void AddPosterToList(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Выбери картинку";
            fileDialog.Filter = "Image files (*.jpg)|*.jpg";
            if(fileDialog.ShowDialog() == true)
            {
                photoPaths.Add(fileDialog.FileName);
                MessageBox.Show("Вы добавили изображение в хранилище!");
            }
        }

        private void LogoutAdmin(object sender, RoutedEventArgs e)
        {
            admin_view.Visibility = Visibility.Hidden;
            login_view.Visibility = Visibility.Visible;
        }

        private void SlideDown(object sender, RoutedEventArgs e)
        {
            try
            {
                index--;
                imgPhoto.Source = new BitmapImage(new Uri(photoPaths[index]));
            }
            catch
            {
                MessageBox.Show("Может быть вы вернулись уже в начало!");
            }
        }

        private void SlideUp(object sender, RoutedEventArgs e)
        {
            try
            {
                index++;
                imgPhoto.Source = new BitmapImage(new Uri(photoPaths[index]));
                imgPhoto.Height = 148;
                imgPhoto.VerticalAlignment = VerticalAlignment.Top;
            }
            catch
            {
                MessageBox.Show("Может быть вы вернулись уже в конец!");
            }
        }

        private void UserToLogin(object sender, RoutedEventArgs e)
        {
            user_view.Visibility = Visibility.Hidden;
            login_view.Visibility = Visibility.Visible;
        }
    }
}
