using Common.Email.Package.Configuration;

namespace Common.Email.Package.Tests.Helpers
{
    public class TestEmailConfiguration : IEmailConfiguration
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
    }
}