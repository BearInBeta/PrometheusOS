using UnityEngine;
using UnityEngine.UI;

public class SliderValueChanger : MonoBehaviour
{
    public Text ValueText;

    public Slider SliderValue;

    // Email qualty slider : Update the qualty text depending on slider value
    void Update() { ValueText.text = "" + (int)(SliderValue.value * 100.0f) + "%"; }
}
