using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDocument : Document
{
    public Sprite image;
    public ImageDocument(string filename, string password, Sprite image) : base(filename, password)
    {
        this.image = image;
    }
    public ImageDocument(string filename, string password, string path) : base(filename, password)
    {
        this.image = Resources.Load<Sprite>(path); ;
    }
}
