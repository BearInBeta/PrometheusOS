using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider brightnessSlider;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        SetGraphics();
        SetVolume();
    }

    private void SetGraphics()
    {

        if (!PlayerPrefs.HasKey("brightness"))
        {
            PlayerPrefs.SetFloat("brightness", 0);

        }
        brightnessSlider.value = PlayerPrefs.GetFloat("brightness") + 0.5f;
    }

    private void SetVolume()
    {
        if (!PlayerPrefs.HasKey("mastervol"))
        {
            PlayerPrefs.SetFloat("mastervol", 1);

        }
        masterVolumeSlider.value = PlayerPrefs.GetFloat("mastervol");

        if(!PlayerPrefs.HasKey("musicvol"))
        {
            PlayerPrefs.SetFloat("musicvol", 1);

        }
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicvol");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeBrightness()
    {
        PlayerPrefs.SetFloat("brightness", brightnessSlider.value - 0.5f);
    }
    public void AdjustVolume()
    {
        PlayerPrefs.SetFloat("mastervol", masterVolumeSlider.value);
        
    }

    public void AdjustMusicVolume()
    {
        PlayerPrefs.SetFloat("musicvol", musicVolumeSlider.value);
        
    }
}
