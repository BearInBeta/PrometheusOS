using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProFileViewer : MonoBehaviour
{
    public GameObject proFileGO;
    public Image image;
    public Text personname, email;
    public ConnectDirect cd;
    public InputField notes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openProFile(PersonHolder ph)
    {
        image.sprite = ph.image.sprite;
        personname.text = ph.personname.text;
        email.text = ph.email.text;
        cd.fsa = ph.fsa;
        notes.gameObject.GetComponent<NotesUpdater>().ph = ph;
        if (ph.notes.Equals("") && PlayerPrefs.HasKey(ph.fsa.filesystem.ACName + "notes"))
        {
            ph.notes = PlayerPrefs.GetString(ph.fsa.filesystem.ACName + "notes");
        }
        notes.text = ph.notes;

        proFileGO.SetActive(true);
    }
}
