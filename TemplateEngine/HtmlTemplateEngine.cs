using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using TemplateEngine;

namespace TemplateEngine;

public class HtmlTemplateEngine : IHtmlTemplateEngine
{
    public string Render(string template, string data)
    {
        return template.Replace("{{Name}}", data);
    }

    public string Render(FileInfo fileInfo, object obj)
    {
        var templatePath = fileInfo.FullName;
        if (File.Exists(templatePath))
        {
            return Render(template: File.ReadAllText(templatePath), obj);
        }
        else
        {
            Console.WriteLine($"File {templatePath} not found");
            throw new FileNotFoundException($"File {templatePath} not found"); // я думаю эти ошибки надо отлавливать там где мы будем вызывать Render
        }
    }

    public string Render(Stream stream, object obj)
    {
        if (stream.CanRead)
        {
            var content = new StreamReader(stream).ReadToEnd();
            return Render(content, obj);
        }
        else
        {
            throw new InvalidOperationException("Stream can't be read");
        }
    }

    public string Render(string template, object obj)
    {
        // Идет по темплейту, вытаскивает все требуемые переменные, если видит if разбивает его на части, по телу if так же проходится
        // анализирует его
        // потом берет данные из модельки obj и распихивает их по запрашиваемым переменным
        // возвращает результат собранный из данных модельки
        var properties = obj.GetType().GetProperties();
        var result = template;
        try
        {
            result = HandleLoops(result, properties, obj);
            result = HandleConditionals(result, properties, obj);
            result = HandleVariables(result, obj);
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine(e.Message);
            return e.Message;
        }
        return result;
    }

    public string HandleVariables(string template, object obj)
    {
        if (obj is null) throw new ArgumentNullException("obj is null");

        var result = template;
        var regex = new Regex(@"{{(.*?)}}");

        result = regex.Replace(result, match =>
        {
            var propertyPath = match.Groups[1].Value;
            var value = GetNestedPropertyValue(obj, propertyPath);
            return value ?? match.Value;
        });

        return result;
    }

    private string GetNestedPropertyValue(object obj, string propertyPath)
    {
        if (obj == null || string.IsNullOrWhiteSpace(propertyPath)) return null;

        var currentObject = obj;
        var properties = propertyPath.Split('.');
        foreach (var property in properties)
        {
            if (currentObject == null) return null;

            var type = currentObject.GetType();
            var propertyInfo = type.GetProperty(property, BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null) return null;

            currentObject = propertyInfo.GetValue(currentObject);
        }

        return currentObject?.ToString();
    }

    private string HandleConditionals(string template, PropertyInfo[] properties, object model)
    {
        if (model is null) throw new ArgumentNullException("model is null");
        var pattern = @"{%if<(.*?)>%}(.*?)(?:{%else%}(.*?))?{%/if%}";
        var regex = new Regex(pattern, RegexOptions.Singleline);
        return regex.Replace(template, match =>
        {
            var conditional = match.Groups[1].Value.Trim();
            var content = match.Groups[2].Value.Trim();
            var elseContent = match.Groups[3].Success ? match.Groups[4].Value.Trim() : null;

            if (ProcessCondition(conditional, properties, model))
            {
                return Render(content, model);
            }
            if (elseContent != null)
            {
                return Render(elseContent, model);
            }
            return string.Empty;
        });
    }

    private bool ProcessCondition(string condition, PropertyInfo[] properties, object model)
    {
        if (model is null) throw new ArgumentNullException("model is null");
        if (properties is null || properties.Length == 0) throw new ArgumentNullException("model properties are null");
        foreach (var property in properties)
        {
            var value = property.GetValue(model) is not bool ?
                $"'{property.GetValue(model)}'" : property.GetValue(model).ToString();
            var pattern = $@"{{{{{property.Name}}}}}";
            condition = Regex.Replace(condition, pattern, value, RegexOptions.IgnoreCase);
        }

        condition = condition.Replace("==", "=")
            .Replace("&&", "AND")
            .Replace("||", "OR")
            .Replace("!=", "<>");
        try
        {
            var result = (bool)new System.Data.DataTable().Compute(condition, string.Empty);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    private string HandleLoops(string template, PropertyInfo[] properties, object model)
    {
        var pattern = @"{%for%(.+?)%in%(.+?)}(.*?){%/for%}";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        return regex.Replace(template, match =>
        {
            var itemName = match.Groups[1].Value.Trim(); 
            var collectionName = match.Groups[2].Value.Trim(); 
            var content = match.Groups[3].Value;

            // Найти коллекцию в свойствах модели
            var collectionProperty = properties.FirstOrDefault(p => p.Name.Equals(collectionName, StringComparison.OrdinalIgnoreCase));
            if (collectionProperty == null)
            {
                throw new ArgumentException($"Collection '{collectionName}' not found in model.");
            }

            // Получить значение коллекции
            var collection = collectionProperty.GetValue(model) as IEnumerable;
            if (collection == null)
            {
                throw new ArgumentException($"Property '{collectionName}' is not a valid collection.");
            }

            var loopResult = new StringBuilder();

            foreach (var value in collection)
            {
                if (value == null) continue;

                string renderedItem;

                if (value.GetType().IsPrimitive || value is string)
                {
                    // Для простых типов (например, string) заменяем {{Book}} на значение
                    renderedItem = content.Replace($"{{{{{itemName}}}}}", value.ToString());
                }
                else
                {
                    // Для объектов заменяем {{Book.Property}} на значения их свойств
                    var itemTemplate = content.Replace("{{" + itemName + ".", "{{");
                    renderedItem = Render(itemTemplate, value);
                }

                loopResult.Append(renderedItem);
            }

            return loopResult.ToString();
        });
    }

    private bool CheckStringInLoop(string content, string check)
    {
        if (content.IndexOf("{%for%") == -1) return false;
        return content.IndexOf("{%for%") < content.IndexOf(check) &&
               content.IndexOf("{%/for%}") > content.IndexOf(check);
    }

    private bool CheckStringInCond(string content, string check)
    {
        if (content.IndexOf("{%if") == -1) return false;
        return content.IndexOf("{%if") < content.IndexOf(check) &&
               content.IndexOf("{%/if%}") > content.IndexOf(check);
    }

    void exampleException()
    {
        try
        {
            throw new InvalidOperationException("denisiki pobedili");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {

        }
    }
}
