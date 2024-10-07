namespace CommonModule.Shared.Common.BaseInterfaces;

public interface IBaseDateTimeEntity<TId> : IBaseIdEntity<TId>
{
    DateTime Created { get; set; }
    DateTime? Modified { get; set; }
}