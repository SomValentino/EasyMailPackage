using System;
using Common.Email.Package.Enums;
using Common.Email.Package.Exceptions;

namespace Common.Email.Package.Services
{
    public class EmailMessageResult
    {
        public Status Status{ get; set; }
        public Exception Exception { get; set; }
    }
}