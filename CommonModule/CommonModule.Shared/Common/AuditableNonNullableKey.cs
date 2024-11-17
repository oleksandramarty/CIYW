using CommonModule.Shared.Common.BaseInterfaces;

namespace CommonModule.Shared.Common;

public class AuditableNonNullableKey : IAuditableNonNullableKey
{
    private string key = string.Empty;

    public string? Key
    {
        get
        {
            if (string.IsNullOrEmpty(this.key))
            {
                // TODO Audit Trail warning empty key
            }
            else
            {
                // TODO Audit Trail warning empty instance name
            }

            return this.key;
        }
        set
        {
            if (string.IsNullOrEmpty(this.key))
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.key = value;
                }
                else
                {
                    // TODO Audit Trail warning empty key
                }
            }
        }
    }
}