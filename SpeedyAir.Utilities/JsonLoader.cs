using System.IO;

namespace SpeedyAir.Utilities
{
    // Utility to load JSON files
    public static class JsonLoader
    {
        public static string LoadJson(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
