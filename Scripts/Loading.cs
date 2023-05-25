using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{

    public float LoadingTime;                                             // The time of load screen
    public Text LoadingVlaue;                                             // Loading text value
    public GameObject LoadingBar, LoadingBall, LoadingCanvas, MainCanvas; // Loader inner elements ( the full loading image and the fire ball )

    // Update is called once per frame
    void Update()
    {
        if (LoadingTime < 100) LoadingTime += Time.deltaTime * 40;
        if (LoadingTime >= 100)
        {
            LoadingTime = 100;               // Time of load from 0 to 100%
            LoadingCanvas.SetActive(false); // Hide loading canvas
            MainCanvas.SetActive(true);     // Show the main canvas ( the main game canvas )
        }

        LoadingVlaue.text = "" + (int)LoadingTime + "%";                                       // Loading text value
        LoadingBar.transform.localScale = new Vector3(LoadingTime / 100, 1, 1);                 // Transform the load effect from 0 to 100 ( loading bar effect )
        LoadingBall.transform.localPosition = new Vector3((int)LoadingTime * (float)6.2, 0, 0); // Transform the loader fire ball with the loading bar
    }
}
