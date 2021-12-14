using Lopushok.Model;
using Lopushok.Views.Windows;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Lopushok.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Product Product { get; set; }

        /// <summary>
        /// Конструктор страницы
        /// </summary>
        /// <param name="product">Объект класса Продукт</param>
        public AddEditPage(Product product)
        {
            InitializeComponent();
            if (product != null)
            {
                Product = product;
            }
            btnDelete.Visibility = product.ID != 0 ? Visibility.Visible : Visibility.Collapsed;
            DataContext = product;
            cbType.ItemsSource = DB.entities.ProductTypes.ToList();
        }

        /// <summary>
        /// Показывает окно со всеми картинками в папке products
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            var window = new ImagesWindow();
            window.ShowDialog();

            if (window.DialogResult == true)
            {
                Product.Image = window.ImgUri;
                // Обновить контекст, чтобы поменялась картинка
                DataContext = null;
                DataContext = Product;
            }
        }

        /// <summary>
        /// Сохранить в БД с проверкой корректности данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();

            if (string.IsNullOrEmpty(tbTitle.Text) || string.IsNullOrWhiteSpace(tbTitle.Text))
            {
                errors.AppendLine("Введите название");
            }
            if (string.IsNullOrEmpty(tbArticle.Text) || string.IsNullOrWhiteSpace(tbArticle.Text))
            {
                errors.AppendLine("Введите артикул");
            }

            else if (Product.ID == 0)
            {
                var articles = DB.entities.Products.ToList();
                foreach (var item in articles)
                {
                    if (tbArticle.Text == item.ArticleNumber)
                    {
                        errors.AppendLine("Артикул должен быть уникальным");
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(tbMinCostForAgent.Text) || string.IsNullOrWhiteSpace(tbMinCostForAgent.Text))
            {
                errors.AppendLine("Введите минимальную стоимость для агента");
            }
            else
            {
                if (!decimal.TryParse(tbMinCostForAgent.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                {
                    errors.AppendLine("Введите верную стоимость");
                }
            }
            if (!string.IsNullOrEmpty(tbPersonCount.Text))
            {
                if (!int.TryParse(tbPersonCount.Text, out _))
                {
                    errors.AppendLine("Введите корректное количество человек");
                }
            }
            if (!string.IsNullOrEmpty(tbWorkshopNumber.Text))
            {
                if (!int.TryParse(tbWorkshopNumber.Text, out _))
                {
                    errors.AppendLine("Введите корректный номер роизводственного цеха");
                }
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (Product.ID == 0)
            {
                DB.entities.Products.Add(Product);
            }
            DB.entities.SaveChanges();
            MessageBox.Show("Успешно сохранено");
            NavigationService.GoBack();
        }

        /// <summary>
        /// Удалить из БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (Product.ProductSales.Count > 0)
                {
                    MessageBox.Show("У продукта есть информация о его продажах агентами. Удаление запрещено.");
                    return;
                }
                // Если у продукта есть информация о материалах, используемых при его производстве,
                // или история изменения цен, то эта информация должна быть удалена вместе с продуктом.

                if (Product.MaterialsList != null)
                {
                    var pms = Product.ProductMaterials.ToList();
                    foreach (var item in pms)
                    {
                        DB.entities.ProductMaterials.Remove(item);
                    }
                }
                if (Product.ProductCostHistories.Count != 0)
                {
                    var pchs = Product.ProductCostHistories.ToList();
                    foreach (var item in pchs)
                    {
                        DB.entities.ProductCostHistories.Remove(item);
                    }
                }
                DB.entities.Products.Remove(Product);
                DB.entities.SaveChanges();
                MessageBox.Show("Успешно удалено");
                NavigationService.GoBack();
            }
        }
    }
}
