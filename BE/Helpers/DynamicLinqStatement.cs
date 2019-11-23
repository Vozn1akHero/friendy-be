using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using BE.Models;

namespace BE.Helpers
{
    public static class DynamicLinqStatement
    {
        public static IDictionary<string, object> ExtractSpecifiedFields<T>(T obj, string[] selectedFields)
        {
            IDictionary<string, object> userExpando = new ExpandoObject();
            foreach (var selectedField in selectedFields)
            {
                string sentenceCaseSelectedField = CultureInfo
                    .CurrentCulture
                    .TextInfo
                    .ToTitleCase(selectedField.ToLower());
                bool containsField = obj.GetType().GetProperty(sentenceCaseSelectedField) != null;
                if (containsField)
                {
                    object value = typeof(T).GetProperty(sentenceCaseSelectedField)
                        .GetValue(obj);
                    userExpando.Add(sentenceCaseSelectedField, value);
                }
            }
            return userExpando;
        }
    }
}