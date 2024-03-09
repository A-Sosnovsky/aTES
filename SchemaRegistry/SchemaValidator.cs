using System.Threading.Tasks;

namespace Contract.Dto;

public static class SchemaValidator
{
    public static async Task Validate<T>(string value)
    {
        // todo тут валидируем схемы
        // var schema = await GetSchema<T>();
        // var errors = schema.Validate(value);
        // if (errors.Any())
        // {
        //     throw new Exception($"Validation failed:");
        // }
        await Task.Delay(0);
    }
}