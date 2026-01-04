using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using QuizApplication;

namespace QuizApplication.Tests
{
    [TestClass]
    public class QuizSystemTests
    {
        private string _tempDir;
        private string _originalDir;
        private TextReader _originalIn;
        private TextWriter _originalOut;

        [TestInitialize]
        public void Setup()
        {
            _originalDir = Environment.CurrentDirectory;
            _originalIn = Console.In;
            _originalOut = Console.Out;

            _tempDir = Path.Combine(Path.GetTempPath(), "QuizSystemTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempDir);

            Environment.CurrentDirectory = _tempDir;
        }



        [TestCleanup]
        public void Cleanup()
        {
            Console.SetIn(_originalIn);
            Console.SetOut(_originalOut);
            Environment.CurrentDirectory = _originalDir;

            try
            {
                if (Directory.Exists(_tempDir))
                    Directory.Delete(_tempDir, true);
            }
            catch
            {
            }
        }


        // Tests Start() seeds data and saves CSV files on first run
        [TestMethod]
        public void Start_FirstRun_ShouldCreateCsvFiles()
        {
            var originalIn = Console.In;
            var originalOut = Console.Out;

            try
            {
                Environment.CurrentDirectory = _tempDir;

                var system = new QuizSystem();


                Console.SetIn(new StringReader("4\n"));
                Console.SetOut(new StringWriter());

                system.Start();

                Assert.IsTrue(File.Exists("users.csv"));
                Assert.IsTrue(File.Exists("categories.csv"));
                Assert.IsTrue(File.Exists("questions.csv"));
                Assert.IsTrue(File.Exists("quizzes.csv"));
                Assert.IsTrue(File.Exists("results.csv"));
            }
            finally
            {
                Console.SetIn(originalIn);
                Console.SetOut(originalOut);
            }
        }



        // Tests SaveAll + LoadAll round trip preserves non-empty data
        [TestMethod]
        public void SaveAndLoad_RoundTrip_ShouldLoadData()
        {
            var system = new QuizSystem();
            Environment.CurrentDirectory = _tempDir;


            // Seed -> SaveAll (private methods)
            InvokePrivate(system, "SeedSampleData");
            InvokePrivate(system, "SaveAll");

            // New instance -> LoadAll
            var system2 = new QuizSystem();

            InvokePrivate(system2, "LoadAll");

            var users = GetPrivateList<User>(system2, "users");
            var categories = GetPrivateList<Category>(system2, "categories");
            var questions = GetPrivateList<Question>(system2, "questions");
            var quizzes = GetPrivateList<Quiz>(system2, "quizzes");

            Assert.IsTrue(users.Count > 0);
            Assert.IsTrue(categories.Count > 0);
            Assert.IsTrue(questions.Count > 0);
            Assert.IsTrue(quizzes.Count > 0);
        }

        // Tests ParseIdList handles empty/invalid input (private static)
        [TestMethod]
        public void ParseIdList_EmptyOrInvalid_ShouldReturnExpected()
        {
            var list1 = InvokePrivateStatic<List<int>>(typeof(QuizSystem), "ParseIdList", "");
            Assert.AreEqual(0, list1.Count);

            var list2 = InvokePrivateStatic<List<int>>(typeof(QuizSystem), "ParseIdList", "a,b,3");
            Assert.AreEqual(1, list2.Count);
            Assert.AreEqual(3, list2[0]);
        }

        // Tests LoadUsers ignores missing file (no exception, list stays empty)
        [TestMethod]
        public void LoadUsers_WhenFileMissing_ShouldNotThrowAndRemainEmpty()
        {
            var system = new QuizSystem();

            InvokePrivate(system, "LoadUsers");
            var users = GetPrivateList<User>(system, "users");

            Assert.AreEqual(0, users.Count);
        }

        // Tests SaveResults writes header even when no results exist
        [TestMethod]
        public void SaveResults_WhenEmpty_ShouldCreateFileWithHeader()
        {
            var system = new QuizSystem();

            InvokePrivate(system, "SaveResults");

            string path = "results.csv";
            Assert.IsTrue(File.Exists(path));

            var lines = File.ReadAllLines(path);
            Assert.IsTrue(lines.Length >= 1);
            Assert.AreEqual("ResultsID,UserID,QuizID,Mark,WrongAnswersPipe", lines[0]);
        }

        // ---- Helpers ----

        private static void InvokePrivate(object instance, string methodName, params object[] args)
        {
            var t = instance.GetType();
            var m = t.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (m == null) Assert.Fail("Missing method: " + methodName);
            m.Invoke(instance, args);
        }

        private static T InvokePrivateStatic<T>(Type type, string methodName, params object[] args)
        {
            var m = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
            if (m == null) Assert.Fail("Missing method: " + methodName);
            return (T)m.Invoke(null, args);
        }

        private static List<T> GetPrivateList<T>(object instance, string fieldName)
        {
            var t = instance.GetType();
            var f = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (f == null) Assert.Fail("Missing field: " + fieldName);

            var value = f.GetValue(instance);
            return value as List<T> ?? new List<T>();
        }
    }
}
