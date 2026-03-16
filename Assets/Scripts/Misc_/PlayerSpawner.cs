using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
	private const float spawnY = 0.99f;
	private const string _GroundTag = "Ground";

	public int SpawnCount { get; private set; }
	private InputAction clickAction;
	private Camera cam;
	private CharactersSpawner charactersSpawner;
	public Action CompleteSpawnAction;

	private void Awake()
	{
		cam = Camera.main;

		clickAction = new InputAction(
			name: "Click",
			type: InputActionType.Button,
			binding: "<Mouse>/leftButton"
		);

		clickAction.performed += OnClick;

		charactersSpawner = GetComponent<CharactersSpawner>();
	}

	private void OnEnable()
	{
		clickAction.Enable();
	}

	private void OnDisable()
	{
		clickAction.Disable();
	}

	private void OnClick(InputAction.CallbackContext ctx)
	{
		Vector2 mousePos = Mouse.current.position.ReadValue();
		Ray ray = cam.ScreenPointToRay(mousePos);

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			if (hit.collider.CompareTag(_GroundTag))
			{
				Vector3 spawnPos = hit.point;
				spawnPos.y = spawnY;

				Instantiate(charactersSpawner.GetPrefabToSpawn(), spawnPos, Quaternion.identity, charactersSpawner.transform);
				SpawnCount++;

				if (SpawnCount >= 10)
				{
					OnCompleteSpawn();
				}
			}
		}
	}

	private void OnCompleteSpawn()
	{
		CompleteSpawnAction?.Invoke();
		Destroy(this);
	}
}
