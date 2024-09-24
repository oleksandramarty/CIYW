using System;
using System.IO;
using CommonModule.Core.Exceptions;

namespace CommonModule.Facade
{
    public static class VersionGenerator
    {
        private static readonly string version;
        private static readonly string versionFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "version.txt");

        static VersionGenerator()
        {
            if (!File.Exists(versionFilePath))
            {
                throw new VersionException();
            }
            
            version = File.ReadAllText(versionFilePath);
        }
        
        public static string GetVersion()
        {
            return version;
        }
    }
}