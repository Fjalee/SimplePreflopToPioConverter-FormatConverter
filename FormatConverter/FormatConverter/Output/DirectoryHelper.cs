namespace FormatConverter.Output
{
    public static class DirectoryHelper
    {
        public static string CreateDirThrowIfExists(string path)
        {
            if (Directory.Exists(path))
            {
                throw new ArgumentException("Path already exists: " + path);
            }

            Directory.CreateDirectory(path);

            return path;
        }
    }
}
