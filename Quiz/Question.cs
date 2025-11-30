using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApplication
{
    public class Question



    {


        private int questionID;
        private string questionText;
        private List<string> options;
        private string correctAnswer;

        // ===== PROPERTIES =====
        public int QuestionID
        {
            get { return questionID; }
            set { questionID = value; }
        }

        public string QuestionText
        {
            get { return questionText; }
            set { questionText = value; }
        }

        public List<string> Options
        {
            get { return options; }
            set { options = value; }
        }

        public string CorrectAnswer
        {
            get { return correctAnswer; }
            set { correctAnswer = value; }
        }

        // ===== DEFAULT CONSTRUCTOR =====
        public Question()
        {
            questionID = 0;
            questionText = "";
            options = new List<string>();
            correctAnswer = "";
        }

        // ===== CUSTOM CONSTRUCTOR =====
        public Question(int id, string text, List<string> opts, string answer)
        {
            questionID = id;
            questionText = text;
            options = opts;
            correctAnswer = answer;
        }

        // ===== METHODS =====

        public void ShowQuestion()
        {
            Console.WriteLine(questionText);
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine((i + 1) + ") " + options[i]);
            }
        }

        public bool CheckAnswer(string userAnswer)
        {
            return userAnswer.Trim().Equals(correctAnswer.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public void DisplayQuestion()
        {
            Console.WriteLine("Q" + questionID + ": " + questionText);
        }

        public void UpdateQuestion(string newText)
        {
            questionText = newText;
            Console.WriteLine("Question updated.");
        }

        public void DeleteQuestion()
        {
            Console.WriteLine("Question " + questionID + " deleted.");
        }
    }


}







