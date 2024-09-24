using CommonModule.Shared.Common.BaseInterfaces;

namespace CommonModule.Shared.Common;

public class BaseIdEntity<T>: IBaseIdEntity<T>
{
    public T Id { get; set; }
}