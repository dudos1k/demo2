using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using demo2.DbContext; // Подключаем доступ к базе данных и ConnectionClass

namespace demo2.Pages
{
    public partial class AddProductPage : Page
    {
        public AddProductPage()
        {
            InitializeComponent();
        }

        // Кнопка сохранения
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // 1. ПРОВЕРКА ВВОДА (Валидация)
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TxtName.Text))
                errors.AppendLine("Введите наименование товара!");

            if (!int.TryParse(TxtArticul.Text, out int articul))
                errors.AppendLine("Артикул должен быть целым числом!");

            if (!int.TryParse(TxtPrice.Text, out int price) || price < 0)
                errors.AppendLine("Цена должна быть положительным целым числом!");

            if (!int.TryParse(TxtDiscount.Text, out int discount) || discount < 0 || discount > 100)
                errors.AppendLine("Скидка должна быть числом от 0 до 100!");

            // Если есть ошибки — выводим их пользователю и прекращаем сохранение
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 2. СОЗДАНИЕ ОБЪЕКТА ТОВАРА
            product newProduct = new product
            {
                Name_pr = TxtName.Text,
                Articul = articul,
                Price = price,
                Discount = discount,
                Discription = TxtDescription.Text // Поле 'Discription' пишем с опечаткой 'i', как в вашей БД!
            };

            // 3. ЗАПИСЬ В БАЗУ ДАННЫХ
            try
            {
                ConnectionClass.db.product.Add(newProduct); // Добавляем в таблицу локально
                ConnectionClass.db.SaveChanges();           // Отправляем изменения в SQL Server

                MessageBox.Show("Товар успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Возвращаемся обратно на страницу со списком
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка базы данных при сохранении:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Кнопка отмены (просто возврат назад)
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}