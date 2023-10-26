using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TextHighlight : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public TextMeshProUGUI text;
    public Camera maincamera;
    public string fulltext;
    public string marker = "<mark=#ffff00aa>";
    string selectedText;


    private void Start()
    {
        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C) && Input.GetKey(KeyCode.LeftControl))
        {
            TextClipboard.CopyToClipboard(selectedText);
            print("copied");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        fulltext = text.text;
        int charIndex = TMP_TextUtilities.GetCursorIndexFromPosition(text, eventData.pressPosition, maincamera);
        if (charIndex != -1)
        {
            int startingChar = charIndex;
            if(charIndex < fulltext.Length)
            text.text = fulltext.Substring(0, charIndex) + marker + fulltext[charIndex] + "</mark>" + fulltext.Substring(charIndex + 1);
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        int charIndex = TMP_TextUtilities.GetCursorIndexFromPosition(text, eventData.position, maincamera);
        int startingChar = TMP_TextUtilities.GetCursorIndexFromPosition(text, eventData.pressPosition, maincamera);

        if (charIndex >= 0)
        {
            int length = charIndex - startingChar;
            if (length >= 0)
            {
                if(startingChar <= fulltext.Length && charIndex < fulltext.Length)
                text.text = fulltext.Substring(0, startingChar) + marker + fulltext.Substring(startingChar, length + 1) + "</mark>" + fulltext.Substring(charIndex + 1);
            }
            else
            {
                length = -length;

                 text.text = fulltext.Substring(0, charIndex) + marker + fulltext.Substring(charIndex, length) + "</mark>" + fulltext.Substring(startingChar);




            }

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        int charIndex = TMP_TextUtilities.GetCursorIndexFromPosition(text, eventData.position, maincamera);
        int startingChar = TMP_TextUtilities.GetCursorIndexFromPosition(text, eventData.pressPosition, maincamera);

        if (charIndex != -1)
        {
            int length = charIndex - startingChar;
            if (length >= 0)
            {
                if (startingChar + length < fulltext.Length)
                    selectedText = fulltext.Substring(startingChar, length + 1);
            }
            else
            {
                length = -length;

                if (charIndex + length < fulltext.Length)
                    selectedText = fulltext.Substring(charIndex, length);




            }

        }
    }
}
