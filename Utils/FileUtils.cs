namespace Utils;

public static class FileUtils
{
    public static string GetRootFolder(string currentDirectory, string solutionFileName)
    {
        var directory = new DirectoryInfo(currentDirectory);

        while (directory != null)
        {
            string[] solutionFiles = Directory.GetFiles(directory.FullName, solutionFileName);
            if (solutionFiles.Length > 0)
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException($"Could not find the root folder containing the '{solutionFileName}' file.");
    }

    public static string GetMimeType(string extension)
    {
        return extension.ToLower().Replace(".", "") switch
        {
            "jpg" => "image/jpeg",
            "png" => "image/png",
            "gif" => "image/gif",

            _ => "application/octet-stream"
        };
    }
}
