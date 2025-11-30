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
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public List<Quiz> QuizList { get; set; }

        public Category()
        {
            CategoryID = 0;
            CategoryName = "";
            QuizList = new List<Quiz>();
        }

        public Category(int id, string name)
        {
            CategoryID = id;
            CategoryName = name;
            QuizList = new List<Quiz>();
        }

        public void DisplayList()
        {
            Console.WriteLine("Category: " + CategoryName);

            if (QuizList.Count == 0)
            {
                Console.WriteLine("   No quizzes in this category.");
                return;
            }

            foreach (Quiz q in QuizList)
            {
                Console.WriteLine("   - " + q.QuizName);
            }
        }
    }
}
