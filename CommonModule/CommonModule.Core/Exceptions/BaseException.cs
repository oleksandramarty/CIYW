using CommonModule.Core.Exceptions.Errors;

namespace CommonModule.Core.Exceptions;

public class BaseException: Exception
{
    public BaseException(
        string message,
        int _statusCode,
        IReadOnlyCollection<InvalidFieldInfoModel> invalidFields = null) :
        base(message)
    {
        statusCode = _statusCode;
    }

    public int statusCode { get; set; }
    public IReadOnlyCollection<InvalidFieldInfoModel> invalidFields { get; set; }
    
    public ErrorMessageModel ToErrorMessage()
    {
        return new ErrorMessageModel(Message, statusCode, invalidFields);
    }
}