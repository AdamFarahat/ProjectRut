using UnityEngine;

public class FrameInteractable : Interactable
{
    [SerializeField] private Renderer imageRenderer;

    [SerializeField] private Material frameMaterialNormal;

    [SerializeField] private Material frameMaterialTrigered;

    [SerializeField] private bool isOneTime = false;
    [SerializeField] private bool isTriggered = false;

    public void Start()
    {
        imageRenderer.material = frameMaterialNormal;
    }

    // One time interaction
    public override void Interact(Player player)
    {
        if (isOneTime && isTriggered)
        {
            return;
        }

        isTriggered = !isTriggered;
        if (isTriggered)
        {
            imageRenderer.material = frameMaterialTrigered;
        }
        else
        {
            imageRenderer.material = frameMaterialNormal;
        }
    }
}
