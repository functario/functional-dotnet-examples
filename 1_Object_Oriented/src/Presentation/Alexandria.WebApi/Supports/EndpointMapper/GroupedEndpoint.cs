using System.Collections.Frozen;

namespace Alexandria.WebApi.Supports.EndpointMapper;

internal sealed record GroupedEndpoints(Dictionary<Type, FrozenSet<Type>> KeyValuePairs) { }
