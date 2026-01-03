using System;
using System.Collections.Generic;

namespace QuizApplication
{
    internal class Student : User
    {
        private string status;
        private readonly Dictionary<int, Results> resultsByQuizId;

        public Student(string userName, string userPassword, string userEmail)
            : base(userName, userPassword, userEmail, "Student")
        {
            status = "Active";
            resultsByQuizId = new Dictionary<int, Results>();
        }

        public Student(int userId, string userName, string userPassword, string userEmail)
            : base(userId, userName, userPassword, userEmail, "Student")
        {
            status = "Active";
            resultsByQuizId = new Dictionary<int, Results>();
        }

        public string GetStatus() => status;
        public void SetStatus(string newStatus) => status = newStatus;

        public Results PlayQuiz(int quizId, List<Question> questions)
        {
            if (questions == null || questions.Count == 0)
            {
                Console.WriteLine("No questions were provided for this quiz.");
                return null;
            }

            Results attempt = new Results(GetID(), quizId);

            Console.WriteLine($"\nStarting Quiz {quizId} for {GetUsername()}");
            Console.WriteLine("----------------------------------");

            for (int i = 0; i < questions.Count; i++)
            {
                Question q = questions[i];
                q.DisplayQuestion();

                Console.Write("Your answer: ");
                string userAnswer = Console.ReadLine() ?? "";

                bool correct = q.CheckAnswer(userAnswer);

                if (!correct)
                {
                    attempt.AddWrongAnswer(
                        $"Q{q.GetQuestionID()} - You: \"{userAnswer}\" | Correct: \"{q.GetQuestionCorrectAnswer()}\""
                    );
                }
            }

            attempt.CalculateMark(questions.Count);
            resultsByQuizId[quizId] = attempt;

            return attempt;
        }

        public void RestartQuiz(int quizId)
        {
            if (resultsByQuizId.Remove(quizId))
                Console.WriteLine($"Saved results for quiz {quizId} cleared.");
            else
                Console.WriteLine($"No saved results found for quiz {quizId}.");
        }

        public Results GetSavedResults(int quizId)
        {
            resultsByQuizId.TryGetValue(quizId, out Results res);
            return res;
        }

        public List<Results> GetAllSavedResults()
        {
            return new List<Results>(resultsByQuizId.Values);
        }
    }
}
