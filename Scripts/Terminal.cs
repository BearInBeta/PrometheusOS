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
    public TMP_InputField readerText;
    public Image imageViewer;
    public AudioPlayer audioPlayer;
    static readonly Regex trimmer = new Regex(@"\s\s+");
    List<Command> commands = new List<Command>();
    List<Command> lockedcommands = new List<Command>();
    public string statement;
    Stack<string> history = new Stack<string>();
    Stack<string> future = new Stack<string>();
    Action nextFunction;
    bool waitingForInput;
    string extraInput;
    public AudioSource SFXAS;
    public AudioClip successClip, failClip, notiClip;
    public SocialEngineeringTool SET;
    public bool finalWait = false;
    public bool notutorial = true;
    public Tutorial tutorial;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("VV203R", 0);
        PlayerPrefs.SetInt("MA318F8", 0);

        GameObject[] FileSystemAccessGOs = GameObject.FindGameObjectsWithTag("Connectable");
        filesystems = new List<FileSystemAccess>();
        foreach(GameObject FileSystemAccessGO in FileSystemAccessGOs)
        {
            FileSystemAccess FSA = new FileSystemAccess();
            FSA.filesystem = FileSystemAccessGO.GetComponent<FileSystem>();
            filesystems.Add(FSA);

            if (PlayerPrefs.HasKey(FSA.filesystem.ACName))
            {
                SET.addFSA(FSA);
            }
        }
        EnterResponse("Enter 'help' for list of commands");
        lockedcommands.Add(new Command("help", () => this.Help(), "(no input) Lists all available commands"));
        lockedcommands.Add(new Command("connect", () => this.Connect(), "(requires input) Connects to the computer with the PAP number"));
        lockedcommands.Add(new Command("clear", () => this.ClearTerminal(), "(no input) Clears the terminal"));
        lockedcommands.Add(new Command("say", () => this.Say(), "(requires input) Repeats input"));
        lockedcommands.Add(new Command("list", () => this.List(), "(optional input) lists all system files in current document or path (if given)"));
        lockedcommands.Add(new Command("goto", () => this.ChangeDirectory(), "(requires input) changes the directory to the given path"));
        lockedcommands.Add(new Command("read", () => this.ReadDocument(), "(requires input) reads the document in the specified path"));
        lockedcommands.Add(new Command("dekrypt", () => this.Decrypt(), "(requires input) runs the Dekrypt software on the document in the specified path"));
        lockedcommands.Add(new Command("unlock", () => this.Unlock(), "(requires input) unlocks the document or folder in the specified path"));
        lockedcommands.Add(new Command("email", () => this.Emails(), "List all your emails. (optional input) add the email subject or number in the list to open it."));
        lockedcommands.Add(new Command("search", () => this.Search(), "(requires input) Searches for the requested query. Query must be enclosed with a quotation mark."));
        lockedcommands.Add(new Command("meta", () => this.Meta(), "(requires input) reads the metadata of the document in the specified path"));

        if(notutorial) unlockAllCommands();

    }
    public void unlockAllCommands()
    {
        foreach(Command comm in lockedcommands)
        {
            if (!commands.Contains(comm))
                commands.Add(comm);
        }
    }

    public void lockAllCommands()
    {
        commands.Clear();
    }
    public void unlockCommand(int index, bool visible = false)
    {
        if(!commands.Contains(lockedcommands[index]))
        commands.Add(lockedcommands[index]);

        if (visible)
        {
            EnterSuccessResponse("Command '" + lockedcommands[index].command + "' unlocked");
        }
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
            EnterErrorResponse("Incorrect password. Did not connect.");
            return;
        }
        if (fsa.filesystem.password.ToUpper() == extraInput.ToUpper())
        {
            fileSystem = fsa.filesystem;
            EnterSuccessResponse("Connected to " + fsa.filesystem.ACName);
            SET.addFSA(fsa);
        }
        else
        {
            EnterErrorResponse("Incorrect password. Did not connect.");
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
        EnterNormalResponse(fullHelp);
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
                EnterErrorResponse("Your query returned no results");
            }
            else
            {
                EnterNormalResponse(result);
            }
        }
        else
        {
            EnterErrorResponse("Invalid search query");

        }
    }
    private void Emails()
    {
        if (statement.LastIndexOf(' ') == -1)
        {
            string children = fileSystem.listEmails();
            if (children.Trim() != "")
            {
                EnterNormalResponse(children);
            }
            else
            {
                EnterErrorResponse("No emails available.");
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
                    OpenEmail(email);
                }
                else
                {
                    EnterErrorResponse("Email not found.");
                }
            }
            else
            {
                Email email = fileSystem.gotoemail(emailNum);
                if (email != null)
                {
                    OpenEmail(email);
                }
                else
                {
                    EnterErrorResponse("Email not found.");
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
        EnterSuccessResponse("Email opened in Doc Viewer");
    }
    private void ChangeDirectory()
    {
        if (statement.LastIndexOf(' ') == -1)
        {
            ParameterWarning("goto");
            return;
        }
        if (!checkpass(() => ChangeDirectory()))
        {
            return;
        }
        FileDirectory directory = fileSystem.findDirectory(statement.Substring(statement.IndexOf(' ')).Trim());
        if (directory != null)
        {

            fileSystem.changeDirectory(statement.Substring(statement.IndexOf(' ')).Trim());
            EnterNormalResponse("Changed directory to: <color=#00c7eb>" + fileSystem.getCurrentDirectoryPath() + "</color>");
        }
        else
        {
            EnterErrorResponse("<color=#8c002a>Directory not found</color>");
        }


    }

    private void ReadDocument()
    {
        if (statement.LastIndexOf(' ') == -1)
        {
            ParameterWarning("read");
            return;
        }
        if (!checkpass(() => ReadDocument()))
        {
            return;
        }
        Document file = fileSystem.findDoc(statement.Substring(statement.IndexOf(' ')).Trim());
        if (file != null)
        {
            if (file.GetType().Equals(typeof(TextDocument)))
            {
                TextDocument textFile = (TextDocument)file;
                OpenTextDoc(textFile);
            }
            else if (file.GetType().Equals(typeof(ImageDocument)))
            {
                ImageDocument imageFile = (ImageDocument)file;
                OpenImageDoc(imageFile);
            }
            else if (file.GetType().Equals(typeof(AudioDocument)))
            {
                AudioDocument audioFile = (AudioDocument)file;
                OpenAudioDoc(audioFile);
            }
        }
        else
        {
            EnterErrorResponse("<color=#8c002a>Document not found</color>");
        }



    }
    private void Meta()
    {
        if (statement.LastIndexOf(' ') == -1)
        {
            ParameterWarning("meta");
            return;
        }
        if (!checkpass(() => Meta()))
        {
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
            EnterErrorResponse("<color=#8c002a>Document not found</color>");
        }



    }
    public void OpenAudioDoc(AudioDocument audioFile)
    {
        audioPlayer.addClip(audioFile.clip, audioFile.getPath());
        toolsOpenClose.openTool(5);
        EnterSuccessResponse("Document opened in Audio Player");
    }
    public void OpenImageDoc(ImageDocument imageFile)
    {
        imageViewer.sprite = imageFile.image;
        toolsOpenClose.openTool(6);
        EnterSuccessResponse("Document opened in Picture Viewer");
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
        if (textFile.encrypted == true && !PlayerPrefs.HasKey("dekryptTutorialDone"))
        {
            PlayerPrefs.SetInt("dekryptTutorialDone", 0);
            StartCoroutine(tutorial.dekryptTutorial());
        }
        EnterSuccessResponse("Document opened in Doc Viewer");
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
            ParameterWarning("unlock");
            return;
        }
        if (!checkpass(() => Unlock()))
        {
            return;
        }
        SystemObject systemObject = fileSystem.findSystemObject(statement.Substring(statement.IndexOf(' ')).Trim());
        if (systemObject != null)
        {

            EnterSuccessResponse(systemObject.filename + " is unlocked.");
        }
        else
        {
            EnterErrorResponse("<color=#8c002a>Document or directory not found</color>");
        }


    }
    private bool checkpass(Action action)
    {
        string filename = statement.Substring(statement.IndexOf(' ')).Trim();

        string parentPassword = fileSystem.findParentPassword(filename);
        if (parentPassword != null)
        {
            EnterErrorResponse("Directory " + parentPassword + " is password protected. Please unlock it first.");
            return false;
        }
        SystemObject systemObject = fileSystem.findSystemObject(filename);
        if (systemObject == null)
        {
            EnterErrorResponse("<color=#8c002a>Document or directory not found</color>");
            return false;
        }
        if (systemObject.password != "" && !waitingForInput)
        {
            nextFunction = action;
            waitingForInput = true;
            EnterNormalResponse("Enter password for " + systemObject.filename + ":");
            return false;
        }

        if (systemObject.password != extraInput && waitingForInput)
        {
            EnterErrorResponse("Password was incorrect");
            return false;
        }

        systemObject.password = "";
        return true;
    }
    private void NotRecognized()
    {
        string firstWord = FirstWord();
        EnterErrorResponse("Command <color=#8c002a>" + dontParse(firstWord) + "</color> not recognized");
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
            EnterNormalResponse(statement.Substring(statement.IndexOf(' ')).Trim());
        }
        else
        {
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
            return;
        }
        else{ 
            children = fileSystem.listChildren(statement.Substring(statement.IndexOf(' ')).Trim());
        }


        if (children.Trim() != "")
        {
            EnterNormalResponse(children);
        }
        else
        {
            EnterErrorResponse("<This directory is empty>");
        }
    }
    private void ParameterWarning(string command)
    {
        EnterErrorResponse("Command <color=#8c002a>" + dontParse(command) + "</color> needs a parameter");

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
        EnterNormalResponse("Enter 'help' for list of commands");

    }
    private void EnterCommand(string text)
    {
        if (!terminalField.text.Equals(""))
            terminalField.text += "\n";

        terminalField.text += "<b><color=#0564fc>user" + fileSystem.getCurrentDirectoryPath() + ":  </color></b>" + dontParse(text);
    }
    public void EnterErrorResponse(string text)
    {
        SFXAS.PlayOneShot(failClip);
        EnterResponse("<b><color=#9c0000>" + text + "</color></b>");
    }
    public void EnterSuccessResponse(string text)
    {
        SFXAS.PlayOneShot(successClip);
        EnterResponse("<b><color=#01800e>" + text + "</color></b>");
    }
    public void EnterNormalResponse(string text)
    {
        SFXAS.PlayOneShot(successClip);
        EnterResponse(text);
    }
    public void removeText(string text)
    {
        terminalField.text = terminalField.text.Replace(text, "");
    }
    public void EnterResponse(string text)
    {
        if (!terminalField.text.Equals(""))
            terminalField.text += "\n";

        terminalField.text += text;
        StartCoroutine(scrollDown());

    }

    IEnumerator scrollDown()
    {
        yield return null;
        scrollRect.verticalNormalizedPosition = 0;

    }
    private void FinalChoice(string choice)
    {
   
        if(choice.Equals("no"))
            EnterResponse("are you sure you want to delete all the data related to the case? (Y/N)");
        else
            EnterResponse("are you sure you want to send the data to " + choice + "? (Y/N)");
        SFXAS.PlayOneShot(notiClip);
        waitingForInput = true;
        nextFunction = () => Confirm(choice);

    }
    private void Confirm(string choice)
    {
        if (extraInput.ToUpper().Equals("Y"))
        {
            if (choice.Equals("no"))
                EnterResponse("Data deleted");
            else
                EnterSuccessResponse("Data sent to " + choice);

            finalWait = true;
            return;
        }else
        {
            EnterErrorResponse("Choice unconfirmed, try again");
        }
    }
    public void endingCommands()
    {
        commands.Add(new Command("police", () => this.FinalChoice("the police"), "Send all the findings to the police"));
        commands.Add(new Command("national", () => this.FinalChoice("the national"), "Send all the findings to journalist frank cunningham in the national"));
        commands.Add(new Command("acorp", () => this.FinalChoice("A-Corp"), "Send all the findings to A-Corp"));
        commands.Add(new Command("delete", () => this.FinalChoice("no"), "Delete all your findings and leave the research be"));
        EnterSuccessResponse("command 'police' added");
        EnterSuccessResponse("command 'national' added");
        EnterSuccessResponse("command 'acorp' added");
        EnterSuccessResponse("command 'delete' added");

    }
}
