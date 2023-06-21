using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileGenerator : MonoBehaviour
{
    public FileSystem fileSystem;

    public Sprite test_image;
    public AudioClip test_clip;
    // Start is called before the first frame update
    void Start()
    {
        fileSystem.addEmail(new Email("TestEmail", "", "Text/Emails/test", "test@mail.com"));
        fileSystem.addDirectory("/", "documents", "");
        fileSystem.addDoc("/documents", new TextDocument("test", "pass", "Text/Emails/test", false));

        fileSystem.addDirectory(".", "pictures", "");
        fileSystem.addDirectory("", "music", "");    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
