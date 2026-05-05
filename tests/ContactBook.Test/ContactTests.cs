using System;
using ContactBook;
using Xunit;

namespace ContactBook.Tests
{
    public class ContactTests
    {
        // ---------------------------------------------------------
        // Constructor & Getter Tests
        // ---------------------------------------------------------
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            var c = new Contact("John", "Doe", "123", "john@example.com");

            Assert.Equal("John", c.GetFName());
            Assert.Equal("Doe", c.GetLName());
            Assert.Equal("123", c.GetPhone());
            Assert.Equal("john@example.com", c.GetEmail());
        }

        [Fact]
        public void Constructor_ShouldAllowEmptyValues()
        {
            var c = new Contact();

            Assert.Equal("", c.GetFName());
            Assert.Equal("", c.GetLName());
            Assert.Equal("", c.GetPhone());
            Assert.Equal("", c.GetEmail());
        }

        // ---------------------------------------------------------
        // Setter Tests
        // ---------------------------------------------------------
        [Fact]
        public void SetFName_ShouldUpdateValue()
        {
            var c = new Contact();
            c.SetFName("Alice");

            Assert.Equal("Alice", c.GetFName());
        }

        [Fact]
        public void SetLName_ShouldUpdateValue()
        {
            var c = new Contact();
            c.SetLName("Smith");

            Assert.Equal("Smith", c.GetLName());
        }

        [Fact]
        public void SetPhone_ShouldUpdateValue()
        {
            var c = new Contact();
            c.SetPhone("555-1234");

            Assert.Equal("555-1234", c.GetPhone());
        }

        [Fact]
        public void SetEmail_ShouldUpdateValue()
        {
            var c = new Contact();
            c.SetEmail("test@example.com");

            Assert.Equal("test@example.com", c.GetEmail());
        }

        // ---------------------------------------------------------
        // ToString Tests
        // ---------------------------------------------------------
        [Fact]
        public void ToString_ShouldReturnFormattedString()
        {
            var c = new Contact("John", "Doe", "123", "john@example.com");

            var expected = "Contact[fname=John, lname=Doe, phone=123, email=john@example.com]";
            Assert.Equal(expected, c.ToString());
        }

        // ---------------------------------------------------------
        // Equality Tests
        // ---------------------------------------------------------
        [Fact]
        public void Equals_ShouldReturnTrue_ForSameReference()
        {
            var c = new Contact("A", "B", "C", "D");

            Assert.True(c.Equals(c));
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenOtherIsNull()
        {
            var c = new Contact("A", "B", "C", "D");

            Assert.False(c.Equals(null));
        }

        [Fact]
        public void Equals_ShouldReturnTrue_ForIdenticalValues()
        {
            var c1 = new Contact("A", "B", "C", "D");
            var c2 = new Contact("A", "B", "C", "D");

            Assert.True(c1.Equals(c2));
            Assert.True(c2.Equals(c1));
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForDifferentValues()
        {
            var c1 = new Contact("A", "B", "C", "D");
            var c2 = new Contact("X", "Y", "Z", "W");

            Assert.False(c1.Equals(c2));
        }

        [Fact]
        public void EqualsObject_ShouldReturnFalse_ForDifferentType()
        {
            var c = new Contact("A", "B", "C", "D");

            Assert.False(c.Equals("not a contact"));
        }

        // ---------------------------------------------------------
        // Operator == and != Tests
        // ---------------------------------------------------------
        [Fact]
        public void OperatorEquals_ShouldReturnTrue_WhenBothNull()
        {
            Contact? c1 = null;
            Contact? c2 = null;

            Assert.True(c1 == c2);
        }

        [Fact]
        public void OperatorEquals_ShouldReturnFalse_WhenOneNull()
        {
            Contact? c1 = new Contact("A");
            Contact? c2 = null;

            Assert.False(c1 == c2);
            Assert.False(c2 == c1);
        }

        [Fact]
        public void OperatorEquals_ShouldReturnTrue_ForEqualObjects()
        {
            var c1 = new Contact("A", "B", "C", "D");
            var c2 = new Contact("A", "B", "C", "D");

            Assert.True(c1 == c2);
        }

        [Fact]
        public void OperatorNotEquals_ShouldReturnTrue_ForDifferentObjects()
        {
            var c1 = new Contact("A", "B", "C", "D");
            var c2 = new Contact("X", "Y", "Z", "W");

            Assert.True(c1 != c2);
        }

        // ---------------------------------------------------------
        // GetHashCode Tests
        // ---------------------------------------------------------
        [Fact]
        public void GetHashCode_ShouldMatchForEqualObjects()
        {
            var c1 = new Contact("A", "B", "C", "D");
            var c2 = new Contact("A", "B", "C", "D");

            Assert.Equal(c1.GetHashCode(), c2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_ShouldDifferForDifferentObjects()
        {
            var c1 = new Contact("A", "B", "C", "D");
            var c2 = new Contact("X", "Y", "Z", "W");

            Assert.NotEqual(c1.GetHashCode(), c2.GetHashCode());
        }
    }
}
