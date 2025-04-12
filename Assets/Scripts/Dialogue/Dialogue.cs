using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public GameObject eCanvas;
    public GameObject dTemplate;
    public GameObject canvas;

    private bool playerDetected = false;
    private bool dialogueStarted = false;

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
        if (playerDetected && !dialogueStarted && Input.GetKeyDown(KeyCode.E))
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            eCanvas.SetActive(true);
            playerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
            dialogueStarted = false;
            eCanvas.SetActive(false);
            canvas.SetActive(false);
            ClearOldDialogue();
        }
    }
}
