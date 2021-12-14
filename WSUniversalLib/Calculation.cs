using System;

namespace WSUniversalLib
{
    public static class Calculation
    {
        private static readonly float[] coefficients = new float[] { 1.1f, 2.5f, 8.43f };

        private static readonly float[] defects = new float[] { 0.3f, 0.12f };

        public static int GetQuantityForProduct(int productType, int materialType, int count, float width, float length)
        {
            // 1, 1, 2, 15.564, 20
            // 1, 1, 2, 15, 20.56568
            // 2, 1, 2, 15.564, 20
            // 1, 2, 2, 15, 20.56568
            // 1, 1, 2, 15.564, 20.56568

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
            // count * width * length * 1.1 / 0.997
        }
    }
}
