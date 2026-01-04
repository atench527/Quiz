using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using QuizApplication;

namespace QuizApplication.Tests
{
    [TestClass]
    public class ResultsTests
    {
        // Tests constructor assigns IDs and defaults
        [TestMethod]
        public void ResultsConstructor_ShouldSetDefaults()
        {
            Results r = new Results(1, 2);

            Assert.AreEqual(1, r.GetUserId());
            Assert.AreEqual(2, r.GetQuizId());
            Assert.AreEqual(0, r.GetMark());
            Assert.AreEqual(0, r.GetWrongAnswers().Count);
            Assert.IsTrue(r.GetResultsId() > 0);
        }

        // Tests constructor with specific ID
        [TestMethod]
        public void ResultsConstructor_WithId_ShouldSetIdCorrectly()
        {
            Results r = new Results(10, 1, 2, 5, new List<string> { "Q1" });

            Assert.AreEqual(10, r.GetResultsId());
            Assert.AreEqual(1, r.GetUserId());
            Assert.AreEqual(2, r.GetQuizId());
            Assert.AreEqual(5, r.GetMark());
            Assert.AreEqual(1, r.GetWrongAnswers().Count);
        }

        // Tests wrong answers list null becomes empty list
        [TestMethod]
        public void ResultsConstructor_NullWrongAnswers_ShouldSetEmptyList()
        {
            Results r = new Results(10, 1, 2, 5, null);

            Assert.AreEqual(0, r.GetWrongAnswers().Count);
        }

        // Tests adding wrong answer adds text
        [TestMethod]
        public void AddWrongAnswer_ShouldAddToList()
        {
            Results r = new Results(1, 2);

            r.AddWrongAnswer("Wrong: Question 1");

            Assert.AreEqual(1, r.GetWrongAnswers().Count);
            Assert.AreEqual("Wrong: Question 1", r.GetWrongAnswers()[0]);
        }

        // Tests adding null wrong answer adds empty string
        [TestMethod]
        public void AddWrongAnswer_Null_ShouldAddEmptyString()
        {
            Results r = new Results(1, 2);

            r.AddWrongAnswer(null);

            Assert.AreEqual(1, r.GetWrongAnswers().Count);
            Assert.AreEqual("", r.GetWrongAnswers()[0]);
        }

        // Tests CalculateMark basic case
        [TestMethod]
        public void CalculateMark_ShouldReturnCorrectMark()
        {
            Results r = new Results(1, 2);
            r.AddWrongAnswer("Wrong 1");
            r.AddWrongAnswer("Wrong 2");

            int mark = r.CalculateMark(5);

            Assert.AreEqual(3, mark);
            Assert.AreEqual(3, r.GetMark());
        }

        // Tests CalculateMark does not go below zero
        [TestMethod]
        public void CalculateMark_WhenNegative_ShouldReturnZero()
        {
            Results r = new Results(1, 2);
            r.AddWrongAnswer("W1");
            r.AddWrongAnswer("W2");
            r.AddWrongAnswer("W3");

            int mark = r.CalculateMark(2);

            Assert.AreEqual(0, mark);
            Assert.AreEqual(0, r.GetMark());
        }

        // Tests ShowWrongAnswers does not throw when empty
        [TestMethod]
        public void ShowWrongAnswers_WhenEmpty_ShouldNotThrowException()
        {
            Results r = new Results(1, 2);

            r.ShowWrongAnswers();
        }

        // Tests ShowResults does not throw
        [TestMethod]
        public void ShowResults_ShouldNotThrowException()
        {
            Results r = new Results(1, 2);
            r.CalculateMark(3);

            r.ShowResults();
        }
    }
}
