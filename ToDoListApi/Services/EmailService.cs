using Data;
using MailKit.Net.Smtp;
using MimeKit;

namespace ToDoListApi.Services
{
  public class EmailService
  {
    private readonly EmailConfiguration _emailConfiguration;

    public EmailService(EmailConfiguration emailConfiguration)
    {
      _emailConfiguration = emailConfiguration ?? throw new ArgumentNullException(nameof(emailConfiguration));
    }


    public bool SendEmail(Message message)
    {
      try
      {
        var emailMessage = CreateEmailMessage(message);

        Send(emailMessage);
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    private MimeMessage CreateEmailMessage(Message message)
    {
      try
      {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailConfiguration.From));
        emailMessage.To.Add(new MailboxAddress(_emailConfiguration.RedirectTo));
        emailMessage.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = message.Content;
        emailMessage.Body = bodyBuilder.ToMessageBody();

        return emailMessage;
      }
      catch (Exception)
      {
        throw;
      }
    }

    private void Send(MimeMessage mailMessage)
    {
      using (var client = new SmtpClient())
      {
        try
        {
          client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
          client.AuthenticationMechanisms.Remove("XOAUTH2");
          client.Authenticate(_emailConfiguration.UserName, _emailConfiguration.Password);

          client.Send(mailMessage);
        }
        catch (Exception ex)
        {
          throw;
        }
        finally
        {
          client.Disconnect(true);
          client.Dispose();
        }
      }
    }

  }

  public class EmailConfiguration
  {
    public string From { get; set; }
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string RedirectTo { get; set; }
  }

  public class Message
  {
    public string Subject { get; set; }
    public string Content { get; set; }

    public Message(int incommingAmount, int deleyedAmount)
    {
      Subject = "Powiadonienie o dadchodzących i zaległych zadaniach";
      Content = @$"Liczba zbliżająctch się zadań: {incommingAmount}. 
                  Liczba zaległych zadań: {deleyedAmount}. 
                  Przejdź do aplikacji aby sprawdzić zadania.";
    }
  }
}
