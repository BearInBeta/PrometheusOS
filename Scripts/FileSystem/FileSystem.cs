﻿using System;
using UnityEngine;

public class FileSystem : MonoBehaviour
{
    private FileDirectory currentDirectory;
    private FileDirectory root;
    // Start is called before the first frame update
    void Awake()
    {
        root = new FileDirectory("", "");
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
    public FileDirectory addDirectory(string path, string name, string password)
    {
        FileDirectory directory = findDirectory(path);
        if (directory != null)
        {
            return directory.addDirectory(name, password);
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
