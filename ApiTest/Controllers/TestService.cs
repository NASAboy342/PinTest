namespace ApiTest.Controllers
{
    public class TestService
    {
        public string IsChrismas()
        {
            var chrismasDay = new DateTime(2025, 12, 25);

            return chrismasDay.Month == GetDateToCheck().Month && chrismasDay.Day == GetDateToCheck().Day ? "Today is Chrismas" : "Today is not Chrismas";
        }

        protected virtual DateTime GetDateToCheck()
        {
            return DateTime.Today;
        }
    }
}