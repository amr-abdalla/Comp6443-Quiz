using TMPro;
using UnityEngine;

public class PlayerSpawningInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private GameObject startButton;
	private PlayerSpawner playerSpawner;

	private void OnEnable()
	{
		playerSpawner = GameManager.Instance.PlayerGroup.GetComponent<PlayerSpawner>();
		playerSpawner.CompleteSpawnAction += OnCompleteSpawn;
	}

	private void OnCompleteSpawn()
	{
		GameManager.Instance.initAll();
		startButton.SetActive(true);
		gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		playerSpawner.CompleteSpawnAction -= OnCompleteSpawn;
	}

	private void Update()
	{
		textMeshPro.text =
			"Click to Spawn your Characters\n" +
			$"{10 - playerSpawner.SpawnCount} Characters remain";
	}
}
