using Common.Email.Package.MessageInformation.Interfaces;

namespace Common.Email.Package.MessageInformation
{
    public class EmailAddress : IEmailAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}