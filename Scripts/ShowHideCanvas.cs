using UnityEngine;

public class ShowHideCanvas : MonoBehaviour
{

    public GameObject Canvas; // The main canvas to show or hide on button click
                              // Show canvas
    public void openCanvas()
    {
        if (Canvas != null)
        {
            // Active the canvas
            Canvas.SetActive(true);
            // Show the canvas with fade animation
            Animator animator = Canvas.GetComponent<Animator>();
            if (animator != null)
                animator.SetBool("Open", true);
        }
    }

    // Hide canvas
    public void closeCanvas()
    {
        if (Canvas != null)
        {
            // Hide the canvas with fade animation
            Animator animator = Canvas.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("Open", false);
            }
            // Deactive the canvas
            Canvas.SetActive(false);
        }
    }

}
