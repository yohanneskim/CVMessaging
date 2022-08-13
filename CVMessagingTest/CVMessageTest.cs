namespace CVMessagingTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async  Task GmailTest()
        {
            //assign
            var from = "test@test.com";
            var to = "test@test.com";

            //act
            var gmail = new CVMessagingService.CVMessagingService();

            var answer = await gmail.SendGmail(from, to, "subject","body","cc","bcc","c:\\temp\\test1.pdf");
            //assert
            Assert.That(answer, Is.EqualTo(true));
        }
    }
}