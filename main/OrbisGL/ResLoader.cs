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
            return GetResource(CurrentAssembly, Name);
        }
        public static string GetResource(Assembly Target, string Name)
        {
            var ResName = FindResource(Target, Name) ?? throw new KeyNotFoundException($"Resource '{Name}' aren't available.");

            using (MemoryStream Stream = new MemoryStream())
            using (var Resource = Target.GetManifestResourceStream(ResName))
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
            return GetResourceData(CurrentAssembly, Name);
        }

        public static byte[] GetResourceData(Assembly Target, string Name)
        {
            var ResName = FindResource(Target, Name) ?? throw new KeyNotFoundException($"Resource '{Name}' aren't available.");

            using (MemoryStream Stream = new MemoryStream())
            using (var Resource = Target.GetManifestResourceStream(ResName))
            {
                Resource.CopyTo(Stream);
                var Data = Stream.ToArray();

                return Data;
            }
        }

        public static string FindResource(Assembly Target, string Name)
        {
            return FindResource(Target.GetManifestResourceNames(), Name);
        }
        public static string FindResource(string Name)
        {
            return FindResource(ResourcesList, Name);
        }
        public static string FindResource(string[] List, string Name)
        {
            var NoExtName = Path.GetFileNameWithoutExtension(Name);
            var Entries = List.Where((x) =>
            {
                var ResName = GetResourceFileName(x);
                bool Valid = ResName.Equals(Name, StringComparison.InvariantCultureIgnoreCase);
                Valid |= NoExtName.Equals(Path.GetFileNameWithoutExtension(ResName), StringComparison.InvariantCultureIgnoreCase);
                return Valid;
            });
            

            if (!Entries.Any())
            {
                if (List.Contains(Name))
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
