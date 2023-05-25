using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DecryptionTool : MonoBehaviour
{

    public GameObject buttonPrefab;
    public GameObject emptyButton;
    public string MainText;

    public GameObject instruction;
    public char[] Letters;

    public float buttonPositionX;

    public float buttonPositionY;

    public float differenceX;

    public float differenceY;
    public int lettersInLine;

    public Sprite selectedSprite;

    public Sprite normalSprite;

    public Sprite highlightedSprite;

    public Sprite wordSprite;
    private char[] scrambledAlpha;

    public List<LetterHolder> selectedHolders;

    public List<LetterHolder> letterHolders;
    public ToolsOpenClose toolsOpenClose;
    private TextDocument document;
    private char[] alpha = new char[]
    {
'A',
'B',
'C',
'D',
'E',
'F',
'G',
'H',
'I',
'J',
'K',
'L',
'M',
'N',
'O',
'P',
'Q',
'R',
'S',
'T',
'U',
'V',
'W',
'X',
'Y',
'Z'
    };

    public List<char> selectedLetters;

    public bool allIsCorrect;

    private bool done;
    public Terminal terminal;

    public bool repeated(char letter)
    {
        int num = 0;
        using (List<char>.Enumerator enumerator = selectedLetters.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                if (enumerator.Current == letter)
                {
                    num++;
                }
                if (num >= 2)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void Start()
    {
        //createText();
    }

    private int mod(int x, int m)
    {
        int num = x % m;
        if (num >= 0)
        {
            return num;
        }
        return num + m;

    }

    private void Update()
    {
        float y = Input.mouseScrollDelta.y;
        if (y != 0f && selectedHolders.Count >= 1 && !allIsCorrect && !done)
        {
            int num = 0;
            for (int i = 0; i < alpha.Length; i++)
            {
                if (alpha[i] == selectedHolders[0].newLetter)
                {
                    num = i;
                    break;
                }
            }
            if (selectedHolders[0].newLetter != selectedHolders[0].oldLetter)
            {
                selectedLetters.Remove(selectedHolders[0].newLetter);
            }
            num = (int)((float)num - Mathf.Abs(y) / y);
            char c = alpha[mod(num, 26)];
            using (List<LetterHolder>.Enumerator enumerator = selectedHolders.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current.newLetter = c;
                }
            }
            if (c != selectedHolders[0].oldLetter)
            {
                selectedLetters.Add(c);
            }
            bool flag = true;
            foreach (LetterHolder current in letterHolders)
            {
                if (searchArray(current.oldLetter, scrambledAlpha) != searchArray(current.newLetter, alpha))
                {
                    flag = false;
                    break;
                }
            }
            allIsCorrect = flag;
            return;
        }
        else if (allIsCorrect && !done)
        {
            done = true;
            document.encrypted = false;
            foreach (LetterHolder current in letterHolders)
            {

                current.done = true;
                current.gameObject.GetComponent<Image>().sprite = highlightedSprite;
                current.gameObject.GetComponent<Image>().color = Color.cyan;
            }
            terminal.OpenTextDoc(document);
            
        }


    }

    private int searchArray(char letter, char[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == letter)
            {
                return i;
            }
        }
        return -1;

    }

    public void splitStringToArray(string Phrase)
    {
        Phrase = Phrase.ToUpper();
        Letters = Phrase.ToCharArray();
    }

    public void scrambler()
    {

        for (int j = 0; j < Letters.Length; j++)
        {
            for (int k = 0; k < alpha.Length; k++)
            {
                if (Letters[j] == alpha[k])
                {
                    Letters[j] = scrambledAlpha[k];
                    break;
                }
            }
        }

    }
    public string scrambleText(TextDocument document)
    {
        this.document = document;
        MainText = document.text;
        scrambledAlpha = document.scrambledAlpha;
        splitStringToArray(MainText);
        scrambler();
        string s = "";
        foreach (char c in Letters)
        {
            s += c;
        }
        return s;
    }
    public void createText(TextDocument document)
    {
        
        allIsCorrect = false;
        done = false;
        scrambleText(document);
        instruction.SetActive(false);
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        selectedLetters.RemoveRange(0, selectedLetters.Count);
        selectedHolders.Clear();
        letterHolders.Clear();

        float csize = 24000f / Letters.Length;
        if (csize > 100)
            csize = 100;

        if (csize < 30)
            csize = 30;
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(csize, csize);
        int letterCounter = 0;
        for (int k = 0; k < Letters.Length; k++)
        {
            
                GameObject gameObject = Instantiate(buttonPrefab);
                gameObject.name = " LetterButton (" + k + ")";
                gameObject.GetComponent<LetterHolder>().newLetter = Letters[k];
                gameObject.GetComponent<LetterHolder>().oldLetter = Letters[k];
                letterHolders.Add(gameObject.GetComponent<LetterHolder>());
                gameObject.gameObject.transform.SetParent(transform);
                gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                //gameObject.transform.localPosition = new Vector3(num, num2, 0f);
                if (Letters[k] == ' ' || Letters[k] == '\n')
            {
                    gameObject.GetComponent<Image>().enabled = false;
                }
                if (Letters[k] == '@' || Letters[k] == ',' || Letters[k] == '.' || Letters[k] == '?' || Letters[k] == ':' || Letters[k] == '!' || Letters[k] == '/')
                {
                    gameObject.GetComponent<Button>().enabled = false;
                }
            print(Letters[k]);
            
            letterCounter++;
        }

    }

    public void ResetTool()
    {
        foreach (LetterHolder expr_15 in letterHolders)
        {
            expr_15.newLetter = expr_15.oldLetter;
        }

    }

    public void selectLetter(char l)
    {
        selectedHolders.Clear();
        foreach (LetterHolder current in letterHolders)
        {
            if (current.oldLetter.Equals(l))
            {
                current.gameObject.GetComponent<Image>().sprite = selectedSprite;
                selectedHolders.Add(current);
            }
            else
            {
                current.gameObject.GetComponent<Image>().sprite = normalSprite;
            }
        }

    }

    public void highlight(LetterHolder l)
    {
        int firstletter = 0;
        int lastletter = 0;

        for (int i = 0; i < letterHolders.Count; i++)
        {

            if (letterHolders[i] == l)
            {
                for(int j = i; j >= 0; j--)
                {

                    if (letterHolders[j].oldLetter == ' ' || letterHolders[j].oldLetter == ',' || letterHolders[j].oldLetter == '\n')
                    {

                        break;
                    }
                    firstletter = j;
                }

                for (int j = i; j < letterHolders.Count; j++)
                {

                    if (letterHolders[j].oldLetter == ' ' || letterHolders[j].oldLetter == ',' || letterHolders[j].oldLetter == '\n')
                    {

                        break;
                    }
                    lastletter = j;
                }
            }
        }
        for (int i = 0; i < letterHolders.Count; i++)
        {
            LetterHolder current = letterHolders[i];
            if (current.oldLetter == l.oldLetter)
            {
                if (letterHolders[i] != l)
                {
                    current.gameObject.GetComponent<Image>().sprite = highlightedSprite;
                }
                else
                {
                    current.gameObject.GetComponent<Image>().sprite = selectedSprite;

                }
            }
            else if (i <= lastletter && i >= firstletter)
            {
                current.gameObject.GetComponent<Image>().sprite = wordSprite;
            }
            else if (letterHolders[i] != l)
            {
                current.gameObject.GetComponent<Image>().sprite = normalSprite;
            }
        }

    }

    public void dehighlight()
    {
        foreach (LetterHolder current in letterHolders)
        {
            if (current.gameObject.GetComponent<Image>().sprite != selectedSprite)
            {
                current.gameObject.GetComponent<Image>().sprite = normalSprite;
            }
        }

    }
}
