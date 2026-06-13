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
    /// Логика взаимодействия для MenuGhostPage.xaml
    /// </summary>
    public partial class MenuGhostPage : Page
    {
        public MenuGhostPage()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем продукты из вашей базы данных и передаем в ListView
                var list = ConnectionClass.db.product.ToList();

                if (list.Count == 0)
                {
                    MessageBox.Show("База данных подключена успешно, но в таблице 'product' нет записей!", "Внимание");
                }

                DataGrProduct.ItemsSource = list;
            }
            catch (Exception ex)
            {
                // Если возникнет ошибка подключения к SQL Server - программа не зависнет, а покажет её текст
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}\n\n{ex.InnerException?.Message}", "Ошибка");
            }
        }
    }
}
