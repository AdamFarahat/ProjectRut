using UnityEngine;
using System.Collections;

public class AmbientNoiseGenerator : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] ambientClips;

    [SerializeField] private float minDelay = 5f;
    [SerializeField] private float maxDelay = 10f;

    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;

    [SerializeField] private float minVolume = 0.8f;
    [SerializeField] private float maxVolume = 1.0f;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                Debug.LogError("AudioSource is missing on AmbientNoiseGenerator GameObject.");
        }
    }

    private void Start()
    {
        if (ambientClips == null || ambientClips.Length == 0)
        {
            Debug.LogWarning("No ambient clips assigned to AmbientNoiseGenerator.");
            return;
        }

        StartCoroutine(PlayAmbientLoop());
    }

    private IEnumerator PlayAmbientLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            AudioClip clip = ambientClips[Random.Range(0, ambientClips.Length)];
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.volume = Random.Range(minVolume, maxVolume);

            Debug.Log($"Playing ambient clip: {clip.name}");
            audioSource.PlayOneShot(clip);
        }
    }
}

