using UnityEngine;

public class JumpPad : MonoBehaviour
{
	[SerializeField] private float launchForce = 12f;
	[SerializeField] private float forwardForce = 6f;

	private void OnTriggerEnter(Collider other)
	{
		JumpHandler jump = other.GetComponent<JumpHandler>();

		if (jump != null && jump.IsGrounded)
		{
			Vector3 launchVelocity = other.transform.forward * forwardForce + Vector3.up * launchForce;
			jump.Launch(launchVelocity);
		}
	}
}