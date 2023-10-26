using System;
using System.Collections.Generic;
using UnityEngine;

public class FileSystem : MonoBehaviour
{
    private FileDirectory currentDirectory;
    private FileDirectory root;
    private List<Email> emails;
    public string personName, ACName, password;
    // Start is called before the first frame update
    void Awake()
    {
        emails = new List<Email>();
        root = new FileDirectory("", "", "");
        currentDirectory = root;

    }

    public string getCurrentDirectoryPath()
    {
        return currentDirectory.getPath();
    }

    public string listChildren()
    {
        string childrenList = "";
        foreach (SystemObject child in currentDirectory.getChildren())
        {
            if (child.GetType().Equals(typeof(FileDirectory)))
            {
                childrenList += " " + "<color=#00c7eb>" + child.filename + "</color>";

            }
            else
            {
                childrenList += " " + child.filename;

            }
        }
        return childrenList;
    }
    public string listChildren(string path)
    {
        FileDirectory targetDirectory = findDirectory(path);
        string childrenList = "";
        if (targetDirectory != null)
        {
            foreach (SystemObject child in targetDirectory.getChildren())
            {
                if (child.GetType().Equals(typeof(FileDirectory)))
                {
                    childrenList += " " + "<color=#00c7eb>" + child.filename + "</color>";

                }
                else
                {
                    childrenList += " " + child.filename;

                }
            }
            return childrenList;
        }
        else
        {
            return "<color=#8c002a>Directory not found</color>";
        }
    }
    public FileDirectory addDirectory(string path, string name, string password, string date)
    {
        FileDirectory directory = findDirectory(path);
        if (directory != null)
        {
            return directory.addDirectory(name, password, date);
        }

        return null;
    }

    public Document addDoc(string path, Document doc)
    {
        FileDirectory directory = findDirectory(path);
        if (directory != null)
        {
            return directory.addDoc(doc);

        }
        print("directory " + path + " not found");
        return null;
    }

    public void addEmail(Email email)
    {
        emails.Add(email);
    }

    public string listEmails()
    {
        string childrenList = "";
        int counter = 0;
            foreach (Email child in emails)
            {
            counter++;
                if (child.read == false)
                {
                    childrenList +=  "<color=#00c7eb>" + counter + ". " + child.filename + " - " + child.from + "</color>\n" ;

                }
                else
                {
                    childrenList += counter + " " + child.filename + " - " + child.from;
                

                }
            }
            return childrenList;
        
    }

    public Email gotoemail(string subject)
    {
        foreach(Email email in emails)
        {
            if (email.filename == subject)
                return email;
        }
        return null;
    }
    public Email gotoemail(int order)
    {
        if(order > emails.Count)
        return null;

        return emails[order - 1];
    }
    public bool changeDirectory(string path)
    {
        FileDirectory newDirectory = findDirectory(path);
        if (newDirectory != null)
        {
            currentDirectory = newDirectory;
            return true;
        }

        return false;
    }


    public FileDirectory findDirectory(string path)
    {
        SystemObject systemObject = findSystemObject(path);
        if (systemObject != null && systemObject.GetType().Equals(typeof(FileDirectory)))
        {
            return (FileDirectory)systemObject;
        }

        return null;
    }

    public Document findDoc(string path)
    {
        SystemObject systemObject = findSystemObject(path);
        if (systemObject != null && systemObject.GetType().IsSubclassOf(typeof(Document)))
        {
            return (Document)systemObject;
        }

        return null;
    }

