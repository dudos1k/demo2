using demo2.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        // === Объявление на уровне КЛАССА ===
        private ICollectionView _productsView; // <<< ВОТ ОНА
       
        public MenuUserPage()
        {
            InitializeComponent();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем продукты из вашей базы данных и передаем в ListView
                var lisst = ConnectionClass.db.product.ToList();

                if (lisst.Count == 0)
                {
                    MessageBox.Show("База данных подключена успешно, но в таблице 'product' нет записей!", "Внимание");
                }

                DataGrProduct.ItemsSource = lisst;
            }
            catch (Exception ex)
            {
                // Если возникнет ошибка подключения к SQL Server - программа не зависнет, а покажет её текст
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}\n\n{ex.InnerException?.Message}", "Ошибка");
            }

            var list = ConnectionClass.db.product.ToList();
            _productsView = CollectionViewSource.GetDefaultView(list);
            DataGrProduct.ItemsSource = _productsView;

        }


        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e) //сортировка
        {
            if (_productsView == null) return;

            _productsView.SortDescriptions.Clear();

            switch (ComboSort.SelectedIndex)
            {
                case 1: _productsView.SortDescriptions.Add(new SortDescription("Name_pr", ListSortDirection.Ascending)); break;
                case 2: _productsView.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Ascending)); break;
                case 3: _productsView.SortDescriptions.Add(new SortDescription("Discount", ListSortDirection.Descending)); break;
            }

            _productsView.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddProductPage());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var selectedProduct = DataGrProduct.SelectedItem as product;

            if (selectedProduct == null)
            {
                MessageBox.Show("Сначала выберите товар из списка!");
                return;
            }

            // Открываем страницу
            NavigationService.Navigate(new EditProductPage(selectedProduct));

            // Сбрасываем выделение после перехода, чтобы в следующий раз 
            // кнопка сработала только после нового клика
            DataGrProduct.SelectedItem = null;
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
