using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextClipboard
{
    public static void CopyToClipboard(string s)
    {
        TextEditor te = new TextEditor();
        te.text = s;
        te.SelectAll();
        te.Copy();

    }
}
