namespace EasyStock.Data
{
    public class EmailSettings
    {
        public bool Enabled { get; set; }
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string SenderEmail
        {
            get => FromEmail;
            set => FromEmail = value;
        }

        public string FromName { get; set; } = string.Empty;
        public string DisplayName
        {
            get => FromName;
            set => FromName = value;
        }

        public bool EnableSsl { get; set; }
        public bool UseStartTls { get; set; }
        public List<string> LowStockRecipients { get; set; } = new();
        public string RecipientEmail { get; set; } = string.Empty;
    }
}
