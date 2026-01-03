using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QuizApplication
{
    internal class QuizSystem
    {
        // CSV file names (in project working directory)
        private const string UsersFile = "users.csv";
        private const string CategoriesFile = "categories.csv";
        private const string QuestionsFile = "questions.csv";
        private const string QuizzesFile = "quizzes.csv";
        private const string ResultsFile = "results.csv";

        private readonly List<User> users = new List<User>();
        private readonly List<Category> categories = new List<Category>();
        private readonly List<Question> questions = new List<Question>();
        private readonly List<Quiz> quizzes = new List<Quiz>();
        private readonly List<Results> results = new List<Results>();

        public void Start()
        {
            LoadAll();

            // If first run with no CSVs, seed a tiny dataset
            if (users.Count == 0 && quizzes.Count == 0 && questions.Count == 0 && categories.Count == 0)
            {
                SeedSampleData();
                SaveAll();
            }

            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== QUIZ SYSTEM ===");
                Console.WriteLine("1) Login");
                Console.WriteLine("2) Browse categories");
                Console.WriteLine("3) List all quizzes");
                Console.WriteLine("4) Save & Exit");
                Console.Write("Choose: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1": LoginFlow(); break;
                    case "2": BrowseCategories(null); break;
                    case "3": ListAllQuizzes(); break;
                    case "4":
                        SaveAll();
                        Console.WriteLine("Saved. Bye.");
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private void LoginFlow()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Password: ");
            string password = Console.ReadLine() ?? "";

            User u = users.FirstOrDefault(x => x.VerifyCredentials(username, password));
            if (u == null)
            {
                Console.WriteLine("Login failed.");
                return;
            }

            Console.WriteLine($"Logged in as {u.GetUsername()} ({u.GetRole()})");

            if (u is Admin admin) AdminMenu(admin);
            else if (u is Student student) StudentMenu(student);
            else Console.WriteLine("Unknown user class type.");
        }

        // STUDENT FLOW
        private void StudentMenu(Student student)
        {
            while (true)
            {
                Console.WriteLine("\n=== STUDENT MENU ===");
                Console.WriteLine("1) Browse categories");
                Console.WriteLine("2) List all quizzes");
                Console.WriteLine("3) Play quiz by ID");
                Console.WriteLine("4) View my results");
                Console.WriteLine("5) Logout");
                Console.Write("Choose: ");

                string choice = Console.ReadLine() ?? "";
                switch (choice)
                {
                    case "1":
                        BrowseCategories(student);
                        break;
                    case "2":
                        ListAllQuizzes();
                        break;
                    case "3":
                        PlayQuizById(student);
                        break;
                    case "4":
                        ViewStudentResults(student);
                        break;
                    case "5":
                        Console.WriteLine("Logged out.");
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private void BrowseCategories(Student studentOrNull)
        {
            if (categories.Count == 0)
            {
                Console.WriteLine("No categories available.");
                return;
            }

            Console.WriteLine("\n=== CATEGORIES ===");
            foreach (var c in categories)
                Console.WriteLine($"[{c.GetCategoryID()}] {c.GetCategoryName()} - {c.GetCategoryDescription()}");

            Console.Write("Pick category ID (or blank to cancel): ");
            string input = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(input)) return;

            if (!int.TryParse(input, out int catId))
            {
                Console.WriteLine("Invalid category ID.");
                return;
            }

            var cat = categories.FirstOrDefault(x => x.GetCategoryID() == catId);
            if (cat == null)
            {
                Console.WriteLine("Category not found.");
                return;
            }

            var catQuizzes = quizzes.Where(q => q.GetCategoryId() == catId).ToList();
            if (catQuizzes.Count == 0)
            {
                Console.WriteLine("No quizzes in that category.");
                return;
            }

            Console.WriteLine($"\n=== QUIZZES IN {cat.GetCategoryName()} ===");
            foreach (var q in catQuizzes)
                Console.WriteLine($"[{q.GetQuizId()}] {q.GetQuizName()} (Questions: {q.GetQuestionIds().Count})");

            if (studentOrNull == null) return;

            Console.Write("Enter quiz ID to play (or blank to cancel): ");
            string qid = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(qid)) return;

            if (!int.TryParse(qid, out int quizId))
            {
                Console.WriteLine("Invalid quiz ID.");
                return;
            }

            PlayQuiz(studentOrNull, quizId);
        }

        private void PlayQuizById(Student student)
        {
            ListAllQuizzes();
            Console.Write("Enter quiz ID to play: ");
            if (!int.TryParse(Console.ReadLine(), out int quizId))
            {
                Console.WriteLine("Invalid quiz ID.");
                return;
            }
            PlayQuiz(student, quizId);
        }

        private void PlayQuiz(Student student, int quizId)
        {
            Quiz quiz = quizzes.FirstOrDefault(q => q.GetQuizId() == quizId);
            if (quiz == null)
            {
                Console.WriteLine("Quiz not found.");
                return;
            }

            List<Question> quizQuestions = quiz.GetQuestionIds()
                .Select(id => questions.FirstOrDefault(q => q.GetQuestionID() == id))
                .Where(q => q != null)
                .ToList();

            if (quizQuestions.Count == 0)
            {
                Console.WriteLine("Quiz has no valid questions.");
                return;
            }

            Results attempt = student.PlayQuiz(quizId, quizQuestions);
            if (attempt == null) return;

            attempt.ShowResults();

            // Persist results (store all attempts; simple approach)
            results.Add(attempt);
            SaveResults();
        }

        private void ViewStudentResults(Student student)
        {
            var my = results.Where(r => r.GetUserId() == student.GetID()).ToList();
            if (my.Count == 0)
            {
                Console.WriteLine("No results yet.");
                return;
            }

            Console.WriteLine("\n=== MY RESULTS ===");
            foreach (var r in my.OrderByDescending(x => x.GetResultsId()))
            {
                var quiz = quizzes.FirstOrDefault(q => q.GetQuizId() == r.GetQuizId());
                string quizName = quiz != null ? quiz.GetQuizName() : $"Quiz {r.GetQuizId()}";
                Console.WriteLine($"ResultID {r.GetResultsId()} | {quizName} | Mark {r.GetMark()} | Wrong {r.GetWrongAnswers().Count}");
            }

            Console.Write("Enter ResultID to view details (or blank to cancel): ");
            string input = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(input)) return;

            if (!int.TryParse(input, out int rid))
            {
                Console.WriteLine("Invalid ResultID.");
                return;
            }

            var res = my.FirstOrDefault(x => x.GetResultsId() == rid);
            if (res == null)
            {
                Console.WriteLine("Result not found.");
                return;
            }

            res.ShowResults();
        }

        // ADMIN FLOW
        private void AdminMenu(Admin admin)
        {
            while (true)
            {
                Console.WriteLine("\n=== ADMIN MENU ===");
                Console.WriteLine("1) List users");
                Console.WriteLine("2) Add user");
                Console.WriteLine("3) Edit user");
                Console.WriteLine("4) Delete user");
                Console.WriteLine("5) Manage quizzes");
                Console.WriteLine("6) Manage questions");
                Console.WriteLine("7) Manage categories");
                Console.WriteLine("8) Logout");
                Console.Write("Choose: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1": ListUsers(); break;
                    case "2": AdminAddUser(admin); break;
                    case "3": AdminEditUser(admin); break;
                    case "4": AdminDeleteUser(admin); break;
                    case "5": ManageQuizzes(); break;
                    case "6": ManageQuestions(); break;
                    case "7": ManageCategories(); break;
                    case "8":
                        SaveAll();
                        Console.WriteLine("Logged out.");
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private void AdminAddUser(Admin admin)
        {
            Console.Write("Username: "); string u = Console.ReadLine() ?? "";
            Console.Write("Password: "); string p = Console.ReadLine() ?? "";
            Console.Write("Email: "); string e = Console.ReadLine() ?? "";
            Console.Write("Role (Admin/Student): "); string r = Console.ReadLine() ?? "Student";

            admin.AddUsers(users, u, p, e, r);
            SaveUsers();
        }

        private void AdminEditUser(Admin admin)
        {
            ListUsers();
            Console.Write("User ID to edit: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Console.Write("New username: "); string u = Console.ReadLine() ?? "";
            Console.Write("New password: "); string p = Console.ReadLine() ?? "";
            Console.Write("New email: "); string e = Console.ReadLine() ?? "";
            Console.Write("New role (Admin/Student): "); string r = Console.ReadLine() ?? "Student";

            admin.EditUsers(users, id, u, p, e, r);
            SaveUsers();
        }

        private void AdminDeleteUser(Admin admin)
        {
            ListUsers();
            Console.Write("User ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            admin.DeleteUsers(users, id);
            SaveUsers();
        }

        private void ManageCategories()
        {
            while (true)
            {
                Console.WriteLine("\n=== CATEGORY MANAGER ===");
                Console.WriteLine("1) List categories");
                Console.WriteLine("2) Add category");
                Console.WriteLine("3) Edit category");
                Console.WriteLine("4) Delete category");
                Console.WriteLine("5) Back");
                Console.Write("Choose: ");

                string choice = Console.ReadLine() ?? "";
                switch (choice)
                {
                    case "1":
                        foreach (var c in categories)
                            Console.WriteLine($"[{c.GetCategoryID()}] {c.GetCategoryName()} - {c.GetCategoryDescription()}");
                        break;

                    case "2":
                        Console.Write("Name: "); string n = Console.ReadLine() ?? "";
                        Console.Write("Description: "); string d = Console.ReadLine() ?? "";
                        categories.Add(new Category(n, d));
                        SaveCategories();
                        Console.WriteLine("Category added.");
                        break;

                    case "3":
                        Console.Write("Category ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int cid))
                        {
                            Console.WriteLine("Invalid ID.");
                            break;
                        }
                        var cat = categories.FirstOrDefault(x => x.GetCategoryID() == cid);
                        if (cat == null) { Console.WriteLine("Not found."); break; }
                        Console.Write("New name: "); cat.SetCategoryName(Console.ReadLine() ?? "");
                        Console.Write("New description: "); cat.SetCategoryDescription(Console.ReadLine() ?? "");
                        SaveCategories();
                        Console.WriteLine("Category updated.");
                        break;

                    case "4":
                        Console.Write("Category ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int del))
                        {
                            Console.WriteLine("Invalid ID.");
                            break;
                        }
                        // prevent delete if quizzes exist
                        if (quizzes.Any(q => q.GetCategoryId() == del))
                        {
                            Console.WriteLine("Cannot delete: quizzes still use this category.");
                            break;
                        }
                        int removed = categories.RemoveAll(x => x.GetCategoryID() == del);
                        SaveCategories();
                        Console.WriteLine(removed > 0 ? "Deleted." : "Not found.");
                        break;

                    case "5":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private void ManageQuestions()
        {
            while (true)
            {
                Console.WriteLine("\n=== QUESTION MANAGER ===");
                Console.WriteLine("1) List questions");
                Console.WriteLine("2) Add question");
                Console.WriteLine("3) Edit question");
                Console.WriteLine("4) Delete question");
                Console.WriteLine("5) Back");
                Console.Write("Choose: ");

                string choice = Console.ReadLine() ?? "";
                switch (choice)
                {
                    case "1":
                        foreach (var q in questions)
                            Console.WriteLine($"[{q.GetQuestionID()}] {q.GetQuestionText()} (Diff: {q.GetQuestionDifficultyLevel()})");
                        break;

                    case "2":
                        AddQuestionInteractive();
                        SaveQuestions();
                        break;

                    case "3":
                        EditQuestionInteractive();
                        SaveQuestions();
                        break;

                    case "4":
                        DeleteQuestionInteractive();
                        SaveQuestions();
                        break;

                    case "5":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private void AddQuestionInteractive()
        {
            Console.Write("Question text: ");
            string text = Console.ReadLine() ?? "";

            var opts = new List<string>();
            for (int i = 1; i <= 4; i++)
            {
                Console.Write($"Option {i}: ");
                opts.Add(Console.ReadLine() ?? "");
            }

            Console.Write("Correct answer (enter option number 1-4 as text): ");
            string correct = Console.ReadLine() ?? "1";

            Console.Write("Difficulty (Easy/Medium/Hard): ");
            string diff = Console.ReadLine() ?? "Easy";

            questions.Add(new Question(text, opts, correct, diff));
            Console.WriteLine("Question added.");
        }

        private void EditQuestionInteractive()
        {
            Console.Write("Question ID to edit: ");
            if (!int.TryParse(Console.ReadLine(), out int qid))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            var q = questions.FirstOrDefault(x => x.GetQuestionID() == qid);
            if (q == null) { Console.WriteLine("Not found."); return; }

            Console.Write("New text: ");
            q.SetQuestionText(Console.ReadLine() ?? "");

            var opts = new List<string>();
            for (int i = 1; i <= 4; i++)
            {
                Console.Write($"New option {i}: ");
                opts.Add(Console.ReadLine() ?? "");
            }
            q.SetQuestionOptions(opts);

            Console.Write("New correct answer (option number 1-4 as text): ");
            q.SetQuestionCorrectAnswer(Console.ReadLine() ?? "1");

            Console.Write("New difficulty: ");
            q.SetQuestionDifficultyLevel(Console.ReadLine() ?? "Easy");

            Console.WriteLine("Question updated.");
        }

        private void DeleteQuestionInteractive()
        {
            Console.Write("Question ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int qid))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            // prevent delete if used by any quiz
            if (quizzes.Any(z => z.GetQuestionIds().Contains(qid)))
            {
                Console.WriteLine("Cannot delete: a quiz references this question.");
                return;
            }

            int removed = questions.RemoveAll(x => x.GetQuestionID() == qid);
            Console.WriteLine(removed > 0 ? "Deleted." : "Not found.");
        }

        private void ManageQuizzes()
        {
            while (true)
            {
                Console.WriteLine("\n=== QUIZ MANAGER ===");
                Console.WriteLine("1) List quizzes");
                Console.WriteLine("2) Create quiz");
                Console.WriteLine("3) Edit quiz");
                Console.WriteLine("4) Delete quiz");
                Console.WriteLine("5) Back");
                Console.Write("Choose: ");

                string choice = Console.ReadLine() ?? "";
                switch (choice)
                {
                    case "1": ListAllQuizzes(); break;
                    case "2": CreateQuizInteractive(); SaveQuizzes(); break;
                    case "3": EditQuizInteractive(); SaveQuizzes(); break;
                    case "4": DeleteQuizInteractive(); SaveQuizzes(); break;
                    case "5": return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }

        private void CreateQuizInteractive()
        {
            if (categories.Count == 0)
            {
                Console.WriteLine("Create a category first.");
                return;
            }
            if (questions.Count == 0)
            {
                Console.WriteLine("Create questions first.");
                return;
            }

            Console.Write("Quiz name: ");
            string name = Console.ReadLine() ?? "";

            Console.WriteLine("Pick a category ID:");
            foreach (var c in categories)
                Console.WriteLine($"[{c.GetCategoryID()}] {c.GetCategoryName()}");

            if (!int.TryParse(Console.ReadLine(), out int catId) || categories.All(c => c.GetCategoryID() != catId))
            {
                Console.WriteLine("Invalid category.");
                return;
            }

            Console.WriteLine("Pick question IDs to include (comma-separated), e.g. 1,2,5");
            foreach (var q in questions)
                Console.WriteLine($"[{q.GetQuestionID()}] {q.GetQuestionText()}");

            string raw = Console.ReadLine() ?? "";
            var ids = ParseIdList(raw);
            ids = ids.Where(id => questions.Any(q => q.GetQuestionID() == id)).Distinct().ToList();

            if (ids.Count == 0)
            {
                Console.WriteLine("No valid questions selected.");
                return;
            }

            quizzes.Add(new Quiz(name, catId, ids));
            Console.WriteLine("Quiz created.");
        }

        private void EditQuizInteractive()
        {
            ListAllQuizzes();
            Console.Write("Quiz ID to edit: ");
            if (!int.TryParse(Console.ReadLine(), out int qid))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            var quiz = quizzes.FirstOrDefault(x => x.GetQuizId() == qid);
            if (quiz == null) { Console.WriteLine("Not found."); return; }

            Console.Write("New quiz name: ");
            quiz.SetQuizName(Console.ReadLine() ?? quiz.GetQuizName());

            Console.WriteLine("New category ID (blank to keep):");
            foreach (var c in categories)
                Console.WriteLine($"[{c.GetCategoryID()}] {c.GetCategoryName()}");

            string catInput = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(catInput) && int.TryParse(catInput, out int newCatId))
            {
                if (categories.Any(c => c.GetCategoryID() == newCatId))
                    quiz.SetCategoryId(newCatId);
            }

            Console.WriteLine("New question IDs (comma-separated) OR blank to keep:");
            string qInput = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(qInput))
            {
                var ids = ParseIdList(qInput);
                ids = ids.Where(id => questions.Any(q => q.GetQuestionID() == id)).Distinct().ToList();
                if (ids.Count > 0) quiz.SetQuestionIds(ids);
            }

            Console.WriteLine("Quiz updated.");
        }

        private void DeleteQuizInteractive()
        {
            ListAllQuizzes();
            Console.Write("Quiz ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int qid))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            int removed = quizzes.RemoveAll(x => x.GetQuizId() == qid);
            Console.WriteLine(removed > 0 ? "Deleted." : "Not found.");
        }

        // LISTING STUFF
        private void ListAllQuizzes()
        {
            Console.WriteLine("\n=== QUIZZES ===");
            if (quizzes.Count == 0)
            {
                Console.WriteLine("No quizzes available.");
                return;
            }

            foreach (var q in quizzes)
            {
                var cat = categories.FirstOrDefault(c => c.GetCategoryID() == q.GetCategoryId());
                string catName = cat != null ? cat.GetCategoryName() : "Unknown";
                Console.WriteLine($"[{q.GetQuizId()}] {q.GetQuizName()} (Category: {catName}, Questions: {q.GetQuestionIds().Count})");
            }
        }

        private void ListUsers()
        {
            Console.WriteLine("\n=== USERS ===");
            foreach (var u in users)
                Console.WriteLine($"ID: {u.GetID()} | {u.GetUsername()} | {u.GetEmail()} | Role: {u.GetRole()}");
        }

        private static List<int> ParseIdList(string raw)
        {
            var list = new List<int>();
            if (string.IsNullOrWhiteSpace(raw)) return list;

            foreach (var part in raw.Split(','))
            {
                if (int.TryParse(part.Trim(), out int id))
                    list.Add(id);
            }
            return list;
        }

        // CSV LOAD/SAVE
        private void LoadAll()
        {
            LoadUsers();
            LoadCategories();
            LoadQuestions();
            LoadQuizzes();
            LoadResults();
        }

        private void SaveAll()
        {
            SaveUsers();
            SaveCategories();
            SaveQuestions();
            SaveQuizzes();
            SaveResults();
        }

        private void LoadUsers()
        {
            users.Clear();
            if (!File.Exists(UsersFile)) return;

            var lines = File.ReadAllLines(UsersFile);
            foreach (var line in lines.Skip(1)) // header
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var cols = CsvUtil.SplitLine(line);
                if (cols.Count < 5) continue;

                int id = int.Parse(cols[0]);
                string username = cols[1];
                string password = cols[2];
                string email = cols[3];
                string role = cols[4];

                if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    users.Add(new Admin(id, username, password, email));
                else
                    users.Add(new Student(id, username, password, email));
            }
        }

        private void SaveUsers()
        {
            var lines = new List<string>
            {
                "UserID,Username,Password,Email,Role"
            };

            foreach (var u in users)
            {
                lines.Add(
                    $"{u.GetID()}," +
                    $"{CsvUtil.Escape(u.GetUsername())}," +
                    $"{CsvUtil.Escape(u.GetPassword())}," +
                    $"{CsvUtil.Escape(u.GetEmail())}," +
                    $"{CsvUtil.Escape(u.GetRole())}"
                );
            }

            File.WriteAllLines(UsersFile, lines);
        }

        private void LoadCategories()
        {
            categories.Clear();
            if (!File.Exists(CategoriesFile)) return;

            var lines = File.ReadAllLines(CategoriesFile);
            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var cols = CsvUtil.SplitLine(line);
                if (cols.Count < 3) continue;

                int id = int.Parse(cols[0]);
                string name = cols[1];
                string desc = cols[2];

                categories.Add(new Category(id, name, desc));
            }
        }

        private void SaveCategories()
        {
            var lines = new List<string> { "CategoryID,Name,Description" };
            foreach (var c in categories)
            {
                lines.Add(
                    $"{c.GetCategoryID()}," +
                    $"{CsvUtil.Escape(c.GetCategoryName())}," +
                    $"{CsvUtil.Escape(c.GetCategoryDescription())}"
                );
            }
            File.WriteAllLines(CategoriesFile, lines);
        }

        private void LoadQuestions()
        {
            questions.Clear();
            if (!File.Exists(QuestionsFile)) return;

            var lines = File.ReadAllLines(QuestionsFile);
            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var cols = CsvUtil.SplitLine(line);
                if (cols.Count < 5) continue;

                int id = int.Parse(cols[0]);
                string text = cols[1];
                var opts = CsvUtil.SplitPipe(cols[2]);
                string correct = cols[3];
                string diff = cols[4];

                questions.Add(new Question(id, text, opts, correct, diff));
            }
        }

        private void SaveQuestions()
        {
            var lines = new List<string> { "QuestionID,Text,OptionsPipe,CorrectAnswer,Difficulty" };
            foreach (var q in questions)
            {
                lines.Add(
                    $"{q.GetQuestionID()}," +
                    $"{CsvUtil.Escape(q.GetQuestionText())}," +
                    $"{CsvUtil.Escape(CsvUtil.JoinPipe(q.GetQuestionOptions()))}," +
                    $"{CsvUtil.Escape(q.GetQuestionCorrectAnswer())}," +
                    $"{CsvUtil.Escape(q.GetQuestionDifficultyLevel())}"
                );
            }
            File.WriteAllLines(QuestionsFile, lines);
        }

        private void LoadQuizzes()
        {
            quizzes.Clear();
            if (!File.Exists(QuizzesFile)) return;

            var lines = File.ReadAllLines(QuizzesFile);
            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var cols = CsvUtil.SplitLine(line);
                if (cols.Count < 4) continue;

                int id = int.Parse(cols[0]);
                string name = cols[1];
                int categoryId = int.Parse(cols[2]);
                var qids = CsvUtil.SplitPipe(cols[3]).Select(s => int.TryParse(s, out int v) ? v : -1).Where(v => v > 0).ToList();

                quizzes.Add(new Quiz(id, name, categoryId, qids));
            }
        }

        private void SaveQuizzes()
        {
            // IMPORTANT: all quizzes in ONE file -> quizzes.csv
            var lines = new List<string> { "QuizID,QuizName,CategoryID,QuestionIDsPipe" };
            foreach (var q in quizzes)
            {
                var ids = q.GetQuestionIds().Select(x => x.ToString()).ToList();
                lines.Add(
                    $"{q.GetQuizId()}," +
                    $"{CsvUtil.Escape(q.GetQuizName())}," +
                    $"{q.GetCategoryId()}," +
                    $"{CsvUtil.Escape(CsvUtil.JoinPipe(ids))}"
                );
            }
            File.WriteAllLines(QuizzesFile, lines);
        }

        private void LoadResults()
        {
            results.Clear();
            if (!File.Exists(ResultsFile)) return;

            var lines = File.ReadAllLines(ResultsFile);
            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var cols = CsvUtil.SplitLine(line);
                if (cols.Count < 5) continue;

                int rid = int.Parse(cols[0]);
                int uid = int.Parse(cols[1]);
                int qid = int.Parse(cols[2]);
                int mark = int.Parse(cols[3]);
                var wrong = CsvUtil.SplitPipe(cols[4]);

                results.Add(new Results(rid, uid, qid, mark, wrong));
            }
        }

        private void SaveResults()
        {
            var lines = new List<string> { "ResultsID,UserID,QuizID,Mark,WrongAnswersPipe" };
            foreach (var r in results)
            {
                lines.Add(
                    $"{r.GetResultsId()}," +
                    $"{r.GetUserId()}," +
                    $"{r.GetQuizId()}," +
                    $"{r.GetMark()}," +
                    $"{CsvUtil.Escape(CsvUtil.JoinPipe(r.GetWrongAnswers()))}"
                );
            }
            File.WriteAllLines(ResultsFile, lines);
        }

        // SEED (first run)
        private void SeedSampleData()
        {
            users.Add(new Admin("admin", "admin123", "admin@quiz.local"));
            users.Add(new Student("student", "student123", "student@quiz.local"));

            var catGeneral = new Category("General Knowledge", "Mixed questions");
            var catMaths = new Category("Maths", "Basic maths questions");
            categories.Add(catGeneral);
            categories.Add(catMaths);

            var q1 = new Question(
                "What is the capital of France?",
                new List<string> { "London", "Paris", "Berlin", "Rome" },
                "2",
                "Easy"
            );

            var q2 = new Question(
                "Which planet is known as the Red Planet?",
                new List<string> { "Mars", "Venus", "Jupiter", "Mercury" },
                "1",
                "Easy"
            );

            var q3 = new Question(
                "What is 7 + 5?",
                new List<string> { "10", "11", "12", "13" },
                "3",
                "Easy"
            );

            questions.Add(q1);
            questions.Add(q2);
            questions.Add(q3);

            quizzes.Add(new Quiz("General Quiz 1", catGeneral.GetCategoryID(), new List<int> { q1.GetQuestionID(), q2.GetQuestionID() }));
            quizzes.Add(new Quiz("Maths Quiz 1", catMaths.GetCategoryID(), new List<int> { q3.GetQuestionID() }));
        }
    }
}
