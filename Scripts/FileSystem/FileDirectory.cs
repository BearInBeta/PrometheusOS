using System.Collections.Generic;

public class FileDirectory : SystemObject
{
    private List<SystemObject> children = new List<SystemObject>();
    public FileDirectory(string filename, string password) : base(null, filename, password)
    {

    }
    public FileDirectory addDirectory(string filename, string password)
    {
        return (FileDirectory)addChild(new FileDirectory(filename, password));
    }
    public Document addDoc(Document doc)
    {
        return (Document)addChild(doc);
    }

    public SystemObject addChild(SystemObject systemObject)
    {
        string filename = systemObject.filename;
        int count = 1;
        while (findChild(filename) != null)
        {
            filename = systemObject.filename + "(" + count + ")";
            count++;
        }

        systemObject.filename = filename;
        systemObject.parent = this;
        children.Add(systemObject);
        return systemObject;
    }
    public SystemObject findChild(string filename)
    {
        foreach (SystemObject child in children)
        {
            if (child.filename.ToUpper().Equals(filename.ToUpper()))
            {
                return child;
            }
        }
        return null;
    }

    public void deleteChild(string filename)
    {
        SystemObject child = findChild(filename);
        if (child != null)
        {
            children.Remove(child);
        }
    }

    public List<SystemObject> getChildren()
    {
        return children;

    }
}
