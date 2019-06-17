# EasyMailPackage
Nuget package for sending quick emails. This abstraction over mailkit that does the following:

1) Sends emails using smtp configurations
2) Retrieves emails from server using IMAP or POP3 settings.
3) Delete emails from server uisng IMAP or POP3 settings.


## Send email

```C#
// Create emailConfiguration for smtp
IEmailConfiguration smtpEmailConfiguration = new EmailConfiguration{
  ServerAddress = "smtp.gmail.com",
  Username = "YOUREMAIL",
  Password = "PASSWORD",
  Port = XXXX,
  RequireSSL = false // depends on your server settings
}

// Create an instance of EmailMessage builder
var _testEmailMessageBuilder = new EmailMessageBuilder();
var message = _testEmailMessageBuilder.AddFromAddress("yourname","ToEmailAddress")
                .AddToAddresses("Receipent name","FromEmailAddress")
                .AddSubject("Test Email")
                .AddBody("Test Email")
                .AddAttachment(new List<Attachment> { new Attachment{FileName = "full path FileName"})
                .Build();
                
// create an instance of the EmailService
IEmailService _emailService = new EmailService();
var messageResult = await _emailService.SendAsync(message,smtpEmailConfiguration);

if(messageResult.Status == Status.Success){
   // todo
}
```

## Retrieve emails from server

```C#
// Create emailConfiguration for imap server
IEmailConfiguration imapEmailConfiguration = new EmailConfiguration{
  ServerAddress = "imap.gmail.com",
  Username = "YOUREMAIL",
  Password = "PASSWORD",
  Port = XXXX,
  RequireSSL = true // depends on your server settings
}

// Create emailConfiguration for POP server
IEmailConfiguration popEmailConfiguration = new EmailConfiguration{
  ServerAddress = "pop.gmail.com", // use your server settings
  Username = "YOUREMAIL",
  Password = "PASSWORD",
  Port = XXXX,
  RequireSSL = false // depends on your server settings
}

// create an instance of the EmailService
IEmailService _emailService = new EmailService();
List<ExtractedEmailMessage> extractedEmailMessages = await _emailService.
GetEmailMessageFromServerViaImapAsync(imapEmailConfiguration, batchSize); // batchSize means the number of //emails to retrieve from the server using imap protocol

List<ExtractedEmailMessage> extractedEmailMessages = await _emailService.
GetEmailMessageFromServerViaPopAsync(popEmailConfiguration, batchSize); // batchSize means the number of //emails to retrieve from the server using pop protocol

// Note that Attachments are returned as base64 string 
```

## Delete emails from server

```C#
// create an instance of the EmailService
IEmailService _emailService = new EmailService();

await _emailService.DeleteEmailMessageFromImapServerAsync(extractedEmailMessages,
             imapEmailConfiguration);

await _emailService.DeleteEmailMessageFromPopServerAsync(extractedEmailMessages,
             popEmailConfiguration);

```
