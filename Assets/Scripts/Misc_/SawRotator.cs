using UnityEngine;

public class SawRotator : MonoBehaviour
{
	[SerializeField] private float rotationSpeed = 180f;

	private void Update()
	{
		transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
	}
}
