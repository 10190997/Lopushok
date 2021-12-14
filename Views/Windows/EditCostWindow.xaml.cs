using Lopushok.Model;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Lopushok.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditCostWindow.xaml
    /// </summary>
    public partial class EditCostWindow : Window
    {
        private readonly List<Product> products;

        /// <summary>
        /// Конструктор окна
        /// </summary>
        /// <param name="products"></param>
        public EditCostWindow(List<Product> products)
        {
            InitializeComponent();
            this.products = products;
            decimal avg = 0;
            foreach (var item in products)
            {
                avg += item.MinCostForAgent;
            }
            avg /= products.Count;
            tbCost.Text = Math.Round(avg, 2).ToString();
        }

        /// <summary>
        /// После нажатия кнопки “Изменить” стоимость выделенных продуктов должна быть увеличена на введенное число
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(tbCost.Text, out decimal cost))
            {
                foreach (var item in products)
                {
                    item.MinCostForAgent += cost;
                }
                DB.entities.SaveChanges();
            }
            else
            {
                MessageBox.Show("Проверьте правильность числовых данных");
            }
        }
    }
}
