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
        print("Notes updated to " + newNotes);
    }
}
