/// <summary>
/// Wraps an existing <see cref="SmtpClient"/> with a Polly v8 <see cref="ResiliencePipeline"/>.
/// Use this when you manage your own connection lifecycle.
/// For a fully retry-safe approach that creates a new connection per attempt, use <see cref="ResilientSmtpSender"/>.
/// </summary>
public sealed class ResilientSmtpClient(SmtpClient client, ResiliencePipeline pipeline)
{
    /// <summary>The underlying <see cref="SmtpClient"/>.</summary>
    public SmtpClient Inner => client;

    /// <summary>Sends a <see cref="MimeMessage"/>, protected by the resilience pipeline.</summary>
    public Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default)
        => pipeline.ExecuteAsync(
            async ct => await client.SendAsync(message, ct),
            cancellationToken).AsTask();

    /// <summary>
    /// Executes any <see cref="SmtpClient"/> operation, protected by the resilience pipeline.
    /// </summary>
    public Task<T> ExecuteAsync<T>(
        Func<SmtpClient, CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken = default)
        => pipeline.ExecuteAsync(
            async ct => await operation(client, ct),
            cancellationToken).AsTask();
}
