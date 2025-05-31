namespace Billi.Backend.CrossCutting.Configurations
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = null!;
        public int Port { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool UseSsl { get; set; }
        public string SenderName { get; set; } = null!;
        public string SenderEmail { get; set; } = null!;
    }
}
