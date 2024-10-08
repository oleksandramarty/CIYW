namespace CommonModule.Shared.Responses.Base;

public class TreeNodeResponse<TNode>
{
    public TNode Node { get; set; }
    
    public TNode? Parent { get; set; }
}