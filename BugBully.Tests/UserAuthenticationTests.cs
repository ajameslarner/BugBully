using BugBully.Models;
using Moq;
using FluentAssertions;
using BugBully.Data;
using BugBully.Services;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BugBully.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void HashPassword_ShouldReturnHashedPassword()
        {
            // Arrange
            var password = "password";

            // Act
            var result = UserService.HashPassword(password);

            // Assert
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().NotBe(password);
        }

        [TestMethod]
        public void VerifyPasswordHash_ForValidPassword_ShouldReturnTrue()
        {
            // Arrange
            var password = "password";
            var hashedPassword = UserService.HashPassword(password);

            // Act
            var result = UserService.VerifyPasswordHash(password, hashedPassword);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void VerifyPasswordHash_ForInvalidPassword_ShouldReturnFalse()
        {
            // Arrange
            var password = "password";
            var hashedPassword = UserService.HashPassword(password);

            // Act
            var result = UserService.VerifyPasswordHash("invalidPassword", hashedPassword);

            // Assert
            result.Should().BeFalse();
        }
    }
}