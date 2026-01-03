using System;
using System.Collections.Generic;

namespace QuizApplication
{
    internal class Admin : User
    {
        private DateTime loginDate;

        public Admin(string userName, string userPassword, string userEmail)
            : base(userName, userPassword, userEmail, "Admin")
        {
            loginDate = DateTime.Now;
        }

        public Admin(int userId, string userName, string userPassword, string userEmail)
            : base(userId, userName, userPassword, userEmail, "Admin")
        {
            loginDate = DateTime.Now;
        }

        public DateTime GetLoginDate() => loginDate;
        public void SetLoginDate(DateTime dt) => loginDate = dt;

        // Admin methods work on a passed-in list (since QuizSystem storage is not built just yet, will change if necessary)

        public void AddUsers(List<User> users, string username, string password, string email, string role)
        {
            if (users == null) throw new ArgumentNullException(nameof(users));

            User newUser;
            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
                newUser = new Admin(username, password, email);
            else
                newUser = new Student(username, password, email);

            users.Add(newUser);
            Console.WriteLine($"User added: {newUser.GetUsername()} (Role: {newUser.GetRole()}, ID: {newUser.GetID()})");
        }

        public void EditUsers(List<User> users, int userId, string username, string password, string email, string role)
        {
            if (users == null) throw new ArgumentNullException(nameof(users));

            User u = users.Find(x => x.GetID() == userId);
            if (u == null)
            {
                Console.WriteLine($"No user found with ID {userId}");
                return;
            }

            u.SetUsername(username);
            u.SetPassword(password);
            u.SetEmail(email);
            u.SetRole(role);

            Console.WriteLine($"User updated: ID {userId} now {u.GetUsername()} ({u.GetRole()})");
        }

        public void DeleteUsers(List<User> users, int userId)
        {
            if (users == null) throw new ArgumentNullException(nameof(users));

            int removed = users.RemoveAll(x => x.GetID() == userId);
            Console.WriteLine(removed > 0
                ? $"User with ID {userId} deleted."
                : $"No user found with ID {userId}.");
        }
    }
}
