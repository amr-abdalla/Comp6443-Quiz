using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

        Vector3 fleeDir = (owner.transform.position - enemy.transform.position).normalized;
        owner.MoveToDirection(fleeDir);

        float dist = (owner.transform.position - enemy.transform.position).sqrMagnitude;

        if (dist > 2.5f * 2.5f)
        {
            owner.ChooseNewState();
        }
    }
}
