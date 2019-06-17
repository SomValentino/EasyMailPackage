using Common.Email.Package.Configuration;

namespace Common.Email.Package.Tests.Helpers
{
    public class TestEmailConfiguration : IEmailConfiguration
    {
        public string ServerAddress { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RequireSSL { get; set; }
    }
}