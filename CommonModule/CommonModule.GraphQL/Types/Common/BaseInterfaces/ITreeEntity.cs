namespace CommonModule.Shared.Common.BaseInterfaces;

public interface ITreeEntity<TId, TParentId>: IBaseIdEntity<TId>
{
    TParentId ParentId { get; set; }
}