using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusicSource;
    public AudioSource soundEffectSource;

    public AudioClip forestMusic;
    public AudioClip[] creepySounds;

    public float minCreepyDelay = 5f;
    public float maxCreepyDelay = 15f;

    private bool playCreepySounds = false;

    void Start()
    {
        if (backgroundMusicSource != null && forestMusic != null)
        {
            backgroundMusicSource.clip = forestMusic;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();
        }
    }

    public void StartCreepySounds()
    {
        if (!playCreepySounds)
        {
            playCreepySounds = true;
            StartCoroutine(PlayCreepySounds());
        }
    }

    private IEnumerator PlayCreepySounds()
    {
        while (playCreepySounds)
        {
            yield return new WaitForSeconds(Random.Range(minCreepyDelay, maxCreepyDelay));

            if (creepySounds.Length > 0)
            {
                int index = Random.Range(0, creepySounds.Length);
                soundEffectSource.PlayOneShot(creepySounds[index]);
            }
        }
    }

    public void StopCreepySounds()
    {
        playCreepySounds = false;
    }
}
