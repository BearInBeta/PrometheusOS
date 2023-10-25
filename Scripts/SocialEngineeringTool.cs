using System.Collections.Generic;
using UnityEngine;

public class SocialEngineeringTool : MonoBehaviour
{

    public GameObject personPrefab; // Main person button style prefab
    public int NumOfPersons;        // The numaber of people in social engineering tool
    public float buttonPositionY;   // The right position of the person button
    public List<GameObject> buttons;
    public List<FileSystemAccess> systems;
    public FileSystemAccess playerSystem;
    // Use this for initialization
    void Start()
    {
        addFSA(playerSystem);
        buttons = new List<GameObject>();
        //createButtons();
    }
    public void addFSA(FileSystemAccess fsa)
    {
        if (!systems.Contains(fsa))
        {
            systems.Add(fsa);
            createButton(fsa);
            //createButtons();
        }
    }
    void createButtons()
    {
        foreach(GameObject oldButton in buttons)
        {
            Destroy(oldButton);
        }
        buttons.Clear();
        for (int i = 0; i < systems.Count; i++)
        {
            createButton(systems[i]);
        }
    }
    void createButton(FileSystemAccess fsa)
    {
        GameObject newButton = Instantiate(personPrefab) as GameObject;            // Create a new person button
        PersonHolder ph = newButton.GetComponent<PersonHolder>();
        ph.personname.text = fsa.filesystem.personName;
        ph.email.text = fsa.filesystem.ACName;
        ph.fsa = fsa;
        newButton.transform.parent = gameObject.transform;                         // Set the parent of the button
        newButton.transform.localScale = new Vector3(1, 1, 1);                    // Set the scale of the button to normal scale
        buttons.Add(newButton);
    }
}
