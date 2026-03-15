using UnityEngine;

public class CharactersSpawner : MonoBehaviour
{
	[SerializeField] private Vector2 spawnRange;
	[SerializeField] private int spawnCount;
	[SerializeField] private GameObject prefabToSpawn;

	public void StartSpawning(Transform parent)
	{
		for (int i = 0; i < spawnCount; i++)
		{
			float x = Random.Range(-spawnRange.x, spawnRange.x);
			float z = Random.Range(-spawnRange.y, spawnRange.y);

			Vector3 spawnposition = transform.position + new Vector3(x, 1f, z);

			Instantiate(prefabToSpawn, spawnposition, Quaternion.identity, parent);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		Vector3 center = new Vector3(transform.position.x, 0f, transform.position.z);
		Vector3 size = new Vector3(spawnRange.x * 2f, 0.1f, spawnRange.y * 2f);

		Gizmos.DrawWireCube(center, size);
	}

}
