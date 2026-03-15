using UnityEngine;

public class JumpHandler : MonoBehaviour
{
	private const string _GroundTag = "Ground";
	public bool IsGrounded { get; private set; }

	[SerializeField] private float gravity = -20f;
	private float groundY = 0.99f;

	private Vector3 launchVelocity = Vector3.zero;

	private void Update()
	{
		if (!IsGrounded)
		{
			launchVelocity.y += gravity * Time.deltaTime;
			transform.position += launchVelocity * Time.deltaTime;
		}
	}

	public void Launch()
	{
		launchVelocity = transform.forward * 6f + Vector3.up * 12f;
		IsGrounded = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(_GroundTag))
		{
			IsGrounded = true;
			Vector3 pos = transform.position;
			pos.y = groundY;
			transform.position = pos;
			launchVelocity = Vector3.zero;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag(_GroundTag))
		{
			IsGrounded = false;
		}
	}
}
