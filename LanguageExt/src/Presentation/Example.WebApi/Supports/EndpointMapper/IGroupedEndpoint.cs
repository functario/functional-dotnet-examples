namespace Example.WebApi.Supports.EndpointMapper;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Justification = "<Pending>"
)]
public interface IGroupedEndpoint<TGroup> : IEndpoint
    where TGroup : IGroup { }
