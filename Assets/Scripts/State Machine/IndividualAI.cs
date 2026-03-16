using System;
using System.Linq;
using UnityEngine;

public class IndividualAI : MonoBehaviour
{
	private StateMachine<IndividualAI> stateMachine;
	private JumpHandler jumpHandler;
	private GroupAI groupAI;

	public Flee flee { get; private set; }
	public Seek seek { get; private set; }

	[HideInInspector] public float maxSpeed = 5f;
	[SerializeField] private float collisionAvoidanceRayDistance = 3f;
	[SerializeField] private LayerMask characterLayerMask;
	[SerializeField] private LayerMask obstacleLayerMask;
	[SerializeField] private LayerMask JumpLayerMask;
	[SerializeField] private float detectionRadius = 5f;
	public Action<IndividualAI> OnDeath;

	[SerializeField] private float boostSpeed = 3f;
	[SerializeField] private float boostduration = 3f;
	[SerializeField] private float boostCooldown = 3f;

	private float lastBoostTime;

	public bool CanBoost() => Time.time >= lastBoostTime + boostduration + boostCooldown;
	public bool IsBoosting() => Time.time < lastBoostTime + boostduration;

	public void TryStartBoost()
	{
		if (!CanBoost())
		{
			return;
		}

		lastBoostTime = Time.time;
	}

	public float GetBoostPercentage()
	{
		float timeSinceBoost = Time.time - lastBoostTime;

		if (timeSinceBoost < boostduration)
		{
			return 1f - (timeSinceBoost / boostduration);
		}

		float cooldownTime = timeSinceBoost - boostduration;
		if (cooldownTime < boostCooldown)
		{
			return cooldownTime / boostCooldown;
		}

		return 1f;
	}

	private float GetMaxSpeed()
	{
		if (IsBoosting())
		{
			return maxSpeed + boostSpeed;
		}

		return maxSpeed;
	}

	void Awake()
	{
		stateMachine = new StateMachine<IndividualAI>();
		jumpHandler = GetComponent<JumpHandler>();
		groupAI = GetComponentInParent<GroupAI>();

		flee = new Flee(this, stateMachine);
		seek = new Seek(this, stateMachine);
		flee.jumpMask = JumpLayerMask;
		seek.jumpMask = JumpLayerMask;

		lastBoostTime = Time.time - boostCooldown - boostduration;
	}

	void Update()
	{
		if (stateMachine.CurrentState == null)
		{
			return;
		}

		stateMachine.Update();
	}

	private float turnSpeed = 10f;

	public bool TryMoveToDirection(Vector3 direction)
	{
		if (!jumpHandler.IsGrounded)
		{
			return false;
		}

		Vector3 finalDirection = GetAdjustedDirectionForObstacles(direction);

		if (finalDirection.sqrMagnitude > 0.0001f)
		{
			Quaternion targetRot = Quaternion.LookRotation(finalDirection);
			transform.rotation = Quaternion.Slerp(
				transform.rotation,
				targetRot,
				turnSpeed * Time.deltaTime
			);
		}

		transform.position += finalDirection * GetMaxSpeed() * Time.deltaTime;
		return true;
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
		if (Physics.Raycast(origin, left, out hit, collisionAvoidanceRayDistance / 1.5f, obstacleLayerMask))
		{
			finalDir += hit.normal;
		}

		// Right ray
		Vector3 right = Quaternion.Euler(0, 35, 0) * direction;
		if (Physics.Raycast(origin, right, out hit, collisionAvoidanceRayDistance / 1.5f, obstacleLayerMask))
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

		seek.target = groupAI.GetTargetForMember(this);
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

	public State<IndividualAI> GetCurrentState()
	{
		return stateMachine.CurrentState;
	}
}
