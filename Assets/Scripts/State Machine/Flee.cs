using UnityEngine;

public class Flee : State<IndividualAI>
{
	public Transform enemy;
	public LayerMask obstacleMask;

	public Flee(IndividualAI owner, StateMachine<IndividualAI> stateMachine) : base(owner, stateMachine) { }

	public override void Update()
	{
		if (enemy == null)
		{
			owner.ChooseNewState();
			return;
		}

		float dist = (owner.transform.position - enemy.transform.position).sqrMagnitude;

		if (dist > 5f * 5f)
		{
			owner.TryStartBoost();
		}

		Vector3 fleeDir = (owner.transform.position - enemy.transform.position).normalized;
		owner.MoveToDirection(fleeDir);


		if (dist > 7.5f * 7.5f)
		{
			owner.ChooseNewState();
		}
	}
}
