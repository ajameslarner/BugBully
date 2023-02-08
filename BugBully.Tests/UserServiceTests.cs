using BugBully.Controllers;
using BugBully.Data;
using BugBully.Models;
using BugBully.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Management;

namespace BugBully.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private List<Users> GetMockedList(int size)
        {
            var result = new List<Users>();
            for (int i = 1; i < size + 1; i++)
            {
                result.Add(new Users()
                {
                    Id = i,
                    Name = $"User{i}",
                    Username = $"Username{i}"
                });
            }
            return result;
        }

        [TestMethod]
        public void Authenticate_WhenCalledWithValidUsernameAndPassword_ReturnsTrue()
        {
            // Arrange
            var user1 = new Users { Username = "testuser", Password = "testpassword" };
            var users = new List<Users> { user1 };

            var userDbSet = new Mock<DbSet<Users>>();
            userDbSet.As<IQueryable<Users>>().Setup(b => b.Provider).Returns(users.AsQueryable().Provider);
            userDbSet.As<IQueryable<Users>>().Setup(b => b.Expression).Returns(users.AsQueryable().Expression);
            userDbSet.As<IQueryable<Users>>().Setup(b => b.ElementType).Returns(users.AsQueryable().ElementType);
            userDbSet.As<IQueryable<Users>>().Setup(b => b.GetEnumerator()).Returns(users.AsQueryable().GetEnumerator());

            var repository = new Mock<IRepository>();
            repository.Setup(r => r.Users).Returns(userDbSet.Object);
            var userService = new UserService(repository.Object);

            // Act
            var result = userService.Authenticate(user1.Username, user1.Password);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void Authenticate_WhenCalledWithInvalidUsername_ReturnsFalse()
        {
            // Arrange
            var user1 = new Users { Username = "testuser", Password = "testpassword" };
            var users = new List<Users> { user1 };

            var userDbSet = new Mock<DbSet<Users>>();
            userDbSet.As<IQueryable<Users>>().Setup(b => b.Provider).Returns(users.AsQueryable().Provider);
            userDbSet.As<IQueryable<Users>>().Setup(b => b.Expression).Returns(users.AsQueryable().Expression);
            userDbSet.As<IQueryable<Users>>().Setup(b => b.ElementType).Returns(users.AsQueryable().ElementType);
            userDbSet.As<IQueryable<Users>>().Setup(b => b.GetEnumerator()).Returns(users.AsQueryable().GetEnumerator());

            var repository = new Mock<IRepository>();
            repository.Setup(r => r.Users).Returns(userDbSet.Object);
            var userService = new UserService(repository.Object);

            // Act
            var result = userService.Authenticate("invaliduser", "testpassword");

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Authenticate_WhenCalledWithInvalidPassword_ReturnsFalse()
        {
            // Arrange
            var user1 = new Users { Username = "testuser", Password = "testpassword" };
            var users = new List<Users> { user1 };

            var userDbSet = new Mock<DbSet<Users>>();
            userDbSet.As<IQueryable<Users>>().Setup(b => b.Provider).Returns(users.AsQueryable().Provider);
            userDbSet.As<IQueryable<Users>>().Setup(b => b.Expression).Returns(users.AsQueryable().Expression);
            userDbSet.As<IQueryable<Users>>().Setup(b => b.ElementType).Returns(users.AsQueryable().ElementType);
            userDbSet.As<IQueryable<Users>>().Setup(b => b.GetEnumerator()).Returns(users.AsQueryable().GetEnumerator());

            var repository = new Mock<IRepository>();
            repository.Setup(r => r.Users).Returns(userDbSet.Object);
            var userService = new UserService(repository.Object);

            // Act
            var result = userService.Authenticate(user1.Username, "incorrectpassword");

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void GetUsers_WithValidList_ListOfUsers()
        {
            // Arrange
            var users = GetMockedList(2);

            var usersDBSet = new Mock<DbSet<Users>>();
            usersDBSet.As<IQueryable<Users>>().Setup(b => b.Provider).Returns(users.AsQueryable().Provider);
            usersDBSet.As<IQueryable<Users>>().Setup(b => b.Expression).Returns(users.AsQueryable().Expression);
            usersDBSet.As<IQueryable<Users>>().Setup(b => b.ElementType).Returns(users.AsQueryable().ElementType);
            usersDBSet.As<IQueryable<Users>>().Setup(b => b.GetEnumerator()).Returns(users.AsQueryable().GetEnumerator());

            var repository = new Mock<IRepository>();
            repository.Setup(r => r.Users).Returns(usersDBSet.Object);

            var userService = new UserService(repository.Object);

            // Act
            var result = userService.GetUsers();

            // Assert
            result.Should().HaveCount(2);
            result[0].Name.Should().Contain("User1");
            result[1].Name.Should().Contain("User2");
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        public void GetUserById_WithValidList_ReturnsValidItem(int id)
        {
            // Arrange
            var users = GetMockedList(5);

            var usersDbSet = new Mock<DbSet<Users>>();
            usersDbSet.As<IQueryable<Users>>().Setup(b => b.Provider).Returns(users.AsQueryable().Provider);
            usersDbSet.As<IQueryable<Users>>().Setup(b => b.Expression).Returns(users.AsQueryable().Expression);
            usersDbSet.As<IQueryable<Users>>().Setup(b => b.ElementType).Returns(users.AsQueryable().ElementType);
            usersDbSet.As<IQueryable<Users>>().Setup(b => b.GetEnumerator()).Returns(users.AsQueryable().GetEnumerator());

            var repository = new Mock<IRepository>();
            repository.Setup(r => r.Users).Returns(usersDbSet.Object);

            var userService = new UserService(repository.Object);

            // Act
            var result = userService.GetUserById(id);

            // Assert
            result.Name.Should().Contain($"User{id}");
        }

        [TestMethod]
        [DataRow("Username1")]
        [DataRow("Username2")]
        [DataRow("Username3")]
        [DataRow("Username4")]
        [DataRow("Username5")]
        [DataRow("Username6")]
        [DataRow("Username7")]
        [DataRow("Username8")]
        [DataRow("Username9")]
        [DataRow("Username10")]
        public void GetUserByUsername_WithValidList_ReturnsValidItems(string username)
        {
            // Arrange
            var users = GetMockedList(10);

            var usersDbSet = new Mock<DbSet<Users>>();
            usersDbSet.As<IQueryable<Users>>().Setup(b => b.Provider).Returns(users.AsQueryable().Provider);
            usersDbSet.As<IQueryable<Users>>().Setup(b => b.Expression).Returns(users.AsQueryable().Expression);
            usersDbSet.As<IQueryable<Users>>().Setup(b => b.ElementType).Returns(users.AsQueryable().ElementType);
            usersDbSet.As<IQueryable<Users>>().Setup(b => b.GetEnumerator()).Returns(users.AsQueryable().GetEnumerator());

            var repository = new Mock<IRepository>();
            repository.Setup(r => r.Users).Returns(usersDbSet.Object);

            var userService = new UserService(repository.Object);

            // Act
            var result = userService.GetUserByUsername(username);

            // Assert
            result.Username.Should().Contain(username);
        }

        [TestMethod]
        public void AddUser_PopulatedList_ReturnsTrue()
        {
            // Arrange
            var users = GetMockedList(3);

            var usersDbSet = new Mock<DbSet<Users>>();
            usersDbSet.As<IQueryable<Users>>().Setup(b => b.Provider).Returns(users.AsQueryable().Provider);
            usersDbSet.As<IQueryable<Users>>().Setup(b => b.Expression).Returns(users.AsQueryable().Expression);
            usersDbSet.As<IQueryable<Users>>().Setup(b => b.ElementType).Returns(users.AsQueryable().ElementType);
            usersDbSet.As<IQueryable<Users>>().Setup(b => b.GetEnumerator()).Returns(users.AsQueryable().GetEnumerator());
            usersDbSet.Setup(b => b.Add(It.IsAny<Users>())).Callback((Users user) => users.Add(user));

            var repository = new Mock<IRepository>();
            repository.Setup(r => r.Users).Returns(usersDbSet.Object);

            var userService = new UserService(repository.Object);

            // Act
            var result = userService.AddUser(new Users() { Id = 4, Name = "User4", Username = "Username1" });

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void UpdateUser_PopulatedList_ReturnsTrue()
        {
            // Arrange
            var users = GetMockedList(3);

            var usersDbSet = new Mock<DbSet<Users>>();
            usersDbSet.As<IQueryable<Users>>().Setup(b => b.Provider).Returns(users.AsQueryable().Provider);
            usersDbSet.As<IQueryable<Users>>().Setup(b => b.Expression).Returns(users.AsQueryable().Expression);
            usersDbSet.As<IQueryable<Users>>().Setup(b => b.ElementType).Returns(users.AsQueryable().ElementType);
            usersDbSet.As<IQueryable<Users>>().Setup(b => b.GetEnumerator()).Returns(users.AsQueryable().GetEnumerator());
            usersDbSet.Setup(b => b.Add(It.IsAny<Users>())).Callback((Users user) => users.Add(user));

            var repository = new Mock<IRepository>();
            repository.Setup(r => r.Users).Returns(usersDbSet.Object);

            var userService = new UserService(repository.Object);

            // Act
            var userMod = users[0];
            userMod.Name = "John";
            var result = userService.UpdateUser(userMod);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void DeleteUser_PopulatedList_ReturnsTrue()
        {
            // Arrange
            var users = GetMockedList(3);

            var bugsDbSet = new Mock<DbSet<Users>>();
            bugsDbSet.As<IQueryable<Users>>().Setup(b => b.Provider).Returns(users.AsQueryable().Provider);
            bugsDbSet.As<IQueryable<Users>>().Setup(b => b.Expression).Returns(users.AsQueryable().Expression);
            bugsDbSet.As<IQueryable<Users>>().Setup(b => b.ElementType).Returns(users.AsQueryable().ElementType);
            bugsDbSet.As<IQueryable<Users>>().Setup(b => b.GetEnumerator()).Returns(users.AsQueryable().GetEnumerator());
            bugsDbSet.Setup(b => b.Add(It.IsAny<Users>())).Callback((Users user) => users.Remove(user));

            var repository = new Mock<IRepository>();
            repository.Setup(r => r.Users).Returns(bugsDbSet.Object);

            var userService = new UserService(repository.Object);

            // Act
            var result = userService.DeleteUser(users[0]);

            // Assert
            result.Should().BeTrue();
        }
    }
}
