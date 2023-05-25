using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDocument : Document
{
    public AudioClip clip;
    public AudioDocument(string filename, string password, AudioClip clip) : base(filename, password)
    {
        this.clip = clip;
    }
    public AudioDocument(string filename, string password, string path) : base(filename, password)
    {
        
        this.clip = Resources.Load<AudioClip>(path);
    }
}
