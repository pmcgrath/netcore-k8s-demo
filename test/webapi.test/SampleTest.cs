using Xunit;


namespace webapi.test
{
    public class SampleTest
    {
        [Fact]
        public void JustToIllustrate()
        {
            // Nothing yet - just to illustrate tests at this time
            var n = 1 +1;
            Assert.True(n == 2, $"Bang ! Wheres my {n}");
        }
    }
}