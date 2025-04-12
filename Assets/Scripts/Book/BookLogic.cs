using UnityEngine;

public class OpenOnKeyPress : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            
            animator.SetBool("open", true);
        }
    }
}
