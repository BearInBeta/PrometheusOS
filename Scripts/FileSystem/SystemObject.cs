public abstract class SystemObject
{
    internal FileDirectory parent;
    public string filename;
    public string password;
    public string date;
    public SystemObject(FileDirectory parent, string filename, string password, string date)
    {
        this.parent = parent;
        this.filename = filename.ToLower().Trim();
        this.password = password.ToLower().Trim();
        this.date = date;
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
