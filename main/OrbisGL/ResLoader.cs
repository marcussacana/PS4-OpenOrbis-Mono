using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OrbisGL
{
    public static class ResLoader
    {
        static Assembly CurrentAssembly = Assembly.GetExecutingAssembly();
        public static readonly string[] ResourcesList = CurrentAssembly.GetManifestResourceNames();

        public static string GetResource(string Name)
        {
            var ResName = FindResource(Name) ?? throw new KeyNotFoundException($"Resource '{Name}' aren't available.");

            using (MemoryStream Stream = new MemoryStream())
            using (var Resource = CurrentAssembly.GetManifestResourceStream(ResName))
            {
                Resource.CopyTo(Stream);
                var Data = Stream.ToArray();
                
                if (Data.Length > 3 && Data[0] == 0xEF && Data[1] == 0xBB && Data[2] == 0xBF) 
                    Data = Data.Skip(3).ToArray();

                return Encoding.UTF8.GetString(Data);
            }
        }

        public static byte[] GetResourceData(string Name)
        {
            var ResName = FindResource(Name) ?? throw new KeyNotFoundException($"Resource '{Name}' aren't available.");

            using (MemoryStream Stream = new MemoryStream())
            using (var Resource = CurrentAssembly.GetManifestResourceStream(ResName))
            {
                Resource.CopyTo(Stream);
                var Data = Stream.ToArray();

                return Data;
            }
        }

        public static string FindResource(string Name)
        {
            var NoExtName = Path.GetFileNameWithoutExtension(Name);
            var Entries = ResourcesList.Where((x) =>
            {
                var ResName = GetResourceFileName(x);
                bool Valid = ResName.Equals(Name, StringComparison.InvariantCultureIgnoreCase);
                Valid |= NoExtName.Equals(Path.GetFileNameWithoutExtension(ResName), StringComparison.InvariantCultureIgnoreCase);
                return Valid;
            });
            

            if (!Entries.Any())
            {
                if (ResourcesList.Contains(Name))
                    return Name;
                
                return null;
            }

            return Entries.Single();
        }

        public static string GetResourceFileName(string ResourceFullName)
        {
            int ExtIndex = ResourceFullName.LastIndexOf(".");
            return ResourceFullName.Substring(0, ExtIndex).Replace(".", "/") + ResourceFullName.Substring(ExtIndex);
        }
    }
}
