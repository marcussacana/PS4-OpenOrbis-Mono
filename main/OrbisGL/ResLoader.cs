﻿using System;
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
        static string[] CurrentResources = CurrentAssembly.GetManifestResourceNames();

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
            var Entries = CurrentResources.Where((x) =>
            {
                int ExtIndex = x.LastIndexOf(".");
                x = x.Substring(0, ExtIndex).Replace(".", "/") + x.Substring(ExtIndex);
                bool Valid = x.Equals(Name, StringComparison.InvariantCultureIgnoreCase);
                Valid |= NoExtName.Equals(Path.GetFileNameWithoutExtension(x), StringComparison.InvariantCultureIgnoreCase);
                return Valid;
            });

            if (!Entries.Any())
            {
                return null;
            }

            return Entries.Single();
        }
    }
}
