using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Terminal : MonoBehaviour
{
    public InputField commandField;
    public TMP_Text terminalField;
    public ScrollRect scrollRect;
    public FileSystem fileSystem;
    public DecryptionTool decryptionTool;
    public ToolsOpenClose toolsOpenClose;
    public Text readerTitle;
    List<FileSystemAccess> filesystems;
    public TextMeshProUGUI readerText;
    public Image imageViewer;
    public AudioPlayer audioPlayer;
    static readonly Regex trimmer = new Regex(@"\s\s+");
    List<Command> commands = new List<Command>();
    string statement;
    Stack<string> history = new Stack<string>();
    Stack<string> future = new Stack<string>();
    Action nextFunction;
    bool waitingForInput;
    string extraInput;
    public AudioSource SFXAS;
    public AudioClip successClip, failClip, notiClip;
    public SocialEngineeringTool SET;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] FileSystemAccessGOs = GameObject.FindGameObjectsWithTag("Connectable");
        filesystems = new List<FileSystemAccess>();
        foreach(GameObject FileSystemAccessGO in FileSystemAccessGOs)
        {
            FileSystemAccess FSA = new FileSystemAccess();
            FSA.filesystem = FileSystemAccessGO.GetComponent<FileSystem>();
            filesystems.Add(FSA);
        }
        EnterResponse("Enter 'help' for list of commands");
        commands.Add(new Command("help", () => this.Help(), "(no input) Lists all available commands"));
        commands.Add(new Command("connect", () => this.Connect(), "(requires input) Connects to the computer with the PAP number"));
        commands.Add(new Command("clear", () => this.ClearTerminal(), "(no input) Clears the terminal"));
        commands.Add(new Command("say", () => this.Say(), "(requires input) Repeats input"));
        commands.Add(new Command("list", () => this.List(), "(optional input) lists all system files in current document or path (if given)"));
        commands.Add(new Command("goto", () => this.ChangeDirectory(), "(requires input) changes the directory to the given path"));
        commands.Add(new Command("read", () => this.ReadDocument(), "(requires input) reads the document in the specified path"));
        commands.Add(new Command("dekrypt", () => this.Decrypt(), "(requires input) runs the Dekrypt software on the document in the specified path"));
        commands.Add(new Command("unlock", () => this.Unlock(), "(requires input) unlocks the document or folder in the specified path"));
        commands.Add(new Command("email", () => this.Emails(), "List all your emails. (optional input) add the email subject or number in the list to open it."));
        commands.Add(new Command("search", () => this.Search(), "(requires input) Searches for the requested query. Query must be enclosed with a quotation mark."));
        commands.Add(new Command("meta", () => this.Meta(), "(requires input) reads the metadata of the document in the specified path"));


    }

    int findCommand(string commandText)
    {
        for(int i = 0; i < commands.Count; i++)
        {
            if (commands[i].command.Equals(commandText))
            {
                return i;
            }
        }
        return -1;
    }

    int findCommand(Action commandAction)
    {
        for (int i = 0; i < commands.Count; i++)
        {
            if (commands[i].action.Equals(commandAction))
            {
                return i;
            }
        }
        return -1;
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow) && commandField.isFocused && history.Count >= 1)
        {

            string inputText = history.Pop();
            future.Push(inputText);
            commandField.text = inputText;

        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && commandField.isFocused && future.Count >= 1)
        {
            string inputText = future.Pop();
            history.Push(inputText);
            commandField.text = inputText;
        }

    }

    public void onSubmit()
    {
        if (!Input.GetKey(KeyCode.Return))
        {

            return;

        }
        if (waitingForInput)
        {
            extraInput = commandField.text;
            nextFunction.Invoke();
            extraInput = "";
            waitingForInput = false;
            nextFunction = null;
            ReadyForCommand();
            return;
        }
        statement = commandField.text.Trim();
        statement = trimmer.Replace(statement, " ");

        if (statement.Equals(""))
        {

            return;

        }


        history.Push(statement);
        EnterCommand(statement);

        string command = statement.IndexOf(" ") > -1
                          ? statement.Substring(0, statement.IndexOf(" "))
                          : statement;
        command = command.Trim().ToLower();
        int commandIndex = findCommand(command);
        if (commandIndex > -1)
        {
            commands[commandIndex].action.Invoke();
        }
        else
        {
            NotRecognized();
        }
        ReadyForCommand();
    }


    /*-----------------------------------------------------------------------command functions--------------------------------------------------------------------*/

    private void Connect()
    {
        if (statement.LastIndexOf(' ') == -1)
        {
            SFXAS.PlayOneShot(failClip);
            ParameterWarning("connect");
            return;
        }
        string acname = statement.Substring(statement.IndexOf(' ')).Trim();
        foreach(FileSystemAccess fsa in filesystems)
        {
            if (fsa.filesystem.ACName.ToUpper() == acname.ToUpper())
            {
                nextFunction = () => ConnectWPass(fsa);
                waitingForInput = true;
                EnterResponse("Enter password for " + fsa.filesystem.ACName + ":");
                SFXAS.PlayOneShot(notiClip);
                return;
            }
        }
        EnterResponse("Enter password for " + acname + ":");
        SFXAS.PlayOneShot(notiClip);
        waitingForInput = true;
        nextFunction = () => ConnectWPass(null);

    }
    private void ConnectWPass(FileSystemAccess fsa)
    {
        if (fsa == null)
        {
            SFXAS.PlayOneShot(failClip);
            EnterResponse("Incorrect password. Did not connect.");
            return;
        }
        if (fsa.filesystem.password.ToUpper() == extraInput.ToUpper())
        {
            SFXAS.PlayOneShot(successClip);
            fileSystem = fsa.filesystem;
            EnterResponse("Connected to " + fsa.filesystem.ACName);
            SET.addFSA(fsa);
        }
        else
        {
            SFXAS.PlayOneShot(failClip);
            EnterResponse("Incorrect password. Did not connect.");
        }
    }
    public void connectDirect(FileSystemAccess fsa)
    {
        fileSystem = fsa.filesystem;
        EnterResponse("Connected to " + fsa.filesystem.ACName);
        GetComponent<ToolsOpenClose>().openTool(1);
    }
    private void Help()
    {
        string fullHelp = "";
        foreach(Command command in commands)
        {
            string helpText = command.command + " |     " + command.description;
            fullHelp += helpText + "\n";
        }
        SFXAS.PlayOneShot(successClip);
        EnterResponse(fullHelp);
    }
    public string ExtractTextBetweenQuotes(string input)
    {
        int firstQuoteIndex = input.IndexOf('"');
        int lastQuoteIndex = input.LastIndexOf('"');

        if (firstQuoteIndex == -1 || firstQuoteIndex == lastQuoteIndex || lastQuoteIndex != input.Length - 1)
        {
            // No quotes found, only one quote found, or quotes not balanced.
            return "";
        }

        int countQuotes = input.Count(c => c == '"');

        if (countQuotes != 2)
        {
            // More or less than two quotes found.
            return "";
        }

        string textBetweenQuotes = input.Substring(firstQuoteIndex + 1, lastQuoteIndex - firstQuoteIndex - 1);
        return textBetweenQuotes;
    }

    private void Search()
    {
        string query = ExtractTextBetweenQuotes(statement);
        if (query != "")
        {
            string result = fileSystem.searchfilesystem(query);
            if (result == "")
            {
                SFXAS.PlayOneShot(failClip);
                EnterResponse("Your query returned no results");
            }
            else
            {
                SFXAS.PlayOneShot(successClip);
                EnterResponse(result);
            }
        }
        else
        {
            SFXAS.PlayOneShot(failClip);
            EnterResponse("Invalid search query");

        }
    }
    private void Emails()
    {
        if (statement.LastIndexOf(' ') == -1)
        {
            string children = fileSystem.listEmails();
            if (children.Trim() != "")
            {
                SFXAS.PlayOneShot(successClip);
                EnterResponse(children);
            }
            else
            {
                SFXAS.PlayOneShot(failClip);
                EnterResponse("No emails available.");
            }

        }
        else
        {
            string emailNum = statement.Substring(statement.IndexOf(' '));
            int order = 0;
            bool isNumber = int.TryParse(emailNum, out order); //i now = 108  
            if (isNumber)
            {
                Email email = fileSystem.gotoemail(order);
                if (email != null)
                {
                    SFXAS.PlayOneShot(successClip);
                    OpenEmail(email);
                }
                else
                {
                    SFXAS.PlayOneShot(failClip);
                    EnterResponse("Email not found.");
                }
            }
            else
            {
                Email email = fileSystem.gotoemail(emailNum);
                if (email != null)
                {
                    SFXAS.PlayOneShot(successClip);
                    OpenEmail(email);
                }
                else
                {
                    SFXAS.PlayOneShot(failClip);
                    EnterResponse("Email not found.");
                }
            }
        }

    }
    public void OpenEmail(Email email)
    {
        string text = email.text;
        //EnterResponse(text); return;
        readerTitle.text = email.filename;
        readerText.text = "From: " + email.from + "\nTo: " + email.to + "\nDate: " + email.date + "\n\nSubject: " + email.filename + "\n\n" + text;
        toolsOpenClose.openTool(3);
        EnterResponse("Email opened in Doc Viewer");
    }
    private void ChangeDirectory()
    {
        if (statement.LastIndexOf(' ') == -1)
        {
            SFXAS.PlayOneShot(failClip);
            ParameterWarning("goto");
            return;
        }
        if (!checkpass(() => ChangeDirectory()))
        {
            SFXAS.PlayOneShot(failClip);
            return;
        }
        FileDirectory directory = fileSystem.findDirectory(statement.Substring(statement.IndexOf(' ')).Trim());
        if (directory != null)
        {

            SFXAS.PlayOneShot(successClip);
            fileSystem.changeDirectory(statement.Substring(statement.IndexOf(' ')).Trim());
            EnterResponse("Changed directory to: <color=#00c7eb>" + fileSystem.getCurrentDirectoryPath() + "</color>");
        }
        else
        {
            SFXAS.PlayOneShot(failClip);
            EnterResponse("<color=#8c002a>Directory not found</color>");
        }


    }

    private void ReadDocument()
    {
        if (statement.LastIndexOf(' ') == -1)
        {
            SFXAS.PlayOneShot(failClip);
            ParameterWarning("read");
            return;
        }
        if (!checkpass(() => ReadDocument()))
        {
            SFXAS.PlayOneShot(failClip);
            return;
        }
        Document file = fileSystem.findDoc(statement.Substring(statement.IndexOf(' ')).Trim());
        if (file != null)
        {
            if (file.GetType().Equals(typeof(TextDocument)))
            {
                SFXAS.PlayOneShot(successClip);
                TextDocument textFile = (TextDocument)file;
                OpenTextDoc(textFile);
            }
            else if (file.GetType().Equals(typeof(ImageDocument)))
            {
                SFXAS.PlayOneShot(successClip);
                ImageDocument imageFile = (ImageDocument)file;
                OpenImageDoc(imageFile);
            }
            else if (file.GetType().Equals(typeof(AudioDocument)))
            {
                SFXAS.PlayOneShot(successClip);
                AudioDocument audioFile = (AudioDocument)file;
                OpenAudioDoc(audioFile);
            }
        }
        else
        {
            SFXAS.PlayOneShot(failClip);
            EnterResponse("<color=#8c002a>Document not found</color>");
        }



    }
    private void Meta()
    {
        if (statement.LastIndexOf(' ') == -1)
        {
            SFXAS.PlayOneShot(failClip);
            ParameterWarning("meta");
            return;
        }
        if (!checkpass(() => Meta()))
        {
            SFXAS.PlayOneShot(failClip);
            return;
        }
        SystemObject file = fileSystem.findSystemObject(statement.Substring(statement.IndexOf(' ')).Trim());
        if (file != null)
        {
            string metadata = "";
            metadata += "Name: " + file.filename + "\nType: " + file.GetType() + "\nCreation Date: " + file.date;
            EnterResponse(metadata);

        }
        else
        {
            SFXAS.PlayOneShot(failClip);
            EnterResponse("<color=#8c002a>Document not found</color>");
        }



    }
    public void OpenAudioDoc(AudioDocument audioFile)
    {
        audioPlayer.addClip(audioFile.clip, audioFile.getPath());
        toolsOpenClose.openTool(5);
        EnterResponse("Document opened in Audio Player");
    }
    public void OpenImageDoc(ImageDocument imageFile)
    {
        imageViewer.sprite = imageFile.image;
        toolsOpenClose.openTool(6);
        EnterResponse("Document opened in Picture Viewer");
    }
    public void OpenTextDoc(TextDocument textFile)
    {
        string text = textFile.text;
        //EnterResponse(text); return;
        if (textFile.encrypted)
            text = decryptionTool.scrambleText(textFile);

        readerTitle.text = textFile.filename;
        readerText.text = text;
        toolsOpenClose.openTool(3);
        EnterResponse("Document opened in Doc Viewer");
    }

    private void Decrypt()
    {
        if (statement.LastIndexOf(' ') == -1)
        {
            ParameterWarning("dekrypt");
            return;
        }
        if (!checkpass(() => Decrypt()))
        {
            return;
        }
        Document file = fileSystem.findDoc(statement.Substring(statement.IndexOf(' ')).Trim());
        if (file.GetType().Equals(typeof(TextDocument)))
        {
            TextDocument textFile = (TextDocument)file;

            string text = textFile.text;
            if (textFile.encrypted)
            {
                decryptionTool.createText(textFile);
                toolsOpenClose.openTool(0);
                EnterResponse("Document sent to Dekrypt tool");
            }
            else
            {
                EnterResponse("<color=#8c002a>Document is not encrypted</color>");
            }
        }
        else
        {
            EnterResponse("<color=#8c002a>Document is not a text document</color>");
        }




    }
    private void Unlock()
    {
        if (statement.LastIndexOf(' ') == -1)
        {
            SFXAS.PlayOneShot(failClip);
            ParameterWarning("unlock");
            return;
        }
        if (!checkpass(() => Unlock()))
        {
            SFXAS.PlayOneShot(failClip);
            return;
        }
        SystemObject systemObject = fileSystem.findSystemObject(statement.Substring(statement.IndexOf(' ')).Trim());
        if (systemObject != null)
        {

            SFXAS.PlayOneShot(successClip);
            EnterResponse(systemObject.filename + " is unlocked.");
        }
        else
        {
            SFXAS.PlayOneShot(failClip);
            EnterResponse("<color=#8c002a>Document or directory not found</color>");
        }


    }
    private bool checkpass(Action action)
    {
        string filename = statement.Substring(statement.IndexOf(' ')).Trim();

        string parentPassword = fileSystem.findParentPassword(filename);
        if (parentPassword != null)
        {
            EnterResponse("Directory " + parentPassword + " is password protected. Please unlock it first.");
            return false;
        }
        SystemObject systemObject = fileSystem.findSystemObject(filename);
        if (systemObject == null)
        {
            EnterResponse("<color=#8c002a>Document or directory not found</color>");
            return false;
        }
        if (systemObject.password != "" && !waitingForInput)
        {
            nextFunction = action;
            waitingForInput = true;
            EnterResponse("Enter password for " + systemObject.filename + ":");
            return false;
        }

        if (systemObject.password != extraInput && waitingForInput)
        {
            EnterResponse("Password was incorrect");
            return false;
        }

        systemObject.password = "";
        return true;
    }
    private void NotRecognized()
    {
        SFXAS.PlayOneShot(failClip);
        string firstWord = FirstWord();
        EnterResponse("Command <color=#8c002a>" + dontParse(firstWord) + "</color> not recognized");
    }

    private string FirstWord()
    {
        return statement.IndexOf(" ") > -1
                          ? statement.Substring(0, statement.IndexOf(" "))
                          : statement;
    }

    private void Say()
    {
        if (statement.LastIndexOf(' ') != -1)
        {
            SFXAS.PlayOneShot(successClip);
            EnterResponse(statement.Substring(statement.IndexOf(' ')).Trim());
        }
        else
        {
            SFXAS.PlayOneShot(failClip);
            ParameterWarning("say");
        }
    }

    private void List()
    {
        string children = "";
        if (statement.LastIndexOf(' ') == -1)
        {
            children = fileSystem.listChildren();

        }else if (!checkpass(() => List()))
        {
            SFXAS.PlayOneShot(failClip);
            return;
        }
        else{ 
            children = fileSystem.listChildren(statement.Substring(statement.IndexOf(' ')).Trim());
        }


        if (children.Trim() != "")
        {
            SFXAS.PlayOneShot(successClip);
            EnterResponse(children);
        }
        else
        {
            SFXAS.PlayOneShot(failClip);
            EnterResponse("<This directory is empty>");
        }
    }
    private void ParameterWarning(string command)
    {
        EnterResponse("Command <color=#8c002a>" + dontParse(command) + "</color> needs a parameter");

    }
    private string dontParse(string s)
    {
        return "<noparse>" + s + "</noparse>";
    }
    private void ReadyForCommand()
    {

        commandField.text = "";
        commandField.Select();
        commandField.ActivateInputField();
        StartCoroutine(scrollDown());
    }

    private void ClearTerminal()
    {
        if (!waitingForInput)
            terminalField.text = "";
        EnterResponse("Enter 'help' for list of commands");
        SFXAS.PlayOneShot(successClip);

    }
    private void EnterCommand(string text)
    {
        if (!terminalField.text.Equals(""))
            terminalField.text += "\n";

        terminalField.text += "<b><color=#0564fc>user" + fileSystem.getCurrentDirectoryPath() + ":  </color></b>" + dontParse(text);
    }

    public void EnterResponse(string text)
    {
        if (!terminalField.text.Equals(""))
            terminalField.text += "\n";

        terminalField.text += text;
    }

    IEnumerator scrollDown()
    {
        yield return null;
        scrollRect.verticalNormalizedPosition = 0;

    }
}
