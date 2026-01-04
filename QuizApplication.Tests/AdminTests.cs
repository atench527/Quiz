using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using QuizApplication;

namespace QuizApplication.Tests
{
    [TestClass]
    public class AdminTests
    {
        // Tests Admin constructor without ID
        [TestMethod]
        public void AdminConstructor_WithoutId_ShouldSetRoleAndLoginDate()
        {
            Admin admin = new Admin("adminUser", "password123", "admin@test.com");

            Assert.AreEqual("Admin", admin.GetRole());
            Assert.IsTrue(admin.GetLoginDate() <= DateTime.Now);
        }

        // Tests Admin constructor with ID
        [TestMethod]
        public void AdminConstructor_WithId_ShouldSetIdCorrectly()
        {
            Admin admin = new Admin(1, "adminUser", "password123", "admin@test.com");

            Assert.AreEqual(1, admin.GetID());
            Assert.AreEqual("Admin", admin.GetRole());
        }

        // Tests setting and getting login date
        [TestMethod]
        public void SetLoginDate_ShouldUpdateLoginDate()
        {
            Admin admin = new Admin("adminUser", "password123", "admin@test.com");
            DateTime testDate = new DateTime(2024, 1, 1);

            admin.SetLoginDate(testDate);

            Assert.AreEqual(testDate, admin.GetLoginDate());
        }

        // Tests adding a student user
        [TestMethod]
        public void AddUsers_ShouldAddStudentUser()
        {
            Admin admin = new Admin("admin", "pass", "admin@test.com");
            List<User> users = new List<User>();

            admin.AddUsers(users, "student1", "pass123", "student@test.com", "Student");

            Assert.AreEqual(1, users.Count);
            Assert.AreEqual("Student", users[0].GetRole());
        }

        // Tests adding an admin user
        [TestMethod]
        public void AddUsers_ShouldAddAdminUser()
        {
            Admin admin = new Admin("admin", "pass", "admin@test.com");
            List<User> users = new List<User>();

            admin.AddUsers(users, "admin2", "pass123", "admin2@test.com", "Admin");

            Assert.AreEqual("Admin", users[0].GetRole());
        }

        // Tests null user list handling
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddUsers_NullList_ShouldThrowException()
        {
            Admin admin = new Admin("admin", "pass", "admin@test.com");

            admin.AddUsers(null, "user", "pass", "email@test.com", "Student");
        }

        // Tests editing an existing user
        [TestMethod]
        public void EditUsers_ShouldUpdateUserDetails()
        {
            Admin admin = new Admin("admin", "pass", "admin@test.com");
            List<User> users = new List<User>
            {
                new Student(1, "oldName", "oldPass", "old@test.com")
            };

            admin.EditUsers(users, 1, "newName", "newPass", "new@test.com", "Student");

            Assert.AreEqual("newName", users[0].GetUsername());
            Assert.AreEqual("new@test.com", users[0].GetEmail());
        }

        // Tests editing a non-existent user
        [TestMethod]
        public void EditUsers_InvalidUserId_ShouldNotModifyList()
        {
            Admin admin = new Admin("admin", "pass", "admin@test.com");
            List<User> users = new List<User>();

            admin.EditUsers(users, 99, "name", "pass", "email@test.com", "Student");

            Assert.AreEqual(0, users.Count);
        }

        // Tests deleting an existing user
        [TestMethod]
        public void DeleteUsers_ShouldRemoveUser()
        {
            Admin admin = new Admin("admin", "pass", "admin@test.com");
            List<User> users = new List<User>
            {
                new Student(1, "student", "pass", "test@test.com")
            };

            admin.DeleteUsers(users, 1);

            Assert.AreEqual(0, users.Count);
        }

        // Tests deleting a non-existent user
        [TestMethod]
        public void DeleteUsers_InvalidUserId_ShouldNotChangeList()
        {
            Admin admin = new Admin("admin", "pass", "admin@test.com");
            List<User> users = new List<User>();

            admin.DeleteUsers(users, 5);

            Assert.AreEqual(0, users.Count);
        }
    }
}
