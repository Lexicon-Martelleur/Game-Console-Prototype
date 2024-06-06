namespace Game.Utility;

public static class FileExtensions
{
    /// <summary>
    /// Used to create a file with a time stamp postfix if the file does not exist
    /// </summary>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="dirName">The name of the directory of the file.</param>
    /// <returns>The absolute path to the created file</returns>
    /// <exception cref="IOException">Throws IO exception if the file could not be created.</exception>
    public static string CreateFileWithTimeStampIfNotExist(this string baseFileName, string dirName)
    {
        
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileNameWithTimestamp =
            $"{Path.GetFileNameWithoutExtension(baseFileName)}_" +
            $"{timestamp}{Path.GetExtension(baseFileName)}";
        return fileNameWithTimestamp.CreateFileIfNotExist(dirName);
    }

    /// <summary>
    /// Used to create a file if the file does not exist
    /// </summary>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="dirName">The name of the directory of the file.</param>
    /// <returns>The absolute path to the created file</returns>
    /// <exception cref="IOException">Throws IO exception if the file could not be created.</exception>
    public static string CreateFileIfNotExist(this string fileName, string dirName)
    {
        try
        {
            CreateDirIfNotExist(dirName);
            string absolutePath = Path.Combine(
                Environment.CurrentDirectory,
                dirName,
                fileName);
            if (!File.Exists(absolutePath))
            {
                using (File.Create(absolutePath)) { }
            }
            return absolutePath;
        }
        catch
        {
            throw new IOException("Could not create a valid file");
        }
    }

    private static void CreateDirIfNotExist(string dirName)
    {
        string absolutePath = Path.Combine(Environment.CurrentDirectory, dirName);

        if (!Directory.Exists(absolutePath))
        {
            Directory.CreateDirectory(absolutePath);
        }
    }
}
