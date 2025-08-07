using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace AttitudeQuestions.Application.Shared.Extensions;

public static class EndpointConventionBuilderExtensions
{
    /// <summary>
    /// Appends the name to the name of the group. Replicates the {controller}_{action} pattern from MVC controllers, for the operation ID.
    /// </summary>
    /// <typeparam name="TBuilder"></typeparam>
    /// <param name="builder"></param>
    /// <param name="endpointName"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static TBuilder WithActionName<TBuilder>(this TBuilder builder, string endpointName) where TBuilder : IEndpointConventionBuilder
    {
        builder.Add(builder =>
        {
            var groupEndpointName = builder.Metadata.OfType<EndpointNameMetadata>().FirstOrDefault()?.EndpointName
                ?? throw new InvalidOperationException("EndpointNameMetadata not found, call RouteGroupBuilder.WithName.");

            builder.Metadata.Add(new EndpointNameMetadata($"{groupEndpointName}_{endpointName}"));
            builder.Metadata.Add(new RouteNameMetadata($"{groupEndpointName}_{endpointName}"));
        });

        return builder;
    }
}
