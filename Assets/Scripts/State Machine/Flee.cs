using UnityEngine;

public class Flee : State<IndividualAI>
{
	public Transform enemy;
	public LayerMask jumpMask;

	public Flee(IndividualAI owner, StateMachine<IndividualAI> stateMachine) : base(owner, stateMachine) { }

	public override void Update()
	{
		if (enemy == null)
		{
			owner.ChooseNewState();
			return;
		}

		Vector3 enemyDir = (enemy.position - owner.transform.position);
		enemyDir.y = 0;

		float distanceToEnemy = enemyDir.sqrMagnitude;

		if (distanceToEnemy <= 5f * 5f)
		{
			owner.TryStartBoost();
		}

		Transform nearestJumpPad = FindSafeJumpPad(enemyDir, distanceToEnemy);

		Vector3 moveDir;

		if (nearestJumpPad != null)
		{
			// Prioritize jump pad
			moveDir = (nearestJumpPad.position - owner.transform.position).normalized;
		}
		else
		{
			// Normal fleeing
			moveDir = (-enemyDir).normalized;
		}

		moveDir.y = 0;
		owner.TryMoveToDirection(moveDir);


		if (distanceToEnemy > 7.5f * 7.5f)
		{
			owner.ChooseNewState();
		}
	}

	private Transform FindSafeJumpPad(Vector3 enemyDir, float distanceToEnemy)
	{
		Collider[] hits = Physics.OverlapSphere(owner.transform.position, 5f, jumpMask);

		Transform bestPad = null;
		float bestDist = float.MaxValue;

		Vector3 enemyDirNorm = enemyDir.normalized;

		foreach (var hit in hits)
		{
			Vector3 toPad = hit.transform.position - owner.transform.position;
			toPad.y = 0;

			float padDist = toPad.sqrMagnitude;

			float dot = Vector3.Dot(toPad.normalized, enemyDirNorm);
			if (dot > 0f)
				continue;

			float enemyDistToPad = (enemy.position - hit.transform.position).sqrMagnitude;
			if (enemyDistToPad <= distanceToEnemy)
				continue;

			if (padDist < bestDist)
			{
				bestDist = padDist;
				bestPad = hit.transform;
			}
		}

		return bestPad;
	}

}
