using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSUniversalLib;

namespace Tests
{
    [TestClass]
    public class CalculationTests
    {
        /// <summary>
        /// Проверка с верными целочисленными данными
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_CorrectData()
        {
            Assert.AreEqual(114148, Calculation.GetQuantityForProduct(3, 1, 15, 20, 45));
        }

        /// <summary>
        /// Проверка с несуществующим типом продукта
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_NonExistentProductType()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(4, 1, 15, 20, 45));
        }

        /// <summary>
        /// Проверка с несуществующим типом материала
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_NonExistentMaterialType()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(3, 4, 15, 20, 45));
        }

        /// <summary>
        /// Проверка с отрицательным количеством
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_NegativeCount()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(3, 1, -15, 20, 45));
        }

        /// <summary>
        /// Проверка с нулевым количеством
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_ZeroCount()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(3, 1, 0, 20, 45));
        }

        /// <summary>
        /// Проверка с отрицательной шириной
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_NegativeWidth()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(3, 1, 15, -20, 45));
        }

        /// <summary>
        /// Проверка с нулевой шириной
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_ZeroWidth()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(3, 1, 15, 0, 45));
        }

        /// <summary>
        /// Проверка с отрицательной длиной
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_NegativeLength()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(3, 1, 15, 20, -45));
        }

        /// <summary>
        /// Проверка с нулевой длиной
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_ZeroLength()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(3, 1, 15, 20, 0));
        }

        /// <summary>
        /// Проверка с отрицательным типом продукта
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_NegativeProductType()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(-4, 1, 15, 20, 45));
        }

        /// <summary>
        /// Проверка с отрицательным типом материала
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_NegativeMaterialType()
        {
            Assert.AreEqual(-1, Calculation.GetQuantityForProduct(4, -1, 15, 20, 45));
        }

        /// <summary>
        /// Проверка с шириной - числом с плавающей точкой
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_FloatWidth()
        {
            Assert.AreEqual(687, Calculation.GetQuantityForProduct(1, 1, 2, 15.564f, 20));
        }

        /// <summary>
        /// Проверка с длиной - числом с плавающей точкой
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_FloatLength()
        {
            Assert.AreEqual(681, Calculation.GetQuantityForProduct(1, 1, 2, 15, 20.56568f));
        }

        /// <summary>
        /// Проверка с шириной - числом с плавающей точкой и типом продукта 2
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_FloatWidthProductType2()
        {
            Assert.AreEqual(1562, Calculation.GetQuantityForProduct(2, 1, 2, 15.564f, 20));
        }

        /// <summary>
        /// Проверка с шириной - числом с плавающей точкой и типом материала 2
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_FloatLengthProductType2()
        {
            Assert.AreEqual(680, Calculation.GetQuantityForProduct(1, 2, 2, 15, 20.56568f));
        }

        /// <summary>
        /// Проверка с шириной и длиной - числами с плавающей точкой
        /// </summary>
        [TestMethod]
        public void GetQuantityForProduct_FloatLengthFloatWidth()
        {
            Assert.AreEqual(707, Calculation.GetQuantityForProduct(1, 1, 2, 15.564f, 20.56568f));
        }
    }
}
