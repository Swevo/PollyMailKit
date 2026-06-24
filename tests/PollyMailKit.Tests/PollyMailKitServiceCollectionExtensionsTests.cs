public class PollyMailKitServiceCollectionExtensionsTests
{
    private static readonly SmtpSettings _settings =
        new("smtp.example.com", 587, "user@example.com", "password");

    [Fact]
    public void AddPollyMailKit_RegistersResiliencePipeline()
    {
        var services = new ServiceCollection();
        services.AddPollyMailKit(_settings, p => { });
        Assert.NotNull(services.BuildServiceProvider().GetRequiredService<ResiliencePipeline>());
    }

    [Fact]
    public void AddPollyMailKit_RegistersSmtpSettings()
    {
        var services = new ServiceCollection();
        services.AddPollyMailKit(_settings, p => { });
        var resolved = services.BuildServiceProvider().GetRequiredService<SmtpSettings>();
        Assert.Equal("smtp.example.com", resolved.Host);
    }

    [Fact]
    public void AddPollyMailKit_RegistersResilientSmtpSender()
    {
        var services = new ServiceCollection();
        services.AddPollyMailKit(_settings, p => { });
        Assert.NotNull(services.BuildServiceProvider().GetRequiredService<ResilientSmtpSender>());
    }

    [Fact]
    public void AddPollyMailKit_ReturnsServiceCollection()
    {
        var services = new ServiceCollection();
        Assert.Same(services, services.AddPollyMailKit(_settings, p => { }));
    }

    [Fact]
    public void SmtpSettings_HasCorrectValues()
    {
        Assert.Equal("smtp.example.com", _settings.Host);
        Assert.Equal(587, _settings.Port);
        Assert.Equal("user@example.com", _settings.UserName);
        Assert.Equal(SecureSocketOptions.Auto, _settings.SecureSocketOptions);
    }
}
