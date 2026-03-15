using UnityEngine;

public class Seek : State<IndividualAI>
{
	public Transform target;
	public LayerMask jumpMask;

	private float jumpAvoidRadius = 2.5f;
	private float avoidStrength = 2f;

	public Seek(IndividualAI owner, StateMachine<IndividualAI> stateMachine) : base(owner, stateMachine) { }

	public override void Update()
	{
		if (target == null)
		{
			owner.ChooseNewState();
			return;
		}

		Vector3 seekDir = (target.transform.position - owner.transform.position);
		seekDir.y = 0;
		seekDir.Normalize();

		Vector3 avoidDir = GetJumpPadAvoidance();
		Vector3 finalDir = (seekDir + avoidDir * avoidStrength).normalized;

		owner.TryMoveToDirection(finalDir);

		float dist = (owner.transform.position - target.transform.position).sqrMagnitude;

		if (dist > 7.5f * 7.5f)
		{
			owner.ChooseNewState();
		}
	}

	private Vector3 GetJumpPadAvoidance()
	{
		Collider[] hits = Physics.OverlapSphere(owner.transform.position, jumpAvoidRadius, jumpMask);

		Vector3 avoid = Vector3.zero;

		foreach (var hit in hits)
		{
			Vector3 away = owner.transform.position - hit.transform.position;
			away.y = 0;

			float dist = away.magnitude;

			if (dist > 0.001f)
			{
				avoid += away.normalized / dist;
			}
		}

		return avoid;
	}
}
