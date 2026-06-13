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
    /// Логика взаимодействия для MenuUserPage.xaml
    /// </summary>
    public partial class MenuUserPage : Page
    {
        public MenuUserPage()
        {
            InitializeComponent();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var selectedProduct = DataGrProduct.SelectedItem as product;

            if (selectedProduct == null)
            {
                MessageBox.Show("Пожалуйста, выберите товар из списка для удаления!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Обязательно спрашиваем подтверждение (требование на экзаменах WorldSkills)
            MessageBoxResult result = MessageBox.Show(
                $"Вы действительно хотите удалить товар \"{selectedProduct.Name_pr}\"?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // 3. Удаляем выбранный товар из контекста БД
                    ConnectionClass.db.product.Remove(selectedProduct);

                    // 4. Сохраняем изменения в самой базе данных
                    ConnectionClass.db.SaveChanges();

                    MessageBox.Show("Товар успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // 5. Обновляем ListView, чтобы удаленный товар сразу исчез с экрана
                    DataGrProduct.ItemsSource = ConnectionClass.db.product.ToList();
                }
                catch (Exception ex)
                {
                    // Если товар используется в заказах или других таблицах, SQL не даст его удалить.
                    // Этот блок перехватит ошибку и программа не вылетит.
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}\n\nВозможно, этот товар связан с другими таблицами (например, заказами).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
