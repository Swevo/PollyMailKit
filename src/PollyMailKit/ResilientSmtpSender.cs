/// <summary>
/// Sends email via SMTP using a fresh connection per attempt — fully retry-safe.
/// Wraps the entire connect → authenticate → send → disconnect lifecycle in a
/// Polly v8 <see cref="ResiliencePipeline"/>.
/// </summary>
public sealed class ResilientSmtpSender(SmtpSettings settings, ResiliencePipeline pipeline)
{
    /// <summary>
    /// Sends a <see cref="MimeMessage"/> through the resilience pipeline.
    /// A new <see cref="SmtpClient"/> connection is created for each attempt,
    /// making this approach safe to retry on transient failures.
    /// </summary>
    public Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default)
        => pipeline.ExecuteAsync(async ct =>
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(settings.Host, settings.Port, settings.SecureSocketOptions, ct);
            await client.AuthenticateAsync(settings.UserName, settings.Password, ct);
            await client.SendAsync(message, ct);
            await client.DisconnectAsync(true, ct);
        }, cancellationToken).AsTask();
}
