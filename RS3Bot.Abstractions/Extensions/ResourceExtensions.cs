using Newtonsoft.Json;
using System;
using System.IO;

namespace RS3Bot.Abstractions.Extensions
{
    public static class ResourceExtensions
    {
        public static Stream GetStreamCopy(Type asmType, string name)
        {
            var assembly = asmType.Assembly;
            using (var resourceStream = assembly.GetManifestResourceStream(name))
            {
                var memoryStream = new MemoryStream();
                resourceStream.CopyTo(memoryStream);
                memoryStream.Position = 0;
                return memoryStream;
            }
        }

        public static T DeserializeResource<T>(Type asmType, string name)
        {
            var serializer = new JsonSerializer();

            using (var fishingStream = GetStreamCopy(asmType, name))
            using (var jsonStream = new StreamReader(fishingStream))
            using (var jsonTextReader = new JsonTextReader(jsonStream))
            {
                return serializer.Deserialize<T>(jsonTextReader);
            }
        }
    }
}
