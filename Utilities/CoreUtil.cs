using Blanketmen.Hypnos.Compression;
using Blanketmen.Hypnos.Encryption;
using Blanketmen.Hypnos.Serialization;

namespace Blanketmen.Hypnos
{
    public enum CoreSerializer
    {
        None,
        Json,
        DotNetBinaryFormatter,
        DotNetXmlSerializer
    }

    public enum CoreCompressor
    {
        None
    }

    public enum CoreEncryptor
    {
        None
    }

    public static class CoreUtil
    {
        public static ISerializer CreateSerializer(CoreSerializer type)
        {
            switch (type)
            {
                case CoreSerializer.None:
                {
                    return null;
                }
                case CoreSerializer.Json:
                {
                    return new JsonSerializer();
                }
                case CoreSerializer.DotNetBinaryFormatter:
                {
                    return new DotNetBinaryFormatter();
                }
                case CoreSerializer.DotNetXmlSerializer:
                {
                    return new DotNetXmlSerializer();
                }
                default:
                {
                    Kernel.LogError($"[CoreUtil] Serializer not implemented. Type: {type}");
                    return null;
                }
            }
        }

        public static ICompressor CreateCompressor(CoreCompressor type)
        {
            switch (type)
            {
                case CoreCompressor.None:
                {
                    return null;
                }
                default:
                {
                    Kernel.LogError($"[CoreUtil] Compressor not implemented. Type: {type}");
                    return null;
                }
            }
        }

        public static IEncryptor CreateEncryptor(CoreEncryptor type)
        {
            switch (type)
            {
                case CoreEncryptor.None:
                {
                    return null;
                }
                default:
                {
                    Kernel.LogError($"[CoreUtil] Encryptor not implemented. Type: {type}");
                    return null;
                }
            }
        }
    }
}