using DigitalRuby.SimpleLUT;
using UnityEngine;

public class LutChanger : MonoBehaviour
{
    public SimpleLUT lut;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lut.Brightness = PlayerPrefs.GetFloat("brightness", 0);
        AudioListener.volume = PlayerPrefs.GetFloat("mastervol", 1);
        GameObject bgmusic = GameObject.FindGameObjectWithTag("BackgroundMusic");
        if (bgmusic != null)
            bgmusic.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("musicvol", 1);
    }
}
