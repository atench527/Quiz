using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApplication
{


    public class Question
    {
        private static int nextQuestionID = 1;

        // Private fields
        private int questionID;
        private string questionText;
        private List<string> questionOptions;
        private string questionCorrectAnswer;
        private string questionDifficultyLevel;

        // Constructor
        public Question(string text, List<string> options, string correctAnswer, string difficultyLevel)
        {
            questionID = nextQuestionID++;
            questionText = text;
            questionOptions = options;
            questionCorrectAnswer = correctAnswer;
            questionDifficultyLevel = difficultyLevel;
        }

        // Constructor with specific ID
        public Question(int id, string text, List<string> options, string correctAnswer, string difficultyLevel)
        {
            questionID = id;
            questionText = text;
            questionOptions = options;
            questionCorrectAnswer = correctAnswer;
            questionDifficultyLevel = difficultyLevel;

            if (id >= nextQuestionID)
            {
                nextQuestionID = id + 1;
            }
        }

        // Getter methods
        public int GetQuestionID()
        {
            return questionID;
        }

        public string GetQuestionText()
        {
            return questionText;
        }

        public List<string> GetQuestionOptions()
        {
            return questionOptions;
        }

        public string GetQuestionCorrectAnswer()
        {
            return questionCorrectAnswer;
        }

        public string GetQuestionDifficultyLevel()
        {
            return questionDifficultyLevel;
        }

        // Setter methods
        public void SetQuestionText(string text)
        {
            questionText = text;
        }

        public void SetQuestionOptions(List<string> options)
        {
            questionOptions = options;
        }

        public void SetQuestionCorrectAnswer(string correctAnswer)
        {
            questionCorrectAnswer = correctAnswer;
        }

        public void SetQuestionDifficultyLevel(string difficultyLevel)
        {
            questionDifficultyLevel = difficultyLevel;
        }

        // Display method
        public void DisplayQuestion()
        {
            Console.WriteLine("\nQuestion " + questionID + ": " + questionText);
            Console.WriteLine("Difficulty: " + questionDifficultyLevel);

            for (int i = 0; i < questionOptions.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + questionOptions[i]);
            }
        }

        // Check answer method
        public bool CheckAnswer(string answer)
        {
            if (answer == questionCorrectAnswer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }







}







