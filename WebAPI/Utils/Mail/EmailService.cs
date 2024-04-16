
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;


namespace WebAPI.Utils.Mail
{
    public class EmailService : IEmailService
    {
        //variável que armazena as ações do EmailSettings 
        private readonly EmailSettings emailsettings;

        //construtor que recebe a dependence injection da EmailSettings 
        public EmailService(IOptions<EmailSettings> options)
        {
            emailsettings = options.Value;

        }



        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                //objeto que representa o email 
                var email = new MimeMessage();

                // definimos o remetente do email 
                email.Sender = MailboxAddress.Parse(emailsettings.Email);

                //define o destinatário do email 
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));

                // define o assunto do email 
                email.Subject = mailRequest.Subject;

                // cria o corpo do email 
                var builder = new BodyBuilder();

                // define o corpo do email como html 
                builder.HtmlBody = mailRequest.Body;

                //define o corpo do email no objeto MimeMessage 
                email.Body = builder.ToMessageBody();

                //cria um Client SMTP para envio de email 
                using (var smtp = new SmtpClient())
                {

                    //conecta ao servidor SMTP usando os dados de emailSettings 
                    smtp.Connect(emailsettings.Host, emailsettings.Port, SecureSocketOptions.StartTls);

                    // autentica no servidor SMTP usando os dados de emailSettings 
                    smtp.Authenticate(emailsettings.Email, emailsettings.Password);

                    // envia o email 
                    await smtp.SendAsync(email);

                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
