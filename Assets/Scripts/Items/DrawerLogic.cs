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
        
    }

    private void OnTriggerExit(Collider other)
    {
        
            isPlayerInRange = false;
            Debug.Log("Player left interaction range.");
        
    }

    private void Pickup()
    {
        Debug.Log("Starting pickup...");
        if (playerController != null)
        {
            HandlePickupState();
        }
    }

    private void HandlePickupState()
    {
        Debug.Log("Opening drawer...");
        drawerAnimator.SetBool("open", true);
        Debug.Log("Pickup complete.");
        
    }
}
