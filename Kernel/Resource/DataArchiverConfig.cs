using Blanketmen.Hypnos.Serialization;
using Blanketmen.Hypnos.Compression;
using Blanketmen.Hypnos.Encryption;
using System;

namespace Blanketmen.Hypnos
{
    [Serializable]
    public class DataArchiverConfig
    {
        public int id;
        public ISerializer serializer;
        public ICompressor compressor;
        public IEncryptor encryptor;
    }
}