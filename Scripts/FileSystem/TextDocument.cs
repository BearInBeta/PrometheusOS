using UnityEngine;

public class TextDocument : Document
{
    public string text;
    public bool encrypted;
    public char[] scrambledAlpha;
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

    public TextDocument(string filename, string password, string text, bool encrypted, bool sameText = false) : base(filename, password)
    {
        if(sameText)//the string text is the text
        {
            this.text = text;
        }
        else//the string text is the path of the text
        {
            this.text = Resources.Load<TextAsset>(text).text;
        }
        this.encrypted = encrypted;
        if (encrypted)
        {
            this.scrambledAlpha = new char[26];
            for (int i = 0; i < this.alpha.Length; i++)
            {
                int num = UnityEngine.Random.Range(0, 26);
                while (num == i || this.scrambledAlpha[num] != '\0')
                {
                    num = UnityEngine.Random.Range(0, 26);
                }
                this.scrambledAlpha[num] = this.alpha[i];
            }
        }
        else
        {
            scrambledAlpha = alpha;
        }
    }
}
