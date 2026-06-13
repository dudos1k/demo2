using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using demo2.DbContext; // Подключение папки с базой данных

namespace demo2.Pages
{
    public partial class EditProductPage : Page
    {
        private product _currentProduct; // Переменная для хранения редактируемого товара

        // Конструктор принимает выбранный товар
        public EditProductPage(product selectedProduct)
        {
            InitializeComponent();

            _currentProduct = selectedProduct; // Сохраняем переданный товар
            FillFormFields();                  // Заполняем поля данными товара
        }

        // Метод для заполнения полей формы
        private void FillFormFields()
        {
            TxtName.Text = _currentProduct.Name_pr;
            TxtArticul.Text = _currentProduct.Articul.ToString();
            TxtPrice.Text = _currentProduct.Price.ToString();
            TxtDiscount.Text = _currentProduct.Discount.ToString();
            TxtDescription.Text = _currentProduct.Discription; // 'Discription' с буквой 'i'
        }

        // Кнопка сохранения изменений
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Проверка ввода (Валидация)
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TxtName.Text))
                errors.AppendLine("Введите наименование товара!");

            if (!int.TryParse(TxtArticul.Text, out int articul))
                errors.AppendLine("Артикул должен быть целым числом!");

            if (!int.TryParse(TxtPrice.Text, out int price) || price < 0)
                errors.AppendLine("Цена должна быть положительным целым числом!");

            if (!int.TryParse(TxtDiscount.Text, out int discount) || discount < 0 || discount > 100)
                errors.AppendLine("Скидка должна быть числом от 0 до 100!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Записываем измененные данные обратно в объект
                _currentProduct.Name_pr = TxtName.Text;
                _currentProduct.Articul = articul;
                _currentProduct.Price = price;
                _currentProduct.Discount = discount;
                _currentProduct.Discription = TxtDescription.Text;

                // Сохраняем изменения в базе данных
                ConnectionClass.db.SaveChanges();

                MessageBox.Show("Данные товара успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Возвращаемся на страницу со списком
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения изменений:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Кнопка отмены
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}