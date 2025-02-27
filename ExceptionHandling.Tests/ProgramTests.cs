namespace ExceptionHandling.Tests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void ReadNumber_42_ReturnsNumber()
        {
            // Arrange
            var expected = 42;

            // Act
            var result = Program.ReadNumber("42");

            // Assert
            Assert.IsNotNull(result, "Die Zahl wurde nicht eingelesen!");
            Assert.AreEqual(expected, result, "Die Zahl stimmt nicht ueberein!");
        }

        [TestMethod]
        [DataRow(42, "42")]
        [DataRow(-1, "-1")]
        [DataRow(int.MaxValue, "2147483647")]
        public void ReadNumber_ValidNumber_ReturnsNumber(int expected, string input)
        {
            // Act
            var result = Program.ReadNumber(input);

            // Assert
            Assert.IsNotNull(result, "Die Zahl wurde nicht eingelesen!");
            Assert.AreEqual(expected, result, "Die Zahl stimmt nicht ueberein!");
        }

        [TestMethod]
        [DataRow("foo")]
        [DataRow(null)]
        public void ReadNumber_InvalidNumber_ReturnsNumber(string input)
        {
            // Act
            var result = Program.ReadNumber(input);

            // Assert
            Assert.IsNull(result, "Die Zahl sollte null sein!");
        }

        [TestMethod]
        public void ReadNumber_UnluckyNumber_ThrowsUnluckyNumberException()
        {
            // Act
            Action action = () => Program.ReadNumber("26");

            // Assert
            Assert.ThrowsException<UnluckyNumberException>(action);
        }
    }
}