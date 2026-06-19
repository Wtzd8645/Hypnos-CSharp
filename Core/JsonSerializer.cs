using System;
using Newtonsoft.Json;

namespace Blanketmen.Hypnos.Serialization
{
    public class JsonSerializer : ISerializer
    {
        public byte[] Serialize<T>(T obj)
        {
            try
            {
                string json = JsonConvert.SerializeObject(obj);
                return System.Text.Encoding.UTF8.GetBytes(json);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public T Deserialize<T>(byte[] data)
        {
            try
            {
                string json = System.Text.Encoding.UTF8.GetString(data);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
