using demo2.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace demo2.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var CurrentUser = ConnectionClass.db.users.FirstOrDefault(u => u.Login == LoginTxt.Text && u.Password == PasswordTxt.Text);
            if (CurrentUser != null)
            {
                if(CurrentUser.ID_role == 2) //пользователь
                {
                    NavigationService.Navigate(new MenuUserPage());
                }
                else if (CurrentUser.ID_role == 3) //админ
                {
                    NavigationService.Navigate(new MenuAdminPage());
                }
            }
            else
            {
                MessageBox.Show("Пользователь не зарегестрирован!","Ошибка");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MenuGhostPage());
        }
    }
}
