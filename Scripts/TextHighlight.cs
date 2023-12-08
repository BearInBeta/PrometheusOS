using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TextHighlight : MonoBehaviour
{
    private TMP_Text textMeshPro;
    private int highlightStartIndex = -1;
    private int highlightEndIndex = -1;

    private void Start()
    {
        // Get the TextMeshPro component
        textMeshPro = GetComponent<TMP_Text>();

        // Enable raycasting for the TextMeshPro component
        textMeshPro.raycastTarget = true;
    }

    private void Update()
    {
        // Check for mouse input
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            // Check if the ray hits the TextMeshPro component
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Get the index of the clicked character
                int clickedIndex = TMP_TextUtilities.FindIntersectingCharacter(textMeshPro, Input.mousePosition, Camera.main, true);

                if (clickedIndex != -1)
                {
                    // Set the start index of the highlight
                    highlightStartIndex = clickedIndex;
                    highlightEndIndex = clickedIndex;
                }
            }
        }

        // Check for mouse drag
        if (Input.GetMouseButton(0) && highlightStartIndex != -1)
        {
            // Update the end index of the highlight
            highlightEndIndex = TMP_TextUtilities.FindIntersectingCharacter(textMeshPro, Input.mousePosition, Camera.main, true);
        }

        // Check for mouse release
        if (Input.GetMouseButtonUp(0))
        {
            // Copy the highlighted text to the clipboard
            if (highlightStartIndex != -1 && highlightEndIndex != -1)
            {
                string highlightedText = textMeshPro.text.Substring(Mathf.Min(highlightStartIndex, highlightEndIndex),
                                                                    Mathf.Abs(highlightEndIndex - highlightStartIndex));
                GUIUtility.systemCopyBuffer = highlightedText;

                // Clear the highlight
                highlightStartIndex = -1;
                highlightEndIndex = -1;
            }
        }

        // Highlight the text
        HighlightText();
    }

    private void HighlightText()
    {
        if (highlightStartIndex != -1 && highlightEndIndex != -1)
        {
            // Set the color of the highlighted characters
            for (int i = 0; i < textMeshPro.textInfo.characterCount; i++)
            {
                bool isHighlighted = i >= Mathf.Min(highlightStartIndex, highlightEndIndex) &&
                                      i <= Mathf.Max(highlightStartIndex, highlightEndIndex);
                Color highlightColor = isHighlighted ? Color.yellow : Color.white;

                textMeshPro.textInfo.characterInfo[i].color = highlightColor;
            }

            // Update the TextMeshPro mesh
            textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }
    }
}