    public SystemObject findSystemObject(string path)
    {

        SystemObject searchDirectory;
        string[] pathElements = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

        if (path.StartsWith("/"))
        {
            searchDirectory = root;
        }
        else
        {
            searchDirectory = currentDirectory;
        }

        foreach (string pathElement in pathElements)
        {
            SystemObject nextDirectory = ((FileDirectory)searchDirectory).findChild(pathElement);
            if (pathElement.Equals("."))
            {
                nextDirectory = (FileDirectory)searchDirectory;
            }
            else if (pathElement.Equals(".."))
            {
                if (searchDirectory.parent != null)
                    nextDirectory = (FileDirectory)searchDirectory.parent;
                else
                    nextDirectory = (FileDirectory)searchDirectory;
            }


            if (nextDirectory != null && (nextDirectory.GetType().Equals(typeof(FileDirectory)) || pathElement == pathElements[pathElements.Length - 1]))
            {
                searchDirectory = nextDirectory;
            }
            else
            {

                return null;
            }
        }

        return searchDirectory;
    }
    public string searchfilesystem(string query, bool startFromRoot = true)
    {
        string result = "";
        FileDirectory startingDirectory = startFromRoot ? root : currentDirectory;

        SearchDirectory(startingDirectory, query, ref result);
        int counter = 0;
        foreach (Email email in emails)
        {
            counter++;
            if (email.filename.Contains(query, StringComparison.OrdinalIgnoreCase) || email.text.Contains(query, StringComparison.OrdinalIgnoreCase) || email.from.Contains(query, StringComparison.OrdinalIgnoreCase) || email.to.Contains(query, StringComparison.OrdinalIgnoreCase))
            {
                result +=  email.GetType().Name + " - " + counter + ". " + email.getPath() + " - " + email.from + "\n";
            }
        }
        return result;
    }

    private void SearchDirectory(FileDirectory directory, string query, ref string result)
    {
        foreach (SystemObject child in directory.getChildren())
        {
            
            if (child is FileDirectory fileDir)
            {
                if (fileDir.filename.Contains(query, StringComparison.OrdinalIgnoreCase) || fileDir.date.Equals(query, StringComparison.OrdinalIgnoreCase))
                {
                    result += fileDir.GetType().Name + " - " + fileDir.getPath() + "\n";
                }
                if (string.IsNullOrEmpty(fileDir.password))
                {
                    SearchDirectory(fileDir, query, ref result);
                }
            }
            else if (child is Document document)
            {
                if (document.filename.Contains(query, StringComparison.OrdinalIgnoreCase) || document.date.Equals(query, StringComparison.OrdinalIgnoreCase))
                {
                    result += document.GetType().Name + " - " + document.getPath() + "\n";
                }
                else if (document is TextDocument textDoc && string.IsNullOrEmpty(textDoc.password))
                {
                    
                    if (textDoc.text.Contains(query, StringComparison.OrdinalIgnoreCase))
                    {
                        result += textDoc.GetType().Name + " - " + textDoc.getPath() + "\n";
                    }
                }
            }
        }
    }
    public string findParentPassword(string path)
    {

        SystemObject searchDirectory;
        string[] pathElements = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

        if (path.StartsWith("/"))
        {
            searchDirectory = root;
        }
        else
        {
            searchDirectory = currentDirectory;
        }

        for (int i = 0; i < pathElements.Length - 1; i++)
        {
            string pathElement = pathElements[i];
            SystemObject nextDirectory = ((FileDirectory)searchDirectory).findChild(pathElement);
            if (pathElement.Equals("."))
            {
                nextDirectory = (FileDirectory)searchDirectory;
            }
            else if (pathElement.Equals(".."))
            {
                if (searchDirectory.parent != null)
                    nextDirectory = (FileDirectory)searchDirectory.parent;
                else
                    nextDirectory = (FileDirectory)searchDirectory;
            }
            if (nextDirectory == null)
            {
                return null;
            }
            if (nextDirectory.password != "")
            {
                return nextDirectory.getPath();
            }
            if ((nextDirectory.GetType().Equals(typeof(FileDirectory)) || pathElement == pathElements[pathElements.Length - 1]))
            {
                searchDirectory = nextDirectory;
            }
            else
            {
                return null;
            }
        }

        return null;
    }
}
