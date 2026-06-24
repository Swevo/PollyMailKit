public class MailKitTransientErrorsTests
{
    [Fact]
    public void IsTransient_IsNotNull()
        => Assert.NotNull(MailKitTransientErrors.IsTransient);

    [Fact]
    public async Task IsTransient_RetriesSocketException()
    {
        var pipeline = BuildRetryPipeline();
        var attempts = 0;
        await Assert.ThrowsAsync<SocketException>(() =>
            pipeline.ExecuteAsync(ct => { attempts++; throw new SocketException(); }).AsTask());
        Assert.Equal(2, attempts);
    }

    [Fact]
    public async Task IsTransient_RetriesIOException()
    {
        var pipeline = BuildRetryPipeline();
        var attempts = 0;
        await Assert.ThrowsAsync<IOException>(() =>
            pipeline.ExecuteAsync(ct => { attempts++; throw new IOException("stream error"); }).AsTask());
        Assert.Equal(2, attempts);
    }

    [Fact]
    public async Task IsTransient_RetriesServiceNotConnectedException()
    {
        var pipeline = BuildRetryPipeline();
        var attempts = 0;
        await Assert.ThrowsAsync<ServiceNotConnectedException>(() =>
            pipeline.ExecuteAsync(ct => { attempts++; throw new ServiceNotConnectedException(); }).AsTask());
        Assert.Equal(2, attempts);
    }

    [Fact]
    public async Task IsTransient_RetriesOperationCanceledException()
    {
        var pipeline = BuildRetryPipeline();
        var attempts = 0;
        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            pipeline.ExecuteAsync(ct => { attempts++; throw new OperationCanceledException("timeout"); }).AsTask());
        Assert.Equal(2, attempts);
    }

    private static ResiliencePipeline BuildRetryPipeline() =>
        new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions { MaxRetryAttempts = 1, Delay = TimeSpan.Zero, ShouldHandle = MailKitTransientErrors.IsTransient })
            .Build();
}
