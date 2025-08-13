using TMPro;
using UnityEngine;

public class EV2_FrameChecker : MonoBehaviour
{
    [SerializeField] private Renderer[] materialsToCheck;
    [SerializeField] private Renderer[] frameRenderers;

    public bool isCorrectMaterial = false;

    private GameObject keySpawner;

    public bool hasSpawnedKey = false;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (frameRenderers.Length == 0)
        {
            Debug.LogWarning("No frame renderers assigned to EV2_FrameChecker.");
            return;
        }

        foreach (var renderer in frameRenderers)
        {
            if (renderer == null)
            {
                Debug.LogWarning("One of the frame renderers is not assigned.");
                return;
            }
        }

        keySpawner = transform.Find("KeySpawner")?.gameObject;

        isCorrectMaterial = false;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < frameRenderers.Length; i++)
        {
            if (frameRenderers[i].material.name != materialsToCheck[i].material.name)
            {
                isCorrectMaterial = false; // If any material does not match
                return;
            }
        }

        isCorrectMaterial = true; // If all materials match
    }
}
