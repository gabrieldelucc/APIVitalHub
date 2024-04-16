namespace WebAPI.Utils.Mail
{
    public interface IEmailService
    {
        //método assíncrono para envio de email 
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
