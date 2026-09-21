using EasyStock.Data;
using EasyStock.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace EasyStock.Service
{
    public class NotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailSettings _settings;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ApplicationDbContext context, IOptions<EmailSettings> options, ILogger<NotificationService> logger)
        {
            _context = context;
            _settings = options.Value;
            _logger = logger;
        }

        public async Task<bool> SendLowStockAlertAsync(Article article, string triggeredBy)
        {
            if (!_settings.Enabled)
            {
                return false;
            }

            var recipients = await ResolveRecipientsAsync();
            if (!recipients.Any())
            {
                _logger.LogInformation("Notification stock faible ignorée: aucun destinataire configuré.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(_settings.SmtpServer) || string.IsNullOrWhiteSpace(_settings.FromEmail))
            {
                _logger.LogWarning("Notification stock faible ignorée: configuration SMTP incomplète.");
                return false;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            foreach (var recipient in recipients)
            {
                message.To.Add(MailboxAddress.Parse(recipient));
            }

            var statusColor = article.NombreArticleActuel <= 0 ? "#d93025" : "#f59e0b";
            var statusLabel = article.NombreArticleActuel <= 0 ? "Rupture imminente" : "Stock faible";
            var generatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

            message.Subject = $"Alerte stock faible - {article.Name}";
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = $@"<!DOCTYPE html>
<html lang='fr'>
<head>
<meta charset='utf-8' />
<meta name='viewport' content='width=device-width, initial-scale=1.0' />
<title>Alerte stock faible</title>
</head>
<body style='margin:0;padding:0;background:#eef5ff;font-family:Segoe UI,Arial,sans-serif;color:#14213d;'>
  <div style='padding:32px 16px;background:linear-gradient(180deg,#eef5ff 0%,#f8fbff 100%);'>
    <table role='presentation' cellpadding='0' cellspacing='0' border='0' width='100%' style='max-width:680px;margin:0 auto;border-collapse:separate;'>
      <tr>
        <td style='padding:0;'>
          <div style='background:linear-gradient(135deg,#0f4cde 0%,#2aa4f4 100%);border-radius:28px 28px 0 0;padding:28px 32px;color:#ffffff;box-shadow:0 20px 45px rgba(25,76,173,0.18);'>
            <div style='font-size:13px;letter-spacing:0.18em;text-transform:uppercase;opacity:0.88;'>EasyStock</div>
            <h1 style='margin:10px 0 8px 0;font-size:32px;line-height:1.15;font-weight:800;'>Alerte de stock faible</h1>
            <p style='margin:0;font-size:15px;line-height:1.6;max-width:520px;opacity:0.95;'>Un équipement nécessite une attention rapide. Le niveau disponible a atteint ou dépassé le seuil minimal défini.</p>
          </div>
        </td>
      </tr>
      <tr>
        <td style='padding:0;'>
          <div style='background:#ffffff;border-radius:0 0 28px 28px;padding:32px;box-shadow:0 20px 45px rgba(18,38,63,0.10);'>
            <div style='display:inline-block;padding:8px 14px;border-radius:999px;background:{statusColor};color:#ffffff;font-size:13px;font-weight:700;letter-spacing:0.02em;margin-bottom:20px;'>{statusLabel}</div>
            <h2 style='margin:0 0 8px 0;font-size:26px;line-height:1.2;color:#0f172a;'>{article.Name}</h2>
            <p style='margin:0 0 24px 0;font-size:15px;line-height:1.7;color:#475569;'>Le système a détecté qu'un mouvement récent a fait descendre ce stock à un niveau critique. Un réapprovisionnement ou une vérification opérationnelle est recommandé.</p>

            <table role='presentation' cellpadding='0' cellspacing='0' border='0' width='100%' style='border-collapse:separate;border-spacing:0 12px;'>
              <tr>
                <td style='width:50%;padding-right:8px;'>
                  <div style='background:#f8fbff;border:1px solid #dbeafe;border-radius:20px;padding:18px 20px;'>
                    <div style='font-size:12px;text-transform:uppercase;letter-spacing:0.08em;color:#64748b;margin-bottom:8px;'>Stock disponible</div>
                    <div style='font-size:30px;font-weight:800;color:#0f172a;'>{article.NombreArticleActuel}</div>
                  </div>
                </td>
                <td style='width:50%;padding-left:8px;'>
                  <div style='background:#fffaf0;border:1px solid #fde68a;border-radius:20px;padding:18px 20px;'>
                    <div style='font-size:12px;text-transform:uppercase;letter-spacing:0.08em;color:#92400e;margin-bottom:8px;'>Seuil minimal</div>
                    <div style='font-size:30px;font-weight:800;color:#7c2d12;'>{article.NombreArticleMinimum}</div>
                  </div>
                </td>
              </tr>
            </table>

            <div style='margin-top:10px;background:#f8fafc;border:1px solid #e2e8f0;border-radius:22px;padding:20px 22px;'>
              <div style='font-size:14px;line-height:1.9;color:#334155;'>
                <strong>Déclenché par :</strong> {triggeredBy}<br/>
                <strong>Date locale :</strong> {generatedAt:yyyy-MM-dd HH:mm}<br/>
                <strong>Base de décision :</strong> le stock disponible est inférieur ou égal au seuil minimal configuré.
              </div>
            </div>

            <div style='margin-top:26px;padding:18px 20px;border-radius:20px;background:linear-gradient(135deg,#eff6ff 0%,#f8fafc 100%);border:1px solid #dbeafe;'>
              <div style='font-size:14px;line-height:1.7;color:#1e3a8a;'><strong>Action recommandée :</strong> vérifier les besoins en réapprovisionnement, confirmer les affectations récentes et planifier une entrée de stock si nécessaire.</div>
            </div>
          </div>
        </td>
      </tr>
      <tr>
        <td style='padding:18px 8px 0 8px;'>
          <div style='text-align:center;font-size:12px;line-height:1.7;color:#64748b;'>
            Message automatique envoyé par <strong>EasyStock</strong>.<br/>
            Ce courriel est généré lors d'un mouvement ou d'une affectation provoquant un stock faible.
          </div>
        </td>
      </tr>
    </table>
  </div>
</body>
</html>"
            };

            try
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, ResolveSocketOptions());
                if (!string.IsNullOrWhiteSpace(_settings.Username))
                {
                    await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
                }

                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
                _logger.LogInformation("Notification stock faible envoyée pour l'article {ArticleId} aux destinataires {Recipients}.", article.ArticleId, string.Join(", ", recipients));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Échec de l'envoi de la notification stock faible pour l'article {ArticleId}.", article.ArticleId);
                return false;
            }
        }

        private SecureSocketOptions ResolveSocketOptions()
        {
            if (_settings.UseStartTls)
            {
                return SecureSocketOptions.StartTls;
            }

            if (_settings.EnableSsl)
            {
                return SecureSocketOptions.SslOnConnect;
            }

            return SecureSocketOptions.Auto;
        }

        private async Task<List<string>> ResolveRecipientsAsync()
        {
            if (_settings.LowStockRecipients.Any())
            {
                return _settings.LowStockRecipients
                    .Where(email => !string.IsNullOrWhiteSpace(email))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(_settings.RecipientEmail))
            {
                return new List<string> { _settings.RecipientEmail.Trim() };
            }

            return await _context.User
                .Include(u => u.Role)
                .Where(u => u.Role != null && (u.Role.RoleName == "Admin" || u.Role.RoleName == "Gestionnaire"))
                .Select(u => u.Email)
                .Distinct()
                .ToListAsync();
        }
    }
}
