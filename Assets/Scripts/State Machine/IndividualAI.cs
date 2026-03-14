using System;
using System.Linq;
using UnityEngine;

public class IndividualAI : MonoBehaviour
{
	private StateMachine<IndividualAI> stateMachine;

	public Flee flee { get; private set; }
	public Seek seek { get; private set; }

	[HideInInspector] public float speed = 5f;
	public float collisionAvoidanceRayDistance = 3f;
	public LayerMask characterLayerMask;
	public LayerMask obstacleLayerMask;
	public float detectionRadius = 5f;
	public Action<IndividualAI> OnDeath;

	void Awake()
	{
		stateMachine = new StateMachine<IndividualAI>();

		flee = new Flee(this, stateMachine);
		flee.obstacleMask = obstacleLayerMask;
		seek = new Seek(this, stateMachine);
		seek.obstacleMask = obstacleLayerMask;
	}

	void Start()
	{
		ChooseNewState();
	}

	void Update()
	{
		if (stateMachine.CurrentState == null)
		{
			ChooseNewState();
		}

		stateMachine.Update();
	}

	public void MoveToDirection(Vector3 direction)
	{
		Vector3 finalDirection = GetAdjustedDirectionForObstacles(direction);
		transform.position += finalDirection * speed * Time.deltaTime;
	}

	public Vector3 GetAdjustedDirectionForObstacles(Vector3 direction)
	{
		Vector3 finalDir = direction;


		Vector3 origin = transform.position + Vector3.up * 0.3f;
		RaycastHit hit;

		// Forward ray
		if (Physics.Raycast(origin, direction, out hit, collisionAvoidanceRayDistance, obstacleLayerMask))
		{
			finalDir += hit.normal;
		}

		// Left ray
		Vector3 left = Quaternion.Euler(0, -35, 0) * direction;
		if (Physics.Raycast(origin, left, out hit, collisionAvoidanceRayDistance / 2f, obstacleLayerMask))
		{
			finalDir += hit.normal;
		}

		// Right ray
		Vector3 right = Quaternion.Euler(0, 35, 0) * direction;
		if (Physics.Raycast(origin, right, out hit, collisionAvoidanceRayDistance / 2f, obstacleLayerMask))
		{
			finalDir += hit.normal;
		}

		return finalDir.normalized;
	}

	public void ChooseNewState()
	{
		Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, characterLayerMask);

		string myTag = gameObject.tag;

		Transform nearestEnemy = hits
		.Where(h => h.gameObject != gameObject && h.tag.BeatsFaction(myTag))
		.OrderBy(h => (h.transform.position - transform.position).sqrMagnitude)
		.Select(h => h.transform)
		.FirstOrDefault();

		if (nearestEnemy != null)
		{
			flee.enemy = nearestEnemy;
			stateMachine.ChangeState(flee);
			return;
		}


		IndividualAI[] possibleTargets = GameManager.Instance.GetPossibleTargets(myTag);

		Transform nearestTarget = possibleTargets
		.Where(t => t != this)
		.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude)
		.Select(t => t.transform)
		.FirstOrDefault();

		seek.target = nearestTarget;
		stateMachine.ChangeState(seek);
	}

	private void OnDestroy()
	{
		OnDeath?.Invoke(this);
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, detectionRadius);
	}
}
