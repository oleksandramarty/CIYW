namespace CommonModule.Shared.Common.BaseInterfaces;

public interface IBaseIdEntity<T>
{
    T Id { get; set; }
}