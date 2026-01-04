using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using QuizApplication;

namespace QuizApplication.Tests
{
    [TestClass]
    public class StudentTests
    {
        // Tests constructor sets role and default status
        [TestMethod]
        public void StudentConstructor_WithoutId_ShouldSetRoleAndStatus()
        {
            Student s = new Student("stu", "pass", "stu@test.com");

            Assert.AreEqual("Student", s.GetRole());
            Assert.AreEqual("Active", s.GetStatus());
        }

        // Tests constructor with ID sets ID and status
        [TestMethod]
        public void StudentConstructor_WithId_ShouldSetIdAndStatus()
        {
            Student s = new Student(5, "stu", "pass", "stu@test.com");

            Assert.AreEqual(5, s.GetID());
            Assert.AreEqual("Active", s.GetStatus());
        }

        // Tests SetStatus updates status
        [TestMethod]
        public void SetStatus_ShouldUpdateStatus()
        {
            Student s = new Student("stu", "pass", "stu@test.com");

            s.SetStatus("Inactive");

            Assert.AreEqual("Inactive", s.GetStatus());
        }

        // Tests PlayQuiz returns null when questions list is null
        [TestMethod]
        public void PlayQuiz_NullQuestions_ShouldReturnNull()
        {
            Student s = new Student(1, "stu", "pass", "stu@test.com");

            Results r = s.PlayQuiz(1, null);

            Assert.IsNull(r);
        }

        // Tests PlayQuiz returns null when questions list is empty
        [TestMethod]
        public void PlayQuiz_EmptyQuestions_ShouldReturnNull()
        {
            Student s = new Student(1, "stu", "pass", "stu@test.com");

            Results r = s.PlayQuiz(1, new List<Question>());

            Assert.IsNull(r);
        }

        // Tests PlayQuiz saves results and returns a Results object (all correct answers)
        [TestMethod]
        public void PlayQuiz_ValidQuestions_ShouldSaveResults()
        {
            Student s = new Student(1, "stu", "pass", "stu@test.com");

            var questions = new List<Question>
            {
                new Question(1, "Q1", new List<string> { "A", "B", "C", "D" }, "1", "Easy"),
                new Question(2, "Q2", new List<string> { "A", "B", "C", "D" }, "2", "Easy")
            };

            // Provide correct answers: "1" then "2"
            Console.SetIn(new StringReader("1\n2\n"));
            Console.SetOut(new StringWriter());

            Results attempt = s.PlayQuiz(99, questions);

            Assert.IsNotNull(attempt);
            Assert.AreEqual(s.GetID(), attempt.GetUserId());
            Assert.AreEqual(99, attempt.GetQuizId());
            Assert.AreEqual(2, attempt.GetMark());
            Assert.AreEqual(0, attempt.GetWrongAnswers().Count);

            Results saved = s.GetSavedResults(99);
            Assert.IsNotNull(saved);
            Assert.AreEqual(2, saved.GetMark());
        }

        // Tests RestartQuiz removes saved results
        [TestMethod]
        public void RestartQuiz_ShouldRemoveSavedResults()
        {
            Student s = new Student(1, "stu", "pass", "stu@test.com");

            var questions = new List<Question>
            {
                new Question(1, "Q1", new List<string> { "A", "B", "C", "D" }, "1", "Easy")
            };

            Console.SetIn(new StringReader("1\n"));
            Console.SetOut(new StringWriter());

            s.PlayQuiz(10, questions);
            Assert.IsNotNull(s.GetSavedResults(10));

            s.RestartQuiz(10);

            Assert.IsNull(s.GetSavedResults(10));
        }

        // Tests GetAllSavedResults returns all stored results
        [TestMethod]
        public void GetAllSavedResults_ShouldReturnAllResults()
        {
            Student s = new Student(1, "stu", "pass", "stu@test.com");

            var q1 = new List<Question>
            {
                new Question(1, "Q1", new List<string> { "A", "B", "C", "D" }, "1", "Easy")
            };

            var q2 = new List<Question>
            {
                new Question(2, "Q2", new List<string> { "A", "B", "C", "D" }, "1", "Easy")
            };

            Console.SetOut(new StringWriter());

            Console.SetIn(new StringReader("1\n"));
            s.PlayQuiz(1, q1);

            Console.SetIn(new StringReader("1\n"));
            s.PlayQuiz(2, q2);

            List<Results> all = s.GetAllSavedResults();

            Assert.AreEqual(2, all.Count);
        }
    }
}
