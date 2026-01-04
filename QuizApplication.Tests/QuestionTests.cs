using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using QuizApplication;

namespace QuizApplication.Tests
{
    [TestClass]
    public class QuestionTests
    {
        // Tests constructor assigns text/options/correct answer/difficulty and ID
        [TestMethod]
        public void QuestionConstructor_ShouldSetValues()
        {
            List<string> options = new List<string> { "A", "B", "C" };
            Question q = new Question("What is 1+1?", options, "B", "Easy");

            Assert.AreEqual("What is 1+1?", q.GetQuestionText());
            Assert.AreEqual(options, q.GetQuestionOptions());
            Assert.AreEqual("B", q.GetQuestionCorrectAnswer());
            Assert.AreEqual("Easy", q.GetQuestionDifficultyLevel());
            Assert.IsTrue(q.GetQuestionID() > 0);
        }

        // Tests constructor with specific ID
        [TestMethod]
        public void QuestionConstructor_WithId_ShouldSetIdCorrectly()
        {
            List<string> options = new List<string> { "A", "B" };
            Question q = new Question(25, "Q?", options, "A", "Medium");

            Assert.AreEqual(25, q.GetQuestionID());
            Assert.AreEqual("Q?", q.GetQuestionText());
        }

        // Tests auto-increment of question ID
        [TestMethod]
        public void QuestionConstructor_ShouldIncrementId()
        {
            Question q1 = new Question("T1", new List<string> { "1" }, "1", "Easy");
            Question q2 = new Question("T2", new List<string> { "1" }, "1", "Easy");

            Assert.AreEqual(q1.GetQuestionID() + 1, q2.GetQuestionID());
        }

        // Tests setting question text
        [TestMethod]
        public void SetQuestionText_ShouldUpdateText()
        {
            Question q = new Question("Old", new List<string> { "A" }, "A", "Easy");

            q.SetQuestionText("New");

            Assert.AreEqual("New", q.GetQuestionText());
        }

        // Tests setting question options
        [TestMethod]
        public void SetQuestionOptions_ShouldUpdateOptions()
        {
            Question q = new Question("Text", new List<string> { "A" }, "A", "Easy");
            List<string> newOptions = new List<string> { "X", "Y", "Z" };

            q.SetQuestionOptions(newOptions);

            Assert.AreEqual(newOptions, q.GetQuestionOptions());
            Assert.AreEqual(3, q.GetQuestionOptions().Count);
        }

        // Tests setting correct answer
        [TestMethod]
        public void SetQuestionCorrectAnswer_ShouldUpdateCorrectAnswer()
        {
            Question q = new Question("Text", new List<string> { "A", "B" }, "A", "Easy");

            q.SetQuestionCorrectAnswer("B");

            Assert.AreEqual("B", q.GetQuestionCorrectAnswer());
        }

        // Tests setting difficulty level
        [TestMethod]
        public void SetQuestionDifficultyLevel_ShouldUpdateDifficulty()
        {
            Question q = new Question("Text", new List<string> { "A" }, "A", "Easy");

            q.SetQuestionDifficultyLevel("Hard");

            Assert.AreEqual("Hard", q.GetQuestionDifficultyLevel());
        }

        // Tests CheckAnswer returns true for correct answer
        [TestMethod]
        public void CheckAnswer_Correct_ShouldReturnTrue()
        {
            Question q = new Question("Text", new List<string> { "A", "B" }, "A", "Easy");

            bool result = q.CheckAnswer("A");

            Assert.IsTrue(result);
        }

        // Tests CheckAnswer returns false for wrong answer
        [TestMethod]
        public void CheckAnswer_Wrong_ShouldReturnFalse()
        {
            Question q = new Question("Text", new List<string> { "A", "B" }, "A", "Easy");

            bool result = q.CheckAnswer("B");

            Assert.IsFalse(result);
        }

        // Tests DisplayQuestion does not throw
        [TestMethod]
        public void DisplayQuestion_ShouldNotThrowException()
        {
            Question q = new Question("Text", new List<string> { "A", "B" }, "A", "Easy");

            q.DisplayQuestion();
        }
    }
}
