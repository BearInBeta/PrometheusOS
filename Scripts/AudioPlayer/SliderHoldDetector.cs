using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliderHoldDetector : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public AudioPlayer audioPlayer;
    public void OnBeginDrag(PointerEventData eventData)
    {
        audioPlayer.pauseAudio();
    }



    public void OnEndDrag(PointerEventData eventData)
    {
        if(audioPlayer.isPlaying)
        audioPlayer.playAudio();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
