using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDocument : Document
{
    public Sprite image;
    public ImageDocument(string filename, string password, Sprite image, string date) : base(filename, password, date)
    {
        this.image = image;
    }
    public ImageDocument(string filename, string password, string path, string date) : base(filename, password, date)
    {
        this.image = Resources.Load<Sprite>(path); ;
    }
}
