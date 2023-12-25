using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace MyTemplates;

public class MyTemplates
{
    public static string GetFarewellMessage(string name)
    {
        return $"Здравствуйте, {name}! Вы уволены".Replace("@{name}", name);
    }

    public static string GetPackageMessage(object model)
    {
        Type modelType = model.GetType();

        PropertyInfo fullNameProp = modelType.GetProperty("FullName");
        PropertyInfo addressProp = modelType.GetProperty("Address");

        if (fullNameProp != null && addressProp != null)
        {
            string fullName = fullNameProp.GetValue(model)?.ToString();
            string address = addressProp.GetValue(model)?.ToString();

            if (fullName != null && address != null)
            {
                return $"Здравствуйте, {fullName} проживающий по адресу {address}, Ваша посылка дошла в пункт выдачи";
            }
        }

        return "Неверный тип модели или отсутствуют необходимые свойства";
    }
    public static string GetVideoMessage(object model)
    {
        var template = "Здравствуйте, @{if(Age >= 18)} @then{Вам стали доступны home video} @else{ваша ссылка на Смешариков: https://www.smeshariki.ru}";

        var regex = new Regex(@"@{if\(([^}]*)\)} @then{([^}]*)} @else{([^}]*)}");

        var result = regex.Replace(template, match =>
        {
            var condition = match.Groups[1].Value.Trim();
            var thenValue = match.Groups[2].Value.Trim();
            var elseValue = match.Groups[3].Value.Trim();

            var isConditionTrue = EvaluateCondition(condition, model);

            return isConditionTrue ? thenValue : elseValue;
        });

        return result;
    }

    private static bool EvaluateCondition(string condition, object model)
    {
        try
        {
            var conditionRegex = new Regex(@"([^<>=!]+)\s*([<>=!]+)\s*(.+)");
            var match = conditionRegex.Match(condition);

            if (match.Success)
            {
                var propertyName = match.Groups[1].Value.Trim();
                var operatorString = match.Groups[2].Value.Trim();
                var valueString = match.Groups[3].Value.Trim();

                var propertyValue = model.GetType().GetProperty(propertyName)?.GetValue(model);

                switch (operatorString)
                {
                    case "==":
                        return object.Equals(propertyValue, Convert.ChangeType(valueString, propertyValue.GetType()));
                    case "!=":
                        return !object.Equals(propertyValue, Convert.ChangeType(valueString, propertyValue.GetType()));
                    case ">":
                        return Comparer.Default.Compare(propertyValue, Convert.ChangeType(valueString, propertyValue.GetType())) > 0;
                    case "<":
                        return Comparer.Default.Compare(propertyValue, Convert.ChangeType(valueString, propertyValue.GetType())) < 0;
                    case ">=":
                        return Comparer.Default.Compare(propertyValue, Convert.ChangeType(valueString, propertyValue.GetType())) >= 0;
                    case "<=":
                        return Comparer.Default.Compare(propertyValue, Convert.ChangeType(valueString, propertyValue.GetType())) <= 0;
                    default:
                        throw new InvalidOperationException($"Invalid operator: {operatorString}");
                }
            }

            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public static string GetGroupMessage(object model)
    {
        var template = "Здравствуйте, @{Fullname}! Ваше название предмета @{Subject}. Номер группы @{Group}.";

        var result = template;

        foreach (var property in model.GetType().GetProperties())
        {
            var placeholder = "@{" + property.Name + "}";
            var value = property.GetValue(model)?.ToString() ?? string.Empty;

            result = result.Replace(placeholder, value);
        }

        var students = model.GetType().GetProperty("Students")?.GetValue(model) as IEnumerable<object>;
        if (students != null)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Список группы:");

            foreach (var student in students)
            {
                var surname = student.GetType().GetProperty("Surname")?.GetValue(student);
                var name = student.GetType().GetProperty("Name")?.GetValue(student);

                if (surname != null && name != null)
                {
                    sb.AppendLine($"-{surname} {name}");
                }
            }
            result = $"{result} {sb}";
        }

        return result;
    }
}