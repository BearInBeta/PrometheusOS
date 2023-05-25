using UnityEngine;
using UnityEngine.UI;


public class LoginAnalysis : MonoBehaviour
{
    public InputField usertext;

    public InputField passtxt;

    public GameObject loader;

    public GameObject profile;

    public GameObject wrong;

    public GameObject info;

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void analye()
    {
        if (this.usertext.text.ToLower().Equals("lucio.bruti@promail.com") && this.passtxt.text.Equals("luciobruno1987"))
        {
            this.loader.SetActive(true);
            this.profile.SetActive(true);
            this.info.SetActive(true);
            this.wrong.SetActive(false);
            return;
        }
        this.wrong.SetActive(true);
    }
}
