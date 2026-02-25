using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, IRequest<TResponse> where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handle request = {Request} with data = {Data} and response = {Response}", typeof(TRequest).Name, request, typeof(TResponse).Name);
        Stopwatch timer = new();
        timer.Start();
        TResponse response = await next(cancellationToken);
        timer.Stop();
        TimeSpan time = timer.Elapsed;
        logger.LogInformation("[END] Handle request = {Request} with time = {Time} milliseconds and response = {Response}", typeof(TRequest).Name, time.Milliseconds, typeof(TResponse).Name);
        return response;
    }
}