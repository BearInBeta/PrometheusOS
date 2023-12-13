using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesUpdater : MonoBehaviour
{
    public PersonHolder ph;
    // Start is called before the first frame update
    public void updateNotes()
    {
        string newNotes = GetComponent<InputField>().text;
        ph.notes = GetComponent<InputField>().text;
        PlayerPrefs.SetString(ph.fsa.filesystem.ACName + "notes", ph.notes);
        print("Notes updated to " + PlayerPrefs.GetString(ph.fsa.filesystem.ACName + "notes"));
    }
}
