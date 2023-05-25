using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip clip;
    public AudioSource audioSource;
    public Slider slider;
    public Text text;
    public Text endText;
    public Slider endSlider;
    public Text beginText;
    public Slider beginSlider;
    public Text fullPath;
    public bool isPlaying;

    // Start is called before the first frame update
    void Start()
    {
        addClip(null, "No Audio File");
    }

    string minSec(float seconds)
    {
        int remSecs = (int)(seconds % 60);
        int mins = (int)(seconds / 60);
        string remSecsS = "" + remSecs;
        string minsS = "" + mins;

        if(remSecs < 10)
            remSecsS = "0" + remSecs;

        if (mins < 10)
            minsS = "0" + mins;
        return minsS + ":" + remSecsS;
    }

    // Update is called once per frame
    void Update()
    {
        if (clip != null)
        {
            if (audioSource.isPlaying)
            {
                slider.value = audioSource.time;

            }
            if (beginSlider.value > (clip.length - endSlider.value))
            {
                stopAudio();
                endSlider.value = clip.length - beginSlider.value;
            }
            if (slider.value > (clip.length - endSlider.value))
            {
                stopAudio();
                slider.value = clip.length - endSlider.value;
            }

            if (slider.value < (beginSlider.value - 0.01))
            {
                stopAudio();
            }
        
        beginText.text = minSec(beginSlider.value);
        endText.text = minSec(clip.length - endSlider.value);
        text.text = minSec(slider.value);
        }
        else
        {
            beginText.text = minSec(0);
            endText.text = minSec(0);
            text.text = minSec(0);
        }
    }

    public void addClip(AudioClip clip, string path)
    {
        fullPath.text = path;
        if (clip == null)
        {
            slider.maxValue = 0;
            slider.minValue = 0;
            slider.value = 0;
            beginSlider.maxValue = 0;
            beginSlider.minValue = 0;
            beginSlider.value = 0;
            endSlider.maxValue = 0;
            endSlider.minValue = 0;
            endSlider.value = 0;
            return;
        }
        this.clip = clip;
        slider.maxValue = clip.length;
        slider.minValue = 0;
        slider.value = 0;
        beginSlider.maxValue = clip.length;
        beginSlider.minValue = 0;
        beginSlider.value = 0;
        endSlider.maxValue = clip.length;
        endSlider.minValue = 0;
        endSlider.value = 0;
        audioSource.clip = clip;
        
        playAudio();
    }

    public void pauseAudio()
    {
        audioSource.Pause();
    }

    public void playAudio()
    {
        if (clip == null) return;
        isPlaying = true;
        if (slider.value < clip.length)
        { 
            audioSource.time = slider.value;
            audioSource.Play();
        }
        else
            stopAudio();
        
    }

    public void stopAudio()
    {
        isPlaying = false;
        slider.value = beginSlider.value;
        audioSource.time = slider.value;
        audioSource.Stop();
    }
}
