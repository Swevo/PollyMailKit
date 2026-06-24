/// <summary>
/// SMTP connection settings for use with <see cref="ResilientSmtpSender"/>.
/// </summary>
/// <param name="Host">The SMTP server hostname or IP address.</param>
/// <param name="Port">The SMTP server port (typically 465 for SSL, 587 for STARTTLS).</param>
/// <param name="UserName">The SMTP authentication username.</param>
/// <param name="Password">The SMTP authentication password.</param>
/// <param name="SecureSocketOptions">The TLS/SSL mode. Defaults to <see cref="SecureSocketOptions.Auto"/>.</param>
public sealed record SmtpSettings(
    string Host,
    int Port,
    string UserName,
    string Password,
    SecureSocketOptions SecureSocketOptions = SecureSocketOptions.Auto);
