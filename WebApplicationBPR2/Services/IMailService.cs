namespace WebApplicationBPR2.Services
{
    public interface IMailService
    {
        void SendMail(string email, string subject, string mess);
    }
}