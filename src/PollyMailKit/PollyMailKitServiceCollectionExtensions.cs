/// <summary>Dependency-injection extensions for <c>PollyMailKit</c>.</summary>
public static class PollyMailKitServiceCollectionExtensions
{
    /// <summary>
    /// Registers a singleton <see cref="ResiliencePipeline"/> and a transient
    /// <see cref="ResilientSmtpSender"/> using the provided <paramref name="settings"/>.
    /// </summary>
    public static IServiceCollection AddPollyMailKit(
        this IServiceCollection services,
        SmtpSettings settings,
        Action<ResiliencePipelineBuilder> configure)
    {
        var builder = new ResiliencePipelineBuilder();
        configure(builder);
        var pipeline = builder.Build();

        services.AddSingleton(pipeline);
        services.AddSingleton(settings);
        services.AddTransient(_ => new ResilientSmtpSender(settings, pipeline));

        return services;
    }
}
