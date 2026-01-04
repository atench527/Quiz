using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using QuizApplication;

namespace QuizApplication.Tests
{
    [TestClass]
    public class QuizTests
    {
        // Tests constructor assigns values and ID
        [TestMethod]
        public void QuizConstructor_ShouldSetValues()
        {
            List<int> questionIds = new List<int> { 1, 2, 3 };
            Quiz quiz = new Quiz("Test Quiz", 5, questionIds);

            Assert.AreEqual("Test Quiz", quiz.GetQuizName());
            Assert.AreEqual(5, quiz.GetCategoryId());
            Assert.AreEqual(questionIds, quiz.GetQuestionIds());
            Assert.IsTrue(quiz.GetQuizId() > 0);
        }

        // Tests constructor with specific ID
        [TestMethod]
        public void QuizConstructor_WithId_ShouldSetIdCorrectly()
        {
            Quiz quiz = new Quiz(10, "Quiz", 2, new List<int>());

            Assert.AreEqual(10, quiz.GetQuizId());
            Assert.AreEqual("Quiz", quiz.GetQuizName());
        }

        // Tests auto-increment of quiz ID
        [TestMethod]
        public void QuizConstructor_ShouldIncrementId()
        {
            Quiz q1 = new Quiz("Q1", 1, null);
            Quiz q2 = new Quiz("Q2", 1, null);

            Assert.AreEqual(q1.GetQuizId() + 1, q2.GetQuizId());
        }

        // Tests setting quiz name
        [TestMethod]
        public void SetQuizName_ShouldUpdateName()
        {
            Quiz quiz = new Quiz("Old", 1, null);

            quiz.SetQuizName("New");

            Assert.AreEqual("New", quiz.GetQuizName());
        }

        // Tests setting category ID
        [TestMethod]
        public void SetCategoryId_ShouldUpdateCategoryId()
        {
            Quiz quiz = new Quiz("Quiz", 1, null);

            quiz.SetCategoryId(99);

            Assert.AreEqual(99, quiz.GetCategoryId());
        }

        // Tests setting question IDs
        [TestMethod]
        public void SetQuestionIds_ShouldUpdateList()
        {
            Quiz quiz = new Quiz("Quiz", 1, null);
            List<int> ids = new List<int> { 5, 6 };

            quiz.SetQuestionIds(ids);

            Assert.AreEqual(ids, quiz.GetQuestionIds());
            Assert.AreEqual(2, quiz.GetQuestionIds().Count);
        }

        // Tests null question IDs defaults to empty list
        [TestMethod]
        public void SetQuestionIds_Null_ShouldSetEmptyList()
        {
            Quiz quiz = new Quiz("Quiz", 1, null);

            quiz.SetQuestionIds(null);

            Assert.AreEqual(0, quiz.GetQuestionIds().Count);
        }
    }
}
