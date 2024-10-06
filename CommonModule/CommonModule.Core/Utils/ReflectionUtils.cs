namespace CommonModule.Core.JsonConverter;

public class ReflectionUtils
{
    public static TProperty GetValue<T, TProperty>(T entity, string field)
    {
        var property = typeof(T).GetProperty(field);
        return (TProperty)property.GetValue(entity);
    }
    
    public static void SetValue<T, TProperty>(T entity, string field, TProperty value)
    {
        var property = typeof(T).GetProperty(field);
    
        if (property != null)
        {
            property.SetValue(entity, value);
        }
    }
}