using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuizApplication;

namespace QuizApplication.Tests
{
    [TestClass]
    public class UserTests
    {
        // Tests constructor assigns fields and role
        [TestMethod]
        public void UserConstructor_ShouldSetValues()
        {
            User u = new User("ryan", "pass123", "r@test.com", "Student");

            Assert.AreEqual("ryan", u.GetUsername());
            Assert.AreEqual("pass123", u.GetPassword());
            Assert.AreEqual("r@test.com", u.GetEmail());
            Assert.AreEqual("Student", u.GetRole());
            Assert.IsTrue(u.GetID() > 0);
        }

        // Tests constructor with specific ID
        [TestMethod]
        public void UserConstructor_WithId_ShouldSetIdCorrectly()
        {
            User u = new User(10, "ryan", "pass123", "r@test.com", "Admin");

            Assert.AreEqual(10, u.GetID());
            Assert.AreEqual("Admin", u.GetRole());
        }

        // Tests auto-increment of IDs
        [TestMethod]
        public void UserConstructor_ShouldIncrementId()
        {
            User u1 = new User("a", "p", "a@test.com", "Student");
            User u2 = new User("b", "p", "b@test.com", "Student");

            Assert.AreEqual(u1.GetID() + 1, u2.GetID());
        }

        // Tests SetUsername updates username
        [TestMethod]
        public void SetUsername_ShouldUpdateUsername()
        {
            User u = new User("old", "p", "e@test.com", "Student");

            u.SetUsername("new");

            Assert.AreEqual("new", u.GetUsername());
        }

        // Tests SetPassword updates password
        [TestMethod]
        public void SetPassword_ShouldUpdatePassword()
        {
            User u = new User("u", "old", "e@test.com", "Student");

            u.SetPassword("new");

            Assert.AreEqual("new", u.GetPassword());
        }

        // Tests SetEmail updates email
        [TestMethod]
        public void SetEmail_ShouldUpdateEmail()
        {
            User u = new User("u", "p", "old@test.com", "Student");

            u.SetEmail("new@test.com");

            Assert.AreEqual("new@test.com", u.GetEmail());
        }

        // Tests SetRole updates role
        [TestMethod]
        public void SetRole_ShouldUpdateRole()
        {
            User u = new User("u", "p", "e@test.com", "Student");

            u.SetRole("Admin");

            Assert.AreEqual("Admin", u.GetRole());
        }

        // Tests VerifyCredentials returns true for matching values
        [TestMethod]
        public void VerifyCredentials_Correct_ShouldReturnTrue()
        {
            User u = new User("user", "pass", "e@test.com", "Student");

            bool result = u.VerifyCredentials("user", "pass");

            Assert.IsTrue(result);
        }

        // Tests VerifyCredentials returns false for wrong username
        [TestMethod]
        public void VerifyCredentials_WrongUsername_ShouldReturnFalse()
        {
            User u = new User("user", "pass", "e@test.com", "Student");

            bool result = u.VerifyCredentials("wrong", "pass");

            Assert.IsFalse(result);
        }

        // Tests VerifyCredentials returns false for wrong password
        [TestMethod]
        public void VerifyCredentials_WrongPassword_ShouldReturnFalse()
        {
            User u = new User("user", "pass", "e@test.com", "Student");

            bool result = u.VerifyCredentials("user", "wrong");

            Assert.IsFalse(result);
        }

        // Tests DisplayUserInfo does not throw
        [TestMethod]
        public void DisplayUserInfo_ShouldNotThrowException()
        {
            User u = new User("user", "pass", "e@test.com", "Student");

            u.DisplayUserInfo();
        }
    }
}
