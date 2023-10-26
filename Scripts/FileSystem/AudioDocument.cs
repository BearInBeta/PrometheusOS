using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDocument : Document
{
    public AudioClip clip;
    public AudioDocument(string filename, string password, AudioClip clip, string date) : base(filename, password, date)
    {
        this.clip = clip;
    }
    public AudioDocument(string filename, string password, string path, string date) : base(filename, password, date)
    {
        
        this.clip = Resources.Load<AudioClip>(path);
    }
}
