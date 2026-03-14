using UnityEngine;

public class CharacterCollisionHandler : MonoBehaviour
{

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.CompareTag(FactionTags._Saw) || other.transform.tag.BeatsFaction(transform.tag))
		{
				Destroy(transform.gameObject);
				return;
		}
	}
}
