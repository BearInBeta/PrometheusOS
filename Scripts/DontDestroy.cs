using UnityEngine;


public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("BackgroundMusic").Length > 1)
            Destroy(gameObject);
        else
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
    }
}
