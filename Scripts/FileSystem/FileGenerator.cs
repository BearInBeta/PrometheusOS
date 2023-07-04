using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileGenerator : MonoBehaviour
{
    public FileSystem fileSystem;
    [SerializeField]
    private TextAsset csvFile;
    //0-Type	1-Name	2-Path	3-Password	4-Resource	5-Extra1	6-Extra2	7-Extra3
    // Start is called before the first frame update
    void Start()
    {
    

        List<string[]> csvData = ReadCSVFile();

        // Accessing the rows
        foreach (string[] row in csvData)
        {
            if(row[0] == "Directory")
            {
                fileSystem.addDirectory(row[2], row[1], row[3]);
            }
            else if(row[0] == "Email"){
                fileSystem.addEmail(new Email(row[1], row[3], row[4], row[5],row[6]));
            }
            else if (row[0] == "TextDocument")
            {
                fileSystem.addDoc(row[2], new TextDocument(row[1], row[3], row[4], row[5]=="TRUE"));
            }
            else if (row[0] == "AudioDocument")
            {
                fileSystem.addDoc(row[2], new AudioDocument(row[1], row[3], row[4]));

            }
            else if (row[0] == "ImageDocument")
            {
                fileSystem.addDoc(row[2], new ImageDocument(row[1], row[3], row[4]));
            }
            else
            {
                Debug.Log(row[0] + " is not a type " + row[1]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<string[]> ReadCSVFile(bool skipHeaderRow = true)
    {
        List<string[]> rows = new List<string[]>();

        if (csvFile == null)
        {
            Debug.LogError("CSV file not assigned.");
            return rows;
        }

        // Split the CSV file into lines
        string[] lines = csvFile.text.Split('\n');

        // Start processing from the appropriate index
        int startIndex = skipHeaderRow ? 1 : 0;

        // Process each line
        for (int i = startIndex; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            if (string.IsNullOrEmpty(line))
                continue;

            // Split the line into fields
            string[] fields = line.Split(',');

            // Add the fields to the rows list
            rows.Add(fields);
        }

        return rows;
    }
}
