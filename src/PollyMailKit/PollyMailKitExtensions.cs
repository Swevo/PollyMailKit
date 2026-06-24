/// <summary>Extension methods for adding Polly resilience to MailKit SMTP clients.</summary>
public static class PollyMailKitExtensions
{
    /// <summary>Wraps a <see cref="SmtpClient"/> with the given <see cref="ResiliencePipeline"/>.</summary>
    public static ResilientSmtpClient WithPolly(
        this SmtpClient client,
        ResiliencePipeline pipeline)
        => new(client, pipeline);

    /// <summary>Wraps a <see cref="SmtpClient"/> with a pipeline built by <paramref name="configure"/>.</summary>
    public static ResilientSmtpClient WithPolly(
        this SmtpClient client,
        Action<ResiliencePipelineBuilder> configure)
    {
        var builder = new ResiliencePipelineBuilder();
        configure(builder);
        return new(client, builder.Build());
    }
}
