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
        private Product product { get; set; }

        /// <summary>
        /// Конструктор страницы
        /// </summary>
        /// <param name="product">Объект класса Продукт</param>
        public AddEditPage(Product product)
        {
            InitializeComponent();
            if (product != null)
            {
                this.product = product;
            }
            if (product.ID != 0)
            {
                btnDelete.Visibility = Visibility.Visible;
            }
            else
            {
                btnDelete.Visibility = Visibility.Collapsed;
            }
            DataContext = product;
            cbType.ItemsSource = DB.entities.ProductTypes.ToList();
        }

        /// <summary>
        /// Показывает окно со всеми картинками в папке products
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            var window = new ImagesWindow();
            window.ShowDialog();

            if (window.DialogResult == true)
            {
                product.Image = window.ImgUri;
                // Обновить контекст, чтобы поменялась картинка
                DataContext = null;
                DataContext = product;
            }
        }

        /// <summary>
        /// Сохранить в БД с проверкой корректности данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
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

            else if (product.ID == 0)
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

            if (product.ID == 0)
            {
                DB.entities.Products.Add(product);
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
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (product.ProductSales.Count > 0)
                {
                    MessageBox.Show("У продукта есть информация о его продажах агентами. Удаление запрещено.");
                    return;
                }
                // Если у продукта есть информация о материалах, используемых при его производстве,
                // или история изменения цен, то эта информация должна быть удалена вместе с продуктом.

                if (product.MaterialsList != null)
                {
                    var pms = product.ProductMaterials.ToList();
                    foreach (var item in pms)
                    {
                        DB.entities.ProductMaterials.Remove(item);
                    }
                }
                if (product.ProductCostHistories.Count != 0)
                {
                    var pchs = product.ProductCostHistories.ToList();
                    foreach (var item in pchs)
                    {
                        DB.entities.ProductCostHistories.Remove(item);
                    }
                }
                DB.entities.Products.Remove(product);
                DB.entities.SaveChanges();
                MessageBox.Show("Успешно удалено");
                NavigationService.GoBack();
            }
        }
    }
}
