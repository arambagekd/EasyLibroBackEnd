using MimeKit;

namespace Buisness_Logic_Layer.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendEmail(TextPart text, String To, String Subject);
    }
}
