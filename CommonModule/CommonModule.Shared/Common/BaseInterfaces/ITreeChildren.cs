using CommonModule.Shared.Responses.Base;

namespace CommonModule.Shared.Common.BaseInterfaces;

public interface ITreeChildren<TResponse>
{
    ICollection<TreeNodeResponse<TResponse>> Children { get; set; }
}