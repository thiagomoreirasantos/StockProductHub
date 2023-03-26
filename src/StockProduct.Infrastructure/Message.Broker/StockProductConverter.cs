using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StockProduct.Application.Dtos;

namespace StockProduct.Infrastructure.Message.Broker
{
    public class StockProductConverter : JsonCreationConverter<StockProductData>
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        protected override StockProductData Create(Type objectType, JObject jObject)
        {
            return new StockProductData();
        }
    }
}