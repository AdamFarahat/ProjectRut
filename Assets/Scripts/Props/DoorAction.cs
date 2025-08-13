using System.Collections;
using UnityEngine;

public class DoorAction : MonoBehaviour
{

	public Animator openandclose;
	public bool open;
	public Transform Player;

	[SerializeField] string openAnimation;

	[SerializeField] string closeAnimation;

	[SerializeField] AudioClip openSound;

	[SerializeField] AudioClip closeSound;

	public AudioSource doorAudioSource { get; private set; }

	void Start()
	{
		open = false;

		doorAudioSource = gameObject.GetComponent<AudioSource>();
	}

	void OnMouseOver()
	{
		{
			if (Player)
			{
				float dist = Vector3.Distance(Player.position, transform.position);
				if (dist < 15)
				{
					if (open == false)
					{
						if (Input.GetMouseButtonDown(0))
						{
							StartCoroutine(opening());
						}
					}
					else
					{
						if (open == true)
						{
							if (Input.GetMouseButtonDown(0))
							{
								StartCoroutine(closing());
							}
						}

					}

				}
			}

		}

	}

	public void OpenDoor()
	{
		StartCoroutine(opening());
	}

	public void CloseDoor()
	{
		StartCoroutine(closing());
	}

	public void TeleportPlayerTo(Transform player, Transform targetPosition)
	{
		if (player != null && targetPosition != null)
		{
			player.SetPositionAndRotation(targetPosition.position, targetPosition.rotation);
        }
		else
		{
			Debug.LogWarning("Player or target position is null.");
		}
	}

	public IEnumerator opening(float waitTime = 0.5f)
	{
		print("you are opening the door");
		openandclose.Play(openAnimation);
		doorAudioSource.clip = openSound;
		doorAudioSource.pitch = Random.Range(0.9f, 1.1f);
		doorAudioSource.Play();
		open = true;
		yield return new WaitForSeconds(waitTime);
	}

	public IEnumerator closing(float waitTime = 0.5f)
	{
		print("you are closing the door");
		openandclose.Play(closeAnimation);
		doorAudioSource.clip = closeSound;
		doorAudioSource.Play();
		open = false;
		yield return new WaitForSeconds(waitTime);
	}
}