using UnityEngine;

public class Email : Document
{
    public string text;
    public bool read = false;
    public string from;
    public string to = "det.<name>@promail.com";
    public Email(string filename, string password, string text, string from, string to) : base(filename, password)
    {
        this.text = Resources.Load<TextAsset>(text).text;
        this.from = from;
        this.to = to;

    }
}
