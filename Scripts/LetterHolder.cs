using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class LetterHolder : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
{
    public char oldLetter;

    public char newLetter;

    public bool done;
    public bool repeated;

    private void Start()
    {
    }

    private void Update()
    {
        if (done)
        {
            return;
        }
        GetComponentInChildren<Text>().text = (this.newLetter.ToString() ?? "");

        if (!GetComponent<Button>().enabled)
        {
            GetComponent<Image>().color = Color.clear;
            return;
        }
        
        if (transform.parent.GetComponent<DecryptionTool>().repeated(this.newLetter) && this.oldLetter != this.newLetter)
        {
            GetComponent<Image>().color = Color.red;
            return;
        }
        if (this.newLetter != this.oldLetter)
        {
            GetComponent<Image>().color = Color.green;
            return;
        }
        GetComponent<Image>().color = Color.white;
    }

    public void selectLetter()
    {
        if (done)
        {
            return;
        }
        if (GetComponent<Button>().enabled)
        {
            transform.parent.GetComponent<DecryptionTool>().selectLetter(this.oldLetter);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (done)
        {
            return;
        }
        if (GetComponent<Button>().enabled)
        {
            transform.parent.GetComponent<DecryptionTool>().dehighlight();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (done)
        {
            return;
        }
        if (GetComponent<Button>().enabled)
        {
            transform.parent.GetComponent<DecryptionTool>().highlight(this);
        }
    }
}
