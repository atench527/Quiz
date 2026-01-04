using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuizApplication;

namespace QuizApplication.Tests
{
    [TestClass]
    public class CategoryTests
    {
        // Tests constructor assigns name, description, and ID
        [TestMethod]
        public void CategoryConstructor_ShouldSetValues()
        {
            Category category = new Category("Maths", "Math related questions");

            Assert.AreEqual("Maths", category.GetCategoryName());
            Assert.AreEqual("Math related questions", category.GetCategoryDescription());
            Assert.IsTrue(category.GetCategoryID() > 0);
        }

        // Tests constructor with specific ID
        [TestMethod]
        public void CategoryConstructor_WithId_ShouldSetIdCorrectly()
        {
            Category category = new Category(10, "Science", "Science questions");

            Assert.AreEqual(10, category.GetCategoryID());
            Assert.AreEqual("Science", category.GetCategoryName());
        }

        // Tests auto-increment of category ID
        [TestMethod]
        public void CategoryConstructor_ShouldIncrementId()
        {
            Category c1 = new Category("Cat1", "Desc1");
            Category c2 = new Category("Cat2", "Desc2");

            Assert.AreEqual(c1.GetCategoryID() + 1, c2.GetCategoryID());
        }

        // Tests setting category name
        [TestMethod]
        public void SetCategoryName_ShouldUpdateName()
        {
            Category category = new Category("Old", "Desc");

            category.SetCategoryName("New");

            Assert.AreEqual("New", category.GetCategoryName());
        }

        // Tests setting category description
        [TestMethod]
        public void SetCategoryDescription_ShouldUpdateDescription()
        {
            Category category = new Category("Name", "Old Desc");

            category.SetCategoryDescription("New Desc");

            Assert.AreEqual("New Desc", category.GetCategoryDescription());
        }

        // Tests display method does not throw an exception
        [TestMethod]
        public void DisplayCategory_ShouldNotThrowException()
        {
            Category category = new Category("Test", "Test Desc");

            category.DisplayCategory();
        }
    }
}
