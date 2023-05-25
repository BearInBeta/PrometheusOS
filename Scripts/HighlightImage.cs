using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightImage : MonoBehaviour
{
    public GameObject highlightBtn;
    public int count;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < count; i++)
        {
            Instantiate(highlightBtn, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
