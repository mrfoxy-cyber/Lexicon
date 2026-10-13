using Encapsulation.Domain;

namespace Encapsulation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void ToString_ReturnsCorrectFormat()
        {
            //Arrange

            var person = new Encapsulation.Domain.Person("John", "Doe", 30);
            //Act
            var result = person.ToString();
            //Assert
            Assert.Equal("John Doe is 30 years old.", result);
        }
    }
}
