using ApiTest.Controllers;

namespace ApiUnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Today_is_not_chrismas()
        {
            var testController = new testIsChrismas(new DateTime(2025, 11, 25));
            var expected = "Today is not Chrismas";
            var result = testController.IsChrismas();
            Assert.AreEqual(expected,result);
        }

        [Test]
        public void Today_is_chrismas()
        {
            var testController = new testIsChrismas(new DateTime(2025,12,25));
            var expected = "Today is Chrismas";
            var result = testController.IsChrismas();
            Assert.AreEqual(expected, result);
        }
    }

    public class testIsChrismas : TestService
    {
        private readonly DateTime _dateTime;
        public testIsChrismas(DateTime dateTime)
        {
            _dateTime = dateTime;
        }

        protected override DateTime GetDateToCheck()
        {
            return _dateTime;
        }
    }
}