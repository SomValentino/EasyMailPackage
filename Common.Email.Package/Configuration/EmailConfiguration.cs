namespace Common.Email.Package.Configuration
{
    public class EmailConfiguration : IEmailConfiguration
    {
        public string  ServerAddress { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        
        public bool RequireSSL { get; set; }
    }
}