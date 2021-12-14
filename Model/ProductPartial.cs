using System;
using System.Linq;

namespace Lopushok.Model
{
    public partial class Product
    {
        /// <summary>
        /// Ссылка на картинку с учетом заглушки
        /// </summary>
        public string PicturePath
        {
            get => Image ?? "../../products/picture.png";
            set { }
        }
        /// <summary>
        /// Стоимость продукта рассчитана исходя из используемых материалов
        /// </summary>
        public decimal Price
        {
            get
            {
                decimal price = 0;
                foreach (var item in ProductMaterials)
                {
                    price += item.Material.Cost;
                }
                return price;
            }
            set { }
        }
        /// <summary>
        /// Список материалов продукта
        /// </summary>
        public string MaterialsList
        {
            get
            {
                string materials = "";
                foreach (var item in ProductMaterials)
                {
                    materials += item.Material.Title;
                    if (item != ProductMaterials.Last())
                    {
                        materials += ", ";
                    }
                }
                return materials;
            }
            set { }
        }
        /// <summary>
        /// Необходимо подсвечивать светло-красным цветом те продукты, которые не продавались агентами в последний месяц
        /// </summary>
        // а продаж в бд нету!
        public string Color
        {
            get
            {
                foreach (var item in ProductSales)
                {
                    if (item.SaleDate.Month == DateTime.Today.Month)
                    {
                        return null;
                    }
                }
                return "#ff9e9e";
            }
            set { }
        }
    }
}
