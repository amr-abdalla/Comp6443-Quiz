using UnityEngine;

public class CharacterCollisionHandler : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.parent.tag.BeatsFaction(transform.parent.tag))
		{
				Destroy(transform.parent.gameObject);
				return;
		}
	}
}
