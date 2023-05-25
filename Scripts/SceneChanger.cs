using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{

    private string sceneName;              // Scene name used for know which is the current scene to use fade effect on spisific trigger
    public Button buttonNext, buttonPrev; // Get the name of the button to use it to move scene to the next or previous one 
    public Animator animator;             // Main animator for fade in and out animation
    private string FadeingToScene;           // The next scene to fade on key press
    public ToggleGroup options;           // Start scene options ( create accout, login, credits )
    private string toggleName;            // The name of selected toggle

    // Hide mouse cursor in intro scene
    void Start() {
        sceneName = SceneManager.GetActiveScene().name;
        Cursor.visible = (sceneName != "Intro"); 
    }

    void Update()
    {
        // Fade to next scene on key press then make the mouse cursor visible
        if (sceneName == "Intro" && Input.anyKeyDown) FadeToScene("MainMenu");        // #2 Login & signup scene 
    }

    // Fade to scene function
    public void FadeToScene(string sceneName)
    {
        // Set the trigger to fade out animation when press a key to fade the current scene out then move to the next scene
        FadeingToScene = sceneName;
        animator.SetTrigger("FadeOut");
    }

    // Move to scene function
    public void FadeComplete() { SceneManager.LoadScene(FadeingToScene); }

    // Next button on click function
    public void NextButtonClick()
    {
        /*if (sceneName == "CreateProfile") FadeToScene(4);              // #4 Terms & conditions scene
        else if (sceneName == "Terms&Conditions") FadeToScene(5);      // #5 Finishing setup scene
        else if (sceneName == "FinishingSetup") FadeToScene(6);        // #6 Main game scene*/
    }

    // Previous button on click function
    public void PrevButtonClick()
    {
        if (sceneName == "Credits") FadeToScene("MainMenu");
        /*if (sceneName == "Terms&Conditions") FadeToScene(3);           // #3 Create profile scene
        else if (sceneName == "CreateProfile") FadeToScene(2);         // #2 login & signup scene
        else if (sceneName == "Credits") FadeToScene("MainMenu");  // #2 login & signup scene
        else if (sceneName == "LoginToAnotherAccount") FadeToScene(2); // #2 login & signup scene*/
    }

    // Continue button on click function
    public void onContinueClick()
    {
        // Check selected toggle and get it's name
        foreach (Toggle toggle in options.ActiveToggles())
        {
            toggleName = toggle.name.ToString();
            FadeToScene(toggleName);
            /* // Go to the right scene :)
            if (toggleName == "CreateNewAccount")
            {
                FadeToScene(toggleName);  // #3 Create profile scene
            }
            else if (toggleName == "LoginToYourAccount")
            {
                FadeToScene("MainGame");  // #6 Main game scene
            }
            /*else if (toggleName == "LoginToAnotherAccount")
            {
                FadeToScene(7);  // #7 Login to another account scene
            }*/
            /*else if (toggleName == "Credits&Informations")
            {
                FadeToScene("Credits");  // #8 Credits & Informations scene
            }*/
        }
    }

    // Close application button function
    public void CloseApp() { Application.Quit(); }

}