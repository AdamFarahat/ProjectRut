using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Props
{
    public class LightFlicker : MonoBehaviour
    {
        Light lightSource;
        [SerializeField] float defaultLightIntensity = 1.0f;
        [SerializeField] bool isFlickering = false;
        public void Start()
        {
            lightSource = GetComponent<Light>();
        }

        public void Update()
        {
            if (lightSource && isFlickering)
            {
                lightSource.intensity = Random.Range(0, defaultLightIntensity);
            }
        }
    }
}