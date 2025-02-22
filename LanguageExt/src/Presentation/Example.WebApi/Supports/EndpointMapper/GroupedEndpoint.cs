using System.Collections.Frozen;

namespace Example.WebApi.Supports.EndpointMapper;

internal sealed record GroupedEndpoints(Dictionary<Type, FrozenSet<Type>> KeyValuePairs) { }
