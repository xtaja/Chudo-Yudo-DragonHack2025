using Unity.VisualScripting;
using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    public Object AudioManager;
    private AudioManager am;
    void Start()
    {
        am = AudioManager.GetComponent<AudioManager>();

    }

    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");
        am.StartCreepySounds();

    }
}
