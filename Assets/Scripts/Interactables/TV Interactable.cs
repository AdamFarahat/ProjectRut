using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class TVInteractable : Interactable
{

    private VideoPlayer videoPlayer;

    private GameObject _screen;

    private Renderer _screenRenderer;

    [SerializeField] private Material TVOffMaterial;

    [SerializeField] private Material TVOnMaterial;

    [SerializeField] private Material TVCameraFeedMaterial;

    [SerializeField] private bool isCameraFeed = false;

    [SerializeField] private AudioClip demonHeartBeat;

    private bool isPlaying = false;

    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Transform screenTransform = transform.Find("Screen");
        if (screenTransform != null)
        {
            _screen = screenTransform.gameObject;
        }
        _screenRenderer = _screen.GetComponent<Renderer>();
        videoPlayer = _screen.GetComponent<VideoPlayer>();
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component not found on the screen object.");
            return;
        }

        _screenRenderer.material = TVOffMaterial;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the TVInteractable object.");
        }

        if (demonHeartBeat == null)
        {
            Debug.LogError("Demon heartbeat audio clip not assigned in the TVInteractable object.");
        }
    }

    public override void Interact(Player player)
    {
        isPlaying = !isPlaying;
        Debug.Log("TV Interacted with. Is Playing: " + isPlaying);
        //If the tv was off, turn it on and player the video
        if (isPlaying)
        {
            StartCoroutine(WaitForVideoToStart());
        }
        else
        {
            _screenRenderer.material = TVOffMaterial;
            if (!isCameraFeed)
            {
                videoPlayer.Stop();
            }
        }
    }

    IEnumerator WaitForVideoToStart()
    {
        //Wait for video to start playing before changing the material
        if (!isCameraFeed)
        {
            videoPlayer.Play();
            yield return new WaitUntil(() => videoPlayer.isPlaying);
            _screenRenderer.material = TVOnMaterial;
        }
        else
        {
            _screenRenderer.material = TVCameraFeedMaterial;
        }
    }

    public void StartDemonEvent(Player player)
    {
        audioSource.clip = demonHeartBeat;
        audioSource.loop = true;
        audioSource.Play();
        StartCoroutine(WaitForVideoToStart());
    }
}
