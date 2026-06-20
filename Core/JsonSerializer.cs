using System;

namespace Blanketmen.Hypnos.Serialization
{
    public class JsonSerializer : ISerializer
    {
        public byte[] Serialize<T>(T obj)
        {
            try
            {
                return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(obj);
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
                return System.Text.Json.JsonSerializer.Deserialize<T>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
