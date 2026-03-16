using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroupAI : MonoBehaviour
{
	[SerializeField] private float calmSpeed = 2f;
	[SerializeField] private float averageSpeed = 6f;
	[SerializeField] private float aggressiveSpeed = 10f;
	[SerializeField] private Vector2Int friendlyUnitsBounds;
	[SerializeField] private Vector2Int targetUnitsBounds;
	[SerializeField] private Vector2Int enemyUnitsBounds;

	private float currentSpeed;
	public List<IndividualAI> members { get; private set; }
	public bool isReady = false;

	public void SpawnAllMembers()
	{
		CharactersSpawner spawner = GetComponent<CharactersSpawner>();
		spawner.StartSpawning(transform);
	}

	public void init()
	{
		members = GetComponentsInChildren<IndividualAI>().ToList();

		foreach (IndividualAI member in members)
		{
			member.OnDeath += OnMemberDeath;
		}
	}

	public void StartMoving()
	{
		foreach (IndividualAI member in members)
		{
			member.ChooseNewState();
		}

		isReady = true;
	}

	private void Update()
	{
		if (!GameManager.Instance.isReady())
		{
			return;
		}

		if (members != null)
		{
			UpdateFactionSpeed();
		}
	}

	private void OnMemberDeath(IndividualAI member)
	{
		members.Remove(member);

		if (members.Count == 0)
		{
			GameEnd.Instance.ToggleEndGame(tag);
			return;
		}
	}

	public void UpdateFactionSpeed()
	{
		currentSpeed = GetUpdatedSpeed();

		foreach (IndividualAI member in members)
		{
			member.maxSpeed = currentSpeed;
		}
	}

	private float GetUpdatedSpeed()
	{
		int friendlyUnitsCount = members.Count;
		int enemyUnitsCount = GameManager.Instance.GetPossibleEnemies(tag).Length;
		int targetUnitsCount = GameManager.Instance.GetPossibleTargets(tag).Length;

		float lowFriendly = FuzzyHelper.GetLowDegree(friendlyUnitsCount, friendlyUnitsBounds);
		float highFriendly = FuzzyHelper.GetHighDegree(friendlyUnitsCount, friendlyUnitsBounds);
		float lowEnemy = FuzzyHelper.GetLowDegree(enemyUnitsCount, enemyUnitsBounds);
		float highEnemy = FuzzyHelper.GetHighDegree(enemyUnitsCount, enemyUnitsBounds);
		float lowTarget = FuzzyHelper.GetLowDegree(targetUnitsCount, targetUnitsBounds);
		float highTarget = FuzzyHelper.GetHighDegree(targetUnitsCount, targetUnitsBounds);

		float rule1 = FuzzyHelper.AND(highEnemy, highFriendly);
		float rule2 = FuzzyHelper.AND(highEnemy, lowFriendly);
		float rule3 = FuzzyHelper.OR(lowEnemy, highTarget);
		float rule4 = FuzzyHelper.OR(lowEnemy, lowTarget);
		float rule5 = FuzzyHelper.AND(FuzzyHelper.OR(highTarget, highFriendly), FuzzyHelper.NOT(highEnemy));
		float rule6 = FuzzyHelper.AND(FuzzyHelper.OR(lowTarget, highFriendly), FuzzyHelper.NOT(highEnemy));
		float rule7 = FuzzyHelper.OR(FuzzyHelper.OR(lowTarget, lowFriendly), lowEnemy);

		float value1 = averageSpeed;
		float value2 = aggressiveSpeed;
		float value3 = averageSpeed;
		float value4 = calmSpeed;
		float value5 = aggressiveSpeed;
		float value6 = averageSpeed;
		float value7 = calmSpeed;

		float numerator =
			rule1 * value1 +
			rule2 * value2 +
			rule3 * value3 +
			rule4 * value4 +
			rule5 * value5 +
			rule6 * value6 +
			rule7 * value7;

		float denominator = rule1 + rule2 + rule3 + rule4 + rule5 + rule6 + rule7;

		return numerator / denominator;
	}

}
