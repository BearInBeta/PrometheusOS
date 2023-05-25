public abstract class SystemObject
{
    internal FileDirectory parent;
    public string filename;
    public string password;
    public SystemObject(FileDirectory parent, string filename, string password)
    {
        this.parent = parent;
        this.filename = filename.ToLower().Trim();
        this.password = password.ToLower().Trim();
    }

    public string getPath()
    {
        string path = filename;
        FileDirectory nextDirectory = parent;
        while (nextDirectory != null)
        {
            path = nextDirectory.filename + "/" + path;

            nextDirectory = nextDirectory.parent;
        }

        return path;
    }


}
