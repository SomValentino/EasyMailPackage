using System.IO;

namespace Common.Email.Package.MessageInformation
{
    public class Attachment
    {
        public string FileName { get; set; }
        public Stream FileStream { get; set; }
        public string MediaType {get; set;}
        public string MediaSubType {get; set;}
    }
}