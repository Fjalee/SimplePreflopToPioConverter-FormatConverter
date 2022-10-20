namespace FormatConverter.Output
{
    public class DirectoryHelper
    {
        public void CreateDirThrowIfExists(string path)
        {
            if (Directory.Exists(path))
            {
                throw new ArgumentException("Path already exists: " + path);
            }

            Directory.CreateDirectory(path);
        }
    }
}
