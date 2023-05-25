using UnityEngine;

public class SocialWebsite : MonoBehaviour
{

    public GameObject personPrefab; // Main person button style prefab
    public int NumOfPersons;        // The numaber of people in social engineering tool
    public float buttonPositionX;   // The right position of the person button
    public float buttonPositionY;   // The right position of the person button

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < NumOfPersons; i++)
        {
            GameObject newButton = Instantiate(personPrefab) as GameObject;                        // Create a new person button
            newButton.name = " Person (" + i + ")";                                                // Name the button
            newButton.transform.parent = gameObject.transform;                                     // Set the parent of the button
            newButton.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);                    // Set the scale of the button to normal scale
            newButton.transform.localPosition = new Vector3(buttonPositionX, buttonPositionY, 0); // Set the button to the right position

            buttonPositionY -= 62; // The next one position
        }
    }
}
