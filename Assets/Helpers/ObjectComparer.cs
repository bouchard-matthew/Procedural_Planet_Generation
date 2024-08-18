using System.Reflection;

public static class ObjectComparer
{
    public static bool AreEqual<T>(T obj1, T obj2)
    {
        if (obj1 == null || obj2 == null)
            return false;

        var type = typeof(T);
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var value1 = property.GetValue(obj1);
            var value2 = property.GetValue(obj2);
            if (!object.Equals(value1, value2))
                return false;
        }

        foreach (var field in fields)
        {
            var value1 = field.GetValue(obj1);
            var value2 = field.GetValue(obj2);
            if (!object.Equals(value1, value2))
                return false;
        }

        return true;
    }
}
