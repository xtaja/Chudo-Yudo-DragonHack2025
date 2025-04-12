using UnityEngine;

public class NextDialogue : MonoBehaviour
{
    private int index = 2;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Transform canvas = transform;
            if (index < canvas.childCount)
            {
                canvas.GetChild(index).gameObject.SetActive(true);
                index++;
            }
            else
            {
                // Dialogue ended
                EndDialogue(canvas);
            }
        }
    }

    void EndDialogue(Transform canvas)
    {
        // Hide canvas after dialogue ends
        for (int i = 2; i < canvas.childCount; i++)
        {
            Destroy(canvas.GetChild(i).gameObject);
        }

        canvas.gameObject.SetActive(false);
        index = 2;
    }
}
