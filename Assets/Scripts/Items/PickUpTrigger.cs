using UnityEngine;
using System.Collections;

public class PickupItem : MonoBehaviour
{
    private bool isPlayerInRange = false;
    public GameObject eCanvas;
    [SerializeField] private GameObject Item;
    public float pickupDuration = 7f;

    [SerializeField]public  CharacterControllerWithState playerController;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            eCanvas.SetActive(true);
            playerController = other.GetComponent<CharacterControllerWithState>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            eCanvas.SetActive(false);
            playerController = null;
        }
    }

    private void Pickup()
    {
        Debug.Log("Item picked up!");
        if (playerController != null)
        {
            StartCoroutine(HandlePickupState());
        }

        // Don't deactivate the item yet — wait until the coroutine finishes
        eCanvas.SetActive(false); // Hide UI immediately
    }

    private IEnumerator HandlePickupState()
    {
        playerController.currentState = CharacterState.PickUp;
        
        playerController.UpdateAnimator();
        playerController.key = true;

        yield return new WaitForSeconds(pickupDuration);

        playerController.currentState = CharacterState.Idle;
        playerController.UpdateAnimator();

        // Now it's safe to deactivate the item
        Item.SetActive(false);
        gameObject.SetActive(false); // Optional: disable the pickup trigger itself too
    }
}
