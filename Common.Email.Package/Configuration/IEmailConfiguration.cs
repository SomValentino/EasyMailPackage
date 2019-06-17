namespace Common.Email.Package.Configuration
{
    public interface IEmailConfiguration
    {
        string  ServerAddress { get; set; }
        int Port { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        bool RequireSSL { get; set; }
    }
}