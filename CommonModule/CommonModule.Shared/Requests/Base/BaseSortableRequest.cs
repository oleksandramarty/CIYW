namespace CommonModule.Shared.Requests.Base;

public class BaseSortableRequest
{
    public BaseSortableRequest()
    {
        
    }
    public BaseSortableRequest(string column, string direction)
    {
        Column = column;
        Direction = direction;
    }

    public string Column { get; set; }
    public string Direction { get; set; }
}