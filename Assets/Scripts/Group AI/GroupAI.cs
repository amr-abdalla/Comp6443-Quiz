using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroupAI : MonoBehaviour
{
	[SerializeField] private float calmSpeed = 2f;
	[SerializeField] private float averageSpeed = 6f;
	[SerializeField] private float aggressiveSpeed = 10f;

	private float currentSpeed;
	public List<IndividualAI> members { get; private set; }
	public string factionTag { get; private set; }

	void Start()
	{
		members = GetComponentsInChildren<IndividualAI>().ToList();
		factionTag = members[0].gameObject.tag;

		foreach (IndividualAI member in members)
		{
			member.OnDeath += OnMemberDeath;
		}

		UpdateFactionSpeed();
	}

	private void OnMemberDeath(IndividualAI member)
	{
		members.Remove(member);

		if (members.Count == 0)
		{
			// Completely Died
			return;
		}

		UpdateFactionSpeed();
	}

	private void UpdateFactionSpeed()
	{
		currentSpeed = aggressiveSpeed;

		foreach (IndividualAI member in members)
		{
			member.speed = currentSpeed;
		}

	}
}
