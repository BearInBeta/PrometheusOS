using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonHolder : MonoBehaviour
{
    public Image image;
    public Text personname, email;
    public FileSystemAccess fsa;
    public string notes;

    public void openProFile()
    {
        GameObject.FindGameObjectWithTag("Codes").GetComponent<ProFileViewer>().openProFile(this);
    }
   

}
