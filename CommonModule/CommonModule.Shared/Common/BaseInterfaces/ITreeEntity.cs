namespace CommonModule.Shared.Common.BaseInterfaces;

public interface ITreeEntity<TId, TParentId>
{
    TId Id { get; set; }
    TParentId ParentId { get; set; }
}