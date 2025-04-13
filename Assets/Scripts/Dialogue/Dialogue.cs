using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public GameObject eCanvas;
    public GameObject dTemplate;
    public GameObject canvas;
    public GameObject freeLookCamera1; // First FreeLook Camera
    public GameObject freeLookCamera2; // Second FreeLook Camera
    public Camera mainCamera; // Main Camera (to switch between the free look cameras)

    private bool playerDetected = false;
    private bool dialogueStarted = false;
    private bool isInHouse = false; // Flag to track if the player is in the house

    private string[] dialogueLines = {
        "Chapter One: The Hollow Cradle.",
        "She returns home, expecting warmth... but silence answers.",
        "The crib is shattered, blood staining the floorboards.",
        "No cries, no laughter—only clawed footprints trailing into the woods.",
        "The hearth is cold. The air, still.",
        "She doesn't scream. She never does.",
        "He took the child. And now she knows what he truly is.",
        "There was a book... a potion book. Locked away. Feared. Forgotten.",
        "She hid the key when she was afraid.",
        "She must find it now—no more hiding.",
        "Where would she have placed it? Somewhere small. Somewhere safe.",
        "She begins to search... for the key to the truth."
    };

    void Update()
    {
        // Check if player is inside and wants to start dialogue
        if (isInHouse && !dialogueStarted && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Starting dialogue...");
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        dialogueStarted = true;
        canvas.SetActive(true);
        ClearOldDialogue();

        foreach (string line in dialogueLines)
        {
            CreateDialogueLine(line);
        }

        // Show the first dialogue line
        if (canvas.transform.childCount > 2)
        {
            canvas.transform.GetChild(2).gameObject.SetActive(true);
        }

        // Enable click handler
        canvas.transform.GetChild(1).gameObject.SetActive(true);

        // Zoom in the camera when dialogue starts
        ZoomInCamera();
    }

    void CreateDialogueLine(string text)
    {
        GameObject clone = Instantiate(dTemplate, canvas.transform);
        clone.transform.localScale = Vector3.one;
        clone.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
        clone.SetActive(false);
    }

    void ClearOldDialogue()
    {
        for (int i = canvas.transform.childCount - 1; i >= 2; i--)
        {
            Destroy(canvas.transform.GetChild(i).gameObject);
        }
    }

    // Called when the player enters the trigger area (door)
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        if (!isInHouse) // If player is not already in the house, enter
    //        {
    //            Debug.Log("Player entered the trigger area.");
    //            isInHouse = true;
    //            FakeOnTriggerEnter();
    //        }
    //    }
    //}

    // Called when the player exits the trigger area (door)
    private void OnTriggerExit(Collider other)
    {
        isInHouse = !isInHouse;
        if (isInHouse)
        {
            FakeOnTriggerEnter();
        }
        else { 
            FakeOnTriggerExit();
        }
    }

    // Simulate OnTriggerEnter based on isInHouse flag
    void FakeOnTriggerEnter()
    {
        playerDetected = true;
        eCanvas.SetActive(true);
        Debug.Log("Simulating OnTriggerEnter: Player is now in the house.");

        // Activate first camera and deactivate second camera when player enters trigger
        ZoomInCamera();
    }

    // Simulate OnTriggerExit based on isInHouse flag
    void FakeOnTriggerExit()
    {
        playerDetected = false;
        dialogueStarted = false;
        eCanvas.SetActive(false);
        canvas.SetActive(false);
        ClearOldDialogue();
        Debug.Log("Simulating OnTriggerExit: Player has left the house.");

        // Reset camera zoom when player exits the trigger
        ResetCameraZoom();
    }

    // Function to zoom in the camera (activate first camera, deactivate second camera)
    void ZoomInCamera()
    {
        if (freeLookCamera1 != null)
        {
            freeLookCamera1.SetActive(true); // Enable first camera
            Debug.Log("freeLookCamera1 activated.");
        }

        if (freeLookCamera2 != null)
        {
            freeLookCamera2.SetActive(false); // Disable second camera
            Debug.Log("freeLookCamera2 deactivated.");
        }
    }

    // Function to reset the camera zoom (deactivate first camera, activate second camera)
    void ResetCameraZoom()
    {
        if (freeLookCamera1 != null)
        {
            freeLookCamera1.SetActive(false); // Disable first camera
            Debug.Log("freeLookCamera1 deactivated.");
        }

        if (freeLookCamera2 != null)
        {
            freeLookCamera2.SetActive(true); // Enable second camera
            Debug.Log("freeLookCamera2 activated.");
        }
    }
}
