# PollyMailKit

[![NuGet](https://img.shields.io/nuget/v/PollyMailKit.svg)](https://www.nuget.org/packages/PollyMailKit/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/PollyMailKit.svg)](https://www.nuget.org/packages/PollyMailKit/)
[![CI](https://github.com/Swevo/PollyMailKit/actions/workflows/build.yml/badge.svg)](https://github.com/Swevo/PollyMailKit/actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

**Polly v8 resilience pipelines for MailKit SMTP** — retry, timeout, and circuit-breaker for sending email via SmtpClient.

## Why PollyMailKit?

SMTP is inherently flaky. Connections drop, servers throttle, and ServiceNotConnectedException is thrown mid-send. PollyMailKit wraps MailKit's SmtpClient in a Polly v8 ResiliencePipeline so you get automatic retry, exponential back-off, and circuit-breaking with zero boilerplate.

**Two wrapper styles:**
- ResilientSmtpSender — creates a **fresh connection per attempt** (fully retry-safe; recommended for transient failures)
- ResilientSmtpClient — wraps an **existing connected** SmtpClient (preferred when the connection lifecycle is managed externally)

## Installation

`
dotnet add package PollyMailKit
`

## Quick start

### Option A — ResilientSmtpSender (fresh connection per attempt)

`csharp
// Configure in DI
services.AddPollyMailKit(
    new SmtpSettings("smtp.example.com", 587, "user@example.com", "password"),
    pipeline => pipeline
        .AddRetry(new RetryStrategyOptions
        {
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromSeconds(1),
            BackoffType = DelayBackoffType.Exponential,
            ShouldHandle = MailKitTransientErrors.IsTransient,
        })
        .AddTimeout(TimeSpan.FromSeconds(30)));

// Use via DI
public class EmailService(ResilientSmtpSender sender)
{
    public async Task SendAsync(MimeMessage message, CancellationToken ct = default)
        => await sender.SendAsync(message, ct);
}
`

### Option B — WithPolly() extension on existing SmtpClient

`csharp
var smtpClient = new SmtpClient();
await smtpClient.ConnectAsync("smtp.example.com", 587, SecureSocketOptions.Auto);
await smtpClient.AuthenticateAsync("user", "password");

var resilient = smtpClient.WithPolly(pipeline =>
    pipeline.AddRetry(new RetryStrategyOptions
    {
        MaxRetryAttempts = 3,
        Delay = TimeSpan.FromSeconds(1),
        BackoffType = DelayBackoffType.Exponential,
        ShouldHandle = MailKitTransientErrors.IsTransient,
    }));

await resilient.SendAsync(message);
`

## SmtpSettings

`csharp
public record SmtpSettings(
    string Host,
    int Port,
    string UserName,
    string Password,
    SecureSocketOptions SecureSocketOptions = SecureSocketOptions.Auto);
`

## Transient error predicate

MailKitTransientErrors.IsTransient handles:

| Exception | Reason |
|---|---|
| SocketException | Network-level failure |
| IOException | Stream read/write error |
| ServiceNotConnectedException | SMTP server disconnected mid-session |
| OperationCanceledException | Timeout or cancellation |

## Supported frameworks


et6.0 · 
et8.0 · 
et9.0

## Related packages

| Package | Wraps |
|---|---|
| [PollyEFCore](https://github.com/Swevo/PollyEFCore) | Entity Framework Core DbContext |
| [PollyDapper](https://github.com/Swevo/PollyDapper) | Dapper IDbConnection |
| [PollyMongo](https://github.com/Swevo/PollyMongo) | MongoDB IMongoCollection<T> |
| [PollyAzureBlob](https://github.com/Swevo/PollyAzureBlob) | Azure Blob Storage BlobContainerClient |
| [PollyNpgsql](https://github.com/Swevo/PollyNpgsql) | Npgsql PostgreSQL NpgsqlConnection |
| [PollySqlClient](https://github.com/Swevo/PollySqlClient) | System.Data.SqlClient SqlConnection |
| [PollyCosmosDb](https://github.com/Swevo/PollyCosmosDb) | Azure Cosmos DB CosmosClient |
| [PollyGrpc](https://github.com/Swevo/PollyGrpc) | gRPC channel calls |
| [PollyRabbitMQ](https://github.com/Swevo/PollyRabbitMQ) | RabbitMQ IModel channel |
| [PollyAzureServiceBus](https://github.com/Swevo/PollyAzureServiceBus) | Azure Service Bus sender/receiver |
| [PollyRedis](https://github.com/Swevo/PollyRedis) | StackExchange.Redis IDatabase |
| [PollyMediatR](https://github.com/Swevo/PollyMediatR) | MediatR IMediator |
| [PollyOpenAI](https://github.com/Swevo/PollyOpenAI) | OpenAI ChatClient |
| [PollyHealthChecks](https://github.com/Swevo/PollyHealthChecks) | ASP.NET Core health checks |
| [PollyBackoff](https://github.com/Swevo/PollyBackoff) | Pre-built backoff pipelines |
| [PollyChaos](https://github.com/Swevo/PollyChaos) | Chaos engineering helpers |
| [PollyKafka](https://github.com/Swevo/PollyKafka) | Confluent Kafka producer/consumer |
| [PollySignalR](https://github.com/Swevo/PollySignalR) | SignalR HubConnection |
| [PollyRateLimiter](https://github.com/Swevo/PollyRateLimiter) | .NET rate limiting middleware |
| [PollyElasticsearch](https://github.com/Swevo/PollyElasticsearch) | Elastic.Clients.Elasticsearch |
| [PollyAzureKeyVault](https://github.com/Swevo/PollyAzureKeyVault) | Azure Key Vault SecretClient |
| [PollyAzureEventHub](https://github.com/Swevo/PollyAzureEventHub) | Azure Event Hubs producer |
| [PollySendGrid](https://github.com/Swevo/PollySendGrid) | SendGrid email client |
| [PollyMassTransit](https://github.com/Swevo/PollyMassTransit) | MassTransit IBus |
| [PollyAzureTableStorage](https://github.com/Swevo/PollyAzureTableStorage) | Azure Table Storage TableClient |
| [PollyAzureQueueStorage](https://github.com/Swevo/PollyAzureQueueStorage) | Azure Queue Storage QueueClient |
| [PollyHangfire](https://github.com/Swevo/PollyHangfire) | Hangfire IBackgroundJobClient |

## 💼 Need .NET consulting?

The author of this package is available for consulting on **Polly v8 resilience**, **Azure cloud architecture**, and **clean .NET design**.

**[→ solidqualitysolutions.com](https://www.solidqualitysolutions.com/)** · **[LinkedIn](https://www.linkedin.com/in/justbannister/)**
## License

MIT © Justin Bannister