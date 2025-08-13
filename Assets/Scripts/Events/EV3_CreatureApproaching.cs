using System.Collections;
using UnityEngine;

public class EV3_CreatureApproaching : MonoBehaviour
{
    [SerializeField] private float approachDistance = 2f; // Distance at which the game ends

    [SerializeField] private float timeToEventEnd = 25f;

    [SerializeField] private bool isActivated = false;

    [SerializeField] private Player player;

    // Update is called once per frame
    void Update()
    {
        if (!isActivated) return;

        // Lerp the creature's position towards the player for a total duration of timeToEventEnd
        float newZ = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime / timeToEventEnd).z;
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);

        // Check if the creature is close enough to the player
        if (Vector3.Distance(transform.position, player.transform.position) < approachDistance)
        {
            Debug.Log("Creature has approached the player. Game Over!");
            // GAME OVER LOGIC HERE
            EndRoom.instance.StartEndRoomEvent();
        }
    }

    [NaughtyAttributes.Button("Activate Creature")]
    public void ActivateCreature()
    {
        isActivated = true;
        gameObject.SetActive(true);
        // StartCoroutine(ApproachCreature());
    }

    [NaughtyAttributes.Button("Stop Creature")]
    public void StopCreature()
    {
        isActivated = false;
        gameObject.SetActive(false);
        StopCoroutine(ApproachCreature());
    }
    
    IEnumerator ApproachCreature(float waitTime = 5f)
    {
        while (true)
        {
            transform.position += new Vector3(0, 0, 5f); // Move the creature forward
            yield return new WaitForSeconds(waitTime); // Wait for a short duration
        }
    }
}
