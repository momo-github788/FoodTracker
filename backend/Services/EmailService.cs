namespace backend.Services {
    public interface EmailService {

        void sendEmail(string to, string subject, string body, string from = null);
    }
}
