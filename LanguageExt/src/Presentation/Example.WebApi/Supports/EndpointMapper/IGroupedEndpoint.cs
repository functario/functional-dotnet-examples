namespace Example.WebApi.Supports.EndpointMapper;

internal interface IGroupedEndpoint<TGroup> : IEndpoint
    where TGroup : IGroup
{ }
