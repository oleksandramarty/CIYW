namespace CommonModule.Shared.Common.BaseInterfaces;

public interface ITreeEntityEntity<TId, TParentId>: IBaseIdEntity<TId>
{
    TParentId ParentId { get; set; }
}