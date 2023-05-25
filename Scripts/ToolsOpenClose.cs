using UnityEngine;

public class ToolsOpenClose : MonoBehaviour
{

    public GameObject[] Tools;

    // Open tool function : When tool opened the other tools closed
    public void openTool(int toolIndex)
    {
        closeTools();
        // After close all tools open the needed one :)
        Tools[toolIndex].SetActive(true);
    }

    // Close tools function
    public void closeTools()
    {
        // Set all tools to deactive value
        foreach (GameObject Tool in Tools)
        {
            Tool.SetActive(false);
        }
    }

    public void toggleTool(int toolIndex)
    {
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
