using System;
using System.Collections.Generic;

namespace QuizApplication
{
    internal class Results
    {
        private static int nextResultsId = 1;

        private int resultsId;
        private int userId;
        private int quizId;
        private int mark;
        private List<string> wrongAnswers;

        public Results(int userId, int quizId)
        {
            resultsId = nextResultsId++;
            this.userId = userId;
            this.quizId = quizId;
            mark = 0;
            wrongAnswers = new List<string>();
        }

        public Results(int resultsId, int userId, int quizId, int mark, List<string> wrongAnswers)
        {
            this.resultsId = resultsId;
            this.userId = userId;
            this.quizId = quizId;
            this.mark = mark;
            this.wrongAnswers = wrongAnswers ?? new List<string>();

            if (resultsId >= nextResultsId)
                nextResultsId = resultsId + 1;
        }

        public int GetResultsId() => resultsId;
        public int GetUserId() => userId;
        public int GetQuizId() => quizId;
        public int GetMark() => mark;
        public List<string> GetWrongAnswers() => wrongAnswers;

        public void AddWrongAnswer(string description)
        {
            wrongAnswers.Add(description ?? "");
        }

        public int CalculateMark(int noOfQuestions)
        {
            mark = noOfQuestions - wrongAnswers.Count;
            if (mark < 0) mark = 0;
            return mark;
        }

        public void ShowResults()
        {
            Console.WriteLine("\n===== RESULTS =====");
            Console.WriteLine($"Quiz ID: {quizId}");
            Console.WriteLine($"User ID: {userId}");
            Console.WriteLine($"Mark: {mark}");
            ShowWrongAnswers();
            Console.WriteLine("===================\n");
        }

        public void ShowWrongAnswers()
        {
            if (wrongAnswers.Count == 0)
            {
                Console.WriteLine("Wrong Answers: None 🎉");
                return;
            }

            Console.WriteLine("Wrong Answers:");
            foreach (string wa in wrongAnswers)
                Console.WriteLine("- " + wa);
        }
    }
}
