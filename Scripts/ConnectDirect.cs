using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectDirect : MonoBehaviour
{
    public FileSystemAccess fsa;
    public Terminal terminal;
    // Start is called before the first frame update
    public void connectDirectProFile()
    {
        terminal.connectDirect(fsa);
    }
}
