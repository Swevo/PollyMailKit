/// <summary>
/// Pre-built Polly <see cref="PredicateBuilder"/> for transient MailKit SMTP errors.
/// Covers socket failures, I/O stream errors, lost connections, and operation timeouts.
/// </summary>
public static class MailKitTransientErrors
{
    /// <summary>
    /// A <see cref="PredicateBuilder"/> that handles:
    /// <list type="bullet">
    ///   <item><see cref="SocketException"/> — network connectivity failure</item>
    ///   <item><see cref="IOException"/> — underlying stream read/write error</item>
    ///   <item><see cref="ServiceNotConnectedException"/> — connection lost mid-operation</item>
    ///   <item><see cref="OperationCanceledException"/> — timeout or cancellation during transit</item>
    /// </list>
    /// Assign to <c>ShouldHandle</c> on any Polly strategy.
    /// </summary>
    public static readonly PredicateBuilder IsTransient =
        (PredicateBuilder)new PredicateBuilder()
            .Handle<SocketException>()
            .Handle<IOException>()
            .Handle<ServiceNotConnectedException>()
            .Handle<OperationCanceledException>();
}
