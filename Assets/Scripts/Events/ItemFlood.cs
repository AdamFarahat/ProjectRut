using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFlood : MonoBehaviour
{
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private int initialPoolSize = 50;

    private Coroutine rainCoroutine;
    private Queue<GameObject> activeKeys = new Queue<GameObject>();
    [SerializeField] private int maxKeyCount = 50; // max keys in the scene at once
    [SerializeField] private float spawnInterval = 0.1f;
    [SerializeField] private float keyLifetime = 10f;

    private Queue<GameObject> keyPool = new Queue<GameObject>();

    void Start()
    {
        // Pre-warm pool
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject key = Instantiate(keyPrefab, transform);
            key.SetActive(false);
            keyPool.Enqueue(key);
        }
    }

    private IEnumerator DespawnKeyAfterTime(GameObject key, float time)
    {
        yield return new WaitForSeconds(time);

        key.SetActive(false);
        keyPool.Enqueue(key);
    }

    public void StartRainingKeys()
    {
        if (rainCoroutine == null)
            rainCoroutine = StartCoroutine(RainKeys());
    }

    public void StopRainingKeys()
    {
        if (rainCoroutine != null)
        {
            StopCoroutine(rainCoroutine);
            rainCoroutine = null;
        }
    }

    private IEnumerator RainKeys()
    {
        while (true)
        {
            GameObject key = this.SpawnKey();
            activeKeys.Enqueue(key);

            if (activeKeys.Count > maxKeyCount)
            {
                GameObject oldKey = activeKeys.Dequeue();
                if (oldKey != null)
                    Destroy(oldKey);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public GameObject SpawnKey()
    {
        Vector3 offset = Random.insideUnitSphere * 2f;
        offset.y = Mathf.Abs(offset.y) + 5f; // Spawn above ground
        Vector3 spawnPos = transform.position + offset;

        GameObject key = Instantiate(keyPrefab, spawnPos, Quaternion.identity);
        Destroy(key, keyLifetime); // Optional: auto-destroy key
        return key;
    }

}


