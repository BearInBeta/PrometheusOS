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
        fileSystem.addDirectory("/", "documents", "");
        fileSystem.addDirectory(".", "pictures", "");
        fileSystem.addDirectory("", "music", "");
        fileSystem.addDoc("", new TextDocument("freakingcase", "", "Text/casefile", true));
        fileSystem.addDirectory("documents", "casefiles", "abcd123");
        fileSystem.addDoc("/documents/casefiles", new TextDocument("case_234", "abcd123", "Text/casefile", true));
        fileSystem.addDoc("pictures", new ImageDocument("test_image", "abcd123", "Sprites/test_img"));
        fileSystem.addDoc("/", new AudioDocument("test_clip", "", "Audio/test_clip"));

        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
