using UnityEngine;


public class EndGameListener : MonoBehaviour
{

    [SerializeField] private HUD hudToNotify;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && hudToNotify != null)
        {
            hudToNotify.ShowEndGamePrompt();
        }
    }
}
