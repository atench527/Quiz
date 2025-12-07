using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using System;
using System.Collections.Generic;

namespace QuizApplication



{


    public class Category
    {
        private static int nextCategoryID = 1;

        // Private fields
        private int categoryID;
        private string categoryName;
        private string categoryDescription;

        // Constructor
        public Category(string name, string description)
        {
            categoryID = nextCategoryID++;
            categoryName = name;
            categoryDescription = description;
        }

        // Constructor with specific ID (for loading from file)
        public Category(int id, string name, string description)
        {
            categoryID = id;
            categoryName = name;
            categoryDescription = description;

            if (id >= nextCategoryID)
            {
                nextCategoryID = id + 1;
            }
        }

        // Getter methods
        public int GetCategoryID()
        {
            return categoryID;
        }

        public string GetCategoryName()
        {
            return categoryName;
        }

        public string GetCategoryDescription()
        {
            return categoryDescription;
        }

        // Setter methods
        public void SetCategoryName(string name)
        {
            categoryName = name;
        }

        public void SetCategoryDescription(string description)
        {
            categoryDescription = description;
        }

        // Display method
        public void DisplayCategory()
        {
            Console.WriteLine("Category ID: " + categoryID);
            Console.WriteLine("Name: " + categoryName);
            Console.WriteLine("Description: " + categoryDescription);
        }
    }













}
