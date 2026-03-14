using UnityEngine;

public class Seek : State<IndividualAI>
{
	public Transform target;
	public LayerMask obstacleMask;

	public Seek(IndividualAI owner, StateMachine<IndividualAI> stateMachine) : base(owner, stateMachine) { }

	public override void Update()
	{
		if (target == null)
		{
			owner.ChooseNewState();
			return;
		}

		Vector3 seekDir = (target.transform.position - owner.transform.position).normalized;
		owner.MoveToDirection(seekDir);

		float dist = (owner.transform.position - target.transform.position).sqrMagnitude;

		if (dist > 7.5f * 7.5f)
		{
			owner.ChooseNewState();
		}
	}

}
