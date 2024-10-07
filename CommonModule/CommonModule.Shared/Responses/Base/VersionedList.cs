using CommonModule.Shared.Common.BaseInterfaces;

namespace CommonModule.Shared.Responses.Base;

public class VersionedList<TResponse>: BaseVersionEntity
{
    public List<TResponse> Items { get; set; } = new List<TResponse>();
}