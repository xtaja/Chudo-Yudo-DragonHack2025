using System.Collections;
using UnityEngine;

public class TestMaterial : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(FreezeCameraAfterOneFrame());
    }

    IEnumerator FreezeCameraAfterOneFrame()
    {
        yield return new WaitForEndOfFrame(); // Wait for 1 frame
        GetComponent<Camera>().enabled = false; // Freeze
    }
}
