namespace FormatConverter.Helpers
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

        public static string CreateTxtFileThrowIfExists(string filePathNoExtesnsion, string text)
        {
            var filePath = filePathNoExtesnsion + ".txt";
            if (File.Exists(filePath))
            {
                throw new ArgumentException("File already exists: " + filePath);
            }

            using(var sw = new StreamWriter(filePath))
            {
               sw.WriteLine(text);
            }

            return filePath;
        }
    }
}
