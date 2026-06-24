public class PollyMailKitExtensionsTests
{
    private static readonly SmtpClient _client = new();
    private static readonly ResiliencePipeline _pipeline = new ResiliencePipelineBuilder().Build();

    [Fact]
    public void WithPolly_Pipeline_ReturnsResilientSmtpClient()
    {
        var resilient = _client.WithPolly(_pipeline);
        Assert.NotNull(resilient);
        Assert.Same(_client, resilient.Inner);
    }

    [Fact]
    public void WithPolly_Configure_ReturnsResilientSmtpClient()
    {
        var resilient = _client.WithPolly(p => p.AddRetry(new RetryStrategyOptions
        {
            MaxRetryAttempts = 3, Delay = TimeSpan.Zero,
            ShouldHandle = MailKitTransientErrors.IsTransient,
        }));
        Assert.NotNull(resilient);
        Assert.Same(_client, resilient.Inner);
    }
}
