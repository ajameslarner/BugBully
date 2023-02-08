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
    public class BugServiceTests
    {
        private List<Bugs> GetMockedList(int size)
        {
            var result = new List<Bugs>();
            for (int i = 1; i < size + 1; i++)
            {
                result.Add(new Bugs()
                {
                    Id = i,
                    Title = $"Bug{i}",
                    StatusId = i,
                    Status = new Statuses()
                    {
                        Id = i,
                        Name = $"Status{i}"
                    },
                    UserId = i,
                    User = new Users()
                    {
                        Id = i,
                        Name = $"User{i}",
                        Username = $"Username{i}"
                    }
                });
            }
            return result;
        }

        [TestMethod]
        public void GetBugs_WithValidList_ListOfBugs()
        {
            // Arrange
            var bugs = GetMockedList(2);

            var bugsDbSet = new Mock<DbSet<Bugs>>();
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.Provider).Returns(bugs.AsQueryable().Provider);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.Expression).Returns(bugs.AsQueryable().Expression);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.ElementType).Returns(bugs.AsQueryable().ElementType);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.GetEnumerator()).Returns(bugs.AsQueryable().GetEnumerator());

            var repository = new Mock<IRepository>();
            repository.Setup(r => r.Bugs).Returns(bugsDbSet.Object);

            var bugService = new BugService(repository.Object);

            // Act
            var result = bugService.GetBugs();

            // Assert
            result.Should().HaveCount(2);
            result[0].Title.Should().Contain("Bug1");
            result[1].Title.Should().Contain("Bug2");
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        public void GetBugById_WithValidList_ReturnsValidItem(int id)
        {
            // Arrange
            var bugs = GetMockedList(5);

            var bugsDbSet = new Mock<DbSet<Bugs>>();
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.Provider).Returns(bugs.AsQueryable().Provider);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.Expression).Returns(bugs.AsQueryable().Expression);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.ElementType).Returns(bugs.AsQueryable().ElementType);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.GetEnumerator()).Returns(bugs.AsQueryable().GetEnumerator());

            var repository = new Mock<IRepository>();
            repository.Setup(r => r.Bugs).Returns(bugsDbSet.Object);

            var bugService = new BugService(repository.Object);

            // Act
            var result = bugService.GetBugById(id);

            // Assert
            result.Title.Should().Contain($"Bug{id}");
        }

        [TestMethod]
        [DataRow("Status1")]
        [DataRow("Status2")]
        [DataRow("Status3")]
        [DataRow("Status4")]
        [DataRow("Status5")]
        [DataRow("Status6")]
        [DataRow("Status7")]
        [DataRow("Status8")]
        [DataRow("Status9")]
        [DataRow("Status10")]
        public void GetBugsByStatus_WithValidList_ReturnsValidItems(string status)
        {
            // Arrange
            var bugs = GetMockedList(10);

            var bugsDbSet = new Mock<DbSet<Bugs>>();
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.Provider).Returns(bugs.AsQueryable().Provider);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.Expression).Returns(bugs.AsQueryable().Expression);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.ElementType).Returns(bugs.AsQueryable().ElementType);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.GetEnumerator()).Returns(bugs.AsQueryable().GetEnumerator());

            var repository = new Mock<IRepository>();
            repository.Setup(r => r.Bugs).Returns(bugsDbSet.Object);

            var bugService = new BugService(repository.Object);

            // Act
            var result = bugService.GetBugsByStatus(status);

            // Assert
            result.Should().HaveCount(1);
            result[0].Status.Name.Should().Contain(status);
        }

        [TestMethod]
        [DataRow("Username1")]
        [DataRow("Username2")]
        [DataRow("Username3")]
        [DataRow("Username4")]
        [DataRow("Username5")]
        public void GetBugsAssignedToUser_WithValidList_ReturnsValidItems(string username)
        {
            // Arrange
            var bugs = GetMockedList(5);

            var bugsDbSet = new Mock<DbSet<Bugs>>();
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.Provider).Returns(bugs.AsQueryable().Provider);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.Expression).Returns(bugs.AsQueryable().Expression);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.ElementType).Returns(bugs.AsQueryable().ElementType);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.GetEnumerator()).Returns(bugs.AsQueryable().GetEnumerator());

            var repository = new Mock<IRepository>();
            repository.Setup(r => r.Bugs).Returns(bugsDbSet.Object);

            var bugService = new BugService(repository.Object);

            // Act
            var result = bugService.GetBugsAssignedToUser(username);

            // Assert
            result.Should().HaveCount(1);
            result[0].User.Username.Should().Contain(username);
        }

        [TestMethod]
        public void AddBug_PopulatedList_ReturnsTrue()
        {
            // Arrange
            var bugs = GetMockedList(3);

            var bugsDbSet = new Mock<DbSet<Bugs>>();
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.Provider).Returns(bugs.AsQueryable().Provider);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.Expression).Returns(bugs.AsQueryable().Expression);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.ElementType).Returns(bugs.AsQueryable().ElementType);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.GetEnumerator()).Returns(bugs.AsQueryable().GetEnumerator());
            bugsDbSet.Setup(b => b.Add(It.IsAny<Bugs>())).Callback((Bugs bug) => bugs.Add(bug));

            var repository = new Mock<IRepository>();
            repository.Setup(r => r.Bugs).Returns(bugsDbSet.Object);

            var bugService = new BugService(repository.Object);

            // Act
            var bug4 = new Bugs() { Id = 4, Title = "Bug 4" };
            var result = bugService.AddBug(bug4);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void UpdateBug_PopulatedList_ReturnsTrue()
        {
            // Arrange
            var bugs = GetMockedList(3);

            var bugsDbSet = new Mock<DbSet<Bugs>>();
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.Provider).Returns(bugs.AsQueryable().Provider);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.Expression).Returns(bugs.AsQueryable().Expression);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.ElementType).Returns(bugs.AsQueryable().ElementType);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.GetEnumerator()).Returns(bugs.AsQueryable().GetEnumerator());
            bugsDbSet.Setup(b => b.Add(It.IsAny<Bugs>())).Callback((Bugs bug) => bugs.Add(bug));

            var repository = new Mock<IRepository>();
            repository.Setup(r => r.Bugs).Returns(bugsDbSet.Object);

            var bugService = new BugService(repository.Object);

            // Act
            var bug4 = new Bugs() { Id = 4, Title = "Bug 4" };
            var result = bugService.UpdateBug(bug4);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void DeleteBug_PopulatedList_ReturnsTrue()
        {
            // Arrange
            var bugs = GetMockedList(3);

            var bugsDbSet = new Mock<DbSet<Bugs>>();
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.Provider).Returns(bugs.AsQueryable().Provider);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.Expression).Returns(bugs.AsQueryable().Expression);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.ElementType).Returns(bugs.AsQueryable().ElementType);
            bugsDbSet.As<IQueryable<Bugs>>().Setup(b => b.GetEnumerator()).Returns(bugs.AsQueryable().GetEnumerator());
            bugsDbSet.Setup(b => b.Add(It.IsAny<Bugs>())).Callback((Bugs bug) => bugs.Add(bug));

            var repository = new Mock<IRepository>();
            repository.Setup(r => r.Bugs).Returns(bugsDbSet.Object);

            var bugService = new BugService(repository.Object);

            // Act
            var bug4 = new Bugs() { Id = 4, Title = "Bug 4" };
            var result = bugService.DeleteBug(bug4);

            // Assert
            result.Should().BeTrue();
        }
    }
}
