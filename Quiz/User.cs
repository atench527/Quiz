using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApplication



{

    public class User
    {
        private static int nextUserID = 1;

        // Private fields
        private int id;
        private string username;
        private string password;
        private string email;
        private string role;

        // Constructor
        public User(string userName, string userPassword, string userEmail, string userRole)
        {
            id = nextUserID++;
            username = userName;
            password = userPassword;
            email = userEmail;
            role = userRole;
        }

        // Constructor with specific ID
        public User(int userId, string userName, string userPassword, string userEmail, string userRole)
        {
            id = userId;
            username = userName;
            password = userPassword;
            email = userEmail;
            role = userRole;

            if (userId >= nextUserID)
            {
                nextUserID = userId + 1;
            }
        }

        // Getter methods
        public int GetID()
        {
            return id;
        }

        public string GetUsername()
        {
            return username;
        }

        public string GetPassword()
        {
            return password;
        }

        public string GetEmail()
        {
            return email;
        }

        public string GetRole()
        {
            return role;
        }

        // Setter methods
        public void SetUsername(string userName)
        {
            username = userName;
        }

        public void SetPassword(string userPassword)
        {
            password = userPassword;
        }

        public void SetEmail(string userEmail)
        {
            email = userEmail;
        }

        public void SetRole(string userRole)
        {
            role = userRole;
        }

        // Verify credentials method
        public bool VerifyCredentials(string userName, string userPassword)
        {
            if (username == userName && password == userPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Display method
        public void DisplayUserInfo()
        {
            Console.WriteLine("User ID: " + id);
            Console.WriteLine("Username: " + username);
            Console.WriteLine("Email: " + email);
            Console.WriteLine("Role: " + role);
        }
    }
}











