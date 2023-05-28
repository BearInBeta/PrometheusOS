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
        fileSystem.addDirectory("/", "emails", "");
        fileSystem.addEmail(new Email("Urgent: Dr. Blackwood's Disappearance", "", "Text/Emails/sarah johnson", "sara.johnson@promail.com"));
        fileSystem.addDirectory("/", "documents", "");
        fileSystem.addDirectory(".", "pictures", "");
        fileSystem.addDirectory("", "music", "");    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
