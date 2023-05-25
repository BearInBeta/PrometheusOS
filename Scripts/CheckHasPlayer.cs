using UnityEngine;
using UnityEngine.UI;


public class CheckHasPlayer : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("HasPlayer", 0) == 1)
        {
            base.GetComponent<Toggle>().interactable = true;
            return;
        }
        base.GetComponent<Toggle>().interactable = false;
    }

    private void Update()
    {
    }
}
