using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private bool isPlayerInRange = false;
    public GameObject eCanvas;
    [SerializeField] private GameObject Item;
    public float pickupDuration = 1f; // how long the pickup animation lasts

    private CharacterControllerWithState playerController;

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

        Item.SetActive(false); // Hide or destroy the item
        eCanvas.SetActive(false); // Hide "Press E" UI
    }

    private System.Collections.IEnumerator HandlePickupState()
    {
        playerController.currentState = CharacterState.PickUp;
        yield return new WaitForSeconds(pickupDuration);
        playerController.currentState = CharacterState.Idle;
    }
}
