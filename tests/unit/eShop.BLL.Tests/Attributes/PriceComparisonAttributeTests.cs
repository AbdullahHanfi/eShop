using eShop.BLL.Attributes;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace eShop.BLL.Tests.Attributes
{
    internal class PriceComparisonValidModel
    {
        public decimal PrevValue { get; set; }
        [PriceComparison(nameof(PrevValue), "previse value should be bigger than current one")]
        public decimal CurrentValue { get; set; }
    }
    internal class PriceComparisonWrongPropertyModel
    {
        public decimal PrevValue { get; set; }
        [PriceComparison("Value", "previse value should be bigger than current one")]
        public decimal CurrentValue { get; set; }
    }
    public class PriceComparisonAttributeTests
    {
        [Fact]
        public void IsValid_Should_ReturnsSuccess_WhenPreviousIsGreater()
        {
            // Arrange
            PriceComparisonValidModel testCase = new() { PrevValue = 50, CurrentValue = 20 };
            var context = new ValidationContext(testCase)
            {
                MemberName = nameof(testCase.CurrentValue)
            };
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateProperty(
                testCase.CurrentValue,
                context,
                results);

            // Assert
            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void IsValid_Should_ReturnsFalse_WhenPreviousIsSmaller()
        {
            // Arrange
            PriceComparisonValidModel testCase = new() { PrevValue = 10, CurrentValue = 20 };
            var context = new ValidationContext(testCase)
            {
                MemberName = nameof(testCase.CurrentValue)
            };
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateProperty(
                testCase.CurrentValue,
                context,
                results);

            // Assert
            Assert.False(isValid);
            Assert.Single(results);
            Assert.Equal("previse value should be bigger than current one", results.First().ErrorMessage);
        }

        [Fact]
        public void IsValid_Should_ReturnError_ForUnknownProperty_WhenWrongProperyNamePassed()
        {
            // Arrange
            PriceComparisonWrongPropertyModel testCase = new()
            {
                PrevValue = 50,
                CurrentValue = 20
            };
            var context = new ValidationContext(testCase)
            {
                MemberName = nameof(testCase.CurrentValue)
            };
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateProperty(
                testCase.CurrentValue,
                context,
                results);

            // Assert
            Assert.False(isValid);
            Assert.Single(results);
            Assert.Equal("Unknown property",results.First().ErrorMessage);
        }
    }
}
