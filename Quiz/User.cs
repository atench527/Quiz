using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApplication
{
    public class User




    {
        private int userID;
        private string username;
        private string password;
        private string email;
        private string role;
        private bool isAdmin;




        // some properties will need to be accessed from other classes
        // so we will now add some properties


        public int UserID

        {
            get { return userID; }
            set { userID = value; }
        }


        public string UserName

        {
            get { return username; }
            set { username = value; }
        }


        public string Password

        {
            get { return password;}
            set { password = value;}

        }


        public string Email

        {
            get { return email;}
            set { email = value;}
        }

        public string Role

        {
            get { return role;}
            set { role = value;}
        }



        public bool IsAdmin

        {
            get { return isAdmin;} 
            set { isAdmin = value;}
        }

        // we will now create our own default constructor and not the default c# will give us


        public User()

        {
            userID = 0;
            username = "";
            password = "";
            email = "";
            role = "student";
            isAdmin = false;
        }



        // after creating our defauly constructor we will now create a custom one


        public User(int id, string name, string pwd, string mail, string userRole, bool adminStatus)


        {
            userID = id;
            username = name;
            password = pwd;
            email = mail;
            role = userRole;
            isAdmin = adminStatus;
        }


        // we will now add methods

        public void UpdateProfile(string newEmail)

        {
            email = newEmail;
            Console.WriteLine("Profile updated successfully.");
        }

            
        public void logout ()

        {
            Console.WriteLine(username + "has logged out.");
        }


        public virtual void DeleteUser()

        {
            Console.WriteLine("User" + username + "deleted");
        }











    }






}
