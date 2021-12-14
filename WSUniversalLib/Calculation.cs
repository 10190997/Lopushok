using System;

namespace WSUniversalLib
{
    public static class Calculation
    {
        private static readonly float[] coefficients = new float[] { 1.1f, 2.5f, 8.43f };

        private static readonly float[] defects = new float[] { 0.3f, 0.12f };

        /// <summary>
        /// Метод расчитывает целое количество сырья, необходимого для производства определенного количества продукции, учитывая возможный брак материалов.
        /// </summary>
        /// <param name="productType">Тип продукции</param>
        /// <param name="materialType">Тип материала</param>
        /// <param name="count">Количество продукции</param>
        /// <param name="width">Ширина продукта</param>
        /// <param name="length">Длина продукта</param>
        /// <returns>Количество сырья</returns>
        public static int GetQuantityForProduct(int productType, int materialType, int count, float width, float length)
        {
            // Количество необходимого качественного сырья на одну единицу продукции рассчитывается как площадь
            // продукции, умноженная на коэффициент типа продукции.
            if (productType > coefficients.Length || materialType > defects.Length
                || count <= 0 || width <= 0 || length <= 0
                || productType <= 0 || materialType <= 0)
            {
                return -1;
            }
            float square = width * length;
            float materialAmount = square * coefficients[productType - 1] / (1 - defects[materialType - 1] / 100);
            int materialCount = (int)Math.Ceiling(materialAmount * count);
            return materialCount;
        }
    }
}
