using System;
using System.Collections.Generic;

namespace QuizApplication
{
    internal class Quiz
    {
        private static int nextQuizId = 1;

        private int quizId;
        private string quizName;
        private int categoryId;
        private List<int> questionIds;

        public Quiz(string name, int categoryId, List<int> questionIds)
        {
            quizId = nextQuizId++;
            quizName = name;
            this.categoryId = categoryId;
            this.questionIds = questionIds ?? new List<int>();
        }

        public Quiz(int id, string name, int categoryId, List<int> questionIds)
        {
            quizId = id;
            quizName = name;
            this.categoryId = categoryId;
            this.questionIds = questionIds ?? new List<int>();

            if (id >= nextQuizId)
                nextQuizId = id + 1;
        }

        public int GetQuizId() => quizId;
        public string GetQuizName() => quizName;
        public int GetCategoryId() => categoryId;
        public List<int> GetQuestionIds() => questionIds;

        public void SetQuizName(string name) => quizName = name;
        public void SetCategoryId(int id) => categoryId = id;
        public void SetQuestionIds(List<int> ids) => questionIds = ids ?? new List<int>();
    }
}
