namespace CommonModule.Interfaces;

public interface ITreeEntity<TId, TParentId>
{
    TId Id { get; set; }
    TParentId ParentId { get; set; }
}