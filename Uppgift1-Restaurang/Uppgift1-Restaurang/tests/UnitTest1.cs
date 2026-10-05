namespace Uppgift1_Restaurang
{
    public class UnitTest1
    {
        [Fact]
        public void IsUnitTestConnected()
        {
            Assert.Equal("Hello, World!", new Restaurant.Core.Program().HelloWorld());
        }
    }
}
