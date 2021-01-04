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
    }
}
