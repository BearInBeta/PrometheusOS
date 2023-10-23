using UnityEngine;

public class ToolsOpenClose : MonoBehaviour
{
    public AudioSource SFXAS;
    public AudioClip openClip, closeClip;
    public GameObject[] Tools;

    // Open tool function : When tool opened the other tools closed
    public void openTool(int toolIndex)
    {
        SFXAS.PlayOneShot(openClip);
        closeAllTools();
        // After close all tools open the needed one :)
        Tools[toolIndex].SetActive(true);
    }

    // Close tools function
    public void closeTools()
    {
        SFXAS.PlayOneShot(closeClip);
        // Set all tools to deactive value
        closeAllTools();
    }
    public void closeAllTools()
    {
        foreach (GameObject Tool in Tools)
        {
            Tool.SetActive(false);
        }
    }

    public void toggleTool(int toolIndex)
    {
        SFXAS.PlayOneShot(openClip);
        if (Tools[toolIndex].activeInHierarchy)
        {
            Tools[toolIndex].SetActive(false);
        }
        else
        {
            Tools[toolIndex].SetActive(true);
        }
        

    }
}
