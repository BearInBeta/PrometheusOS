using UnityEngine;

public class NewsWebsite : MonoBehaviour
{

    public GameObject resultPrefab; // Main person button style prefab
    public int NumOfresults;        // The numaber of people in social engineering tool
    public float buttonPositionX;   // The right position of the person button
    public float buttonPositionY;   // The right position of the person button

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < NumOfresults; i++)
        {
            GameObject newButton = Instantiate(resultPrefab) as GameObject;                        // Create a new person button
            newButton.name = " Result (" + i + ")";                                                // Name the button
            newButton.transform.parent = gameObject.transform;                                     // Set the parent of the button
            newButton.transform.localScale = new Vector3(1, 1, 1);                                 // Set the scale of the button to normal scale
            newButton.transform.localPosition = new Vector3(buttonPositionX, buttonPositionY, 0); // Set the button to the right position

            buttonPositionY -= 124; // The next one position
        }
    }
}
