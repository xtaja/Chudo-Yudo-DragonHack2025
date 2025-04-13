using UnityEngine;
using System.Collections;

public class DrawerLogic : MonoBehaviour
{
    [SerializeField] private CharacterControllerWithState playerController;
    [SerializeField] private Animator drawerAnimator;
    [SerializeField] private float pickupDuration = 5f;

    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F) && playerController.key)
        {
            Pickup();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
            isPlayerInRange = true;
            Debug.Log("Player entered interaction range.");
            // Optional: Show interaction UI
        
    }

    private void OnTriggerExit(Collider other)
    {
        
            isPlayerInRange = false;
            Debug.Log("Player left interaction range.");
            // Optional: Hide interaction UI
        
    }

    private void Pickup()
    {
        Debug.Log("Starting pickup...");
        if (playerController != null)
        {
            StartCoroutine(HandlePickupState());
        }
    }

    private IEnumerator HandlePickupState()
    {
        Debug.Log("Opening drawer...");
        
        drawerAnimator.SetBool("open", true);
        

        // Change player state to PickUp
        playerController.currentState = CharacterState.PickUp;
        //playerController.key = false;
        playerController.UpdateAnimator();

        yield return new WaitForSeconds(pickupDuration);

        Debug.Log("Pickup complete.");
        playerController.currentState = CharacterState.Idle;
        playerController.UpdateAnimator();

        // Optional: Disable the drawer or make the item disappear
        // gameObject.SetActive(false);
    }
}
