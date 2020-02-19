using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;

namespace BE.Helpers
{
    public static class DynamicLinqStatement
    {
        public static IDictionary<string, object> ExtractSpecifiedFields<T>(T obj,
            string[] selectedFields)
        {
            IDictionary<string, object> userExpando = new ExpandoObject();
            foreach (var selectedField in selectedFields)
            {
                var sentenceCaseSelectedField = CultureInfo
                    .CurrentCulture
                    .TextInfo
                    .ToTitleCase(selectedField.ToLower());
                var containsField =
                    obj.GetType().GetProperty(sentenceCaseSelectedField) != null;
                if (containsField)
                {
                    var value = typeof(T).GetProperty(sentenceCaseSelectedField)
                        .GetValue(obj);
                    userExpando.Add(sentenceCaseSelectedField, value);
                }
            }

            return userExpando;
        }
    }
}