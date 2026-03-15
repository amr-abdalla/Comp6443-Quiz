using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	private GroupAI[] groups;
	[SerializeField] private CharactersSpawner sawSpawner;

	private void OnEnable()
	{
		groups = GetComponentsInChildren<GroupAI>();
		SpawnAll();
	}

	private void SpawnAll()
	{
		foreach(GroupAI group in groups)
		{
			group.SpawnAndInit();
		}

		foreach (GroupAI group in groups)
		{
			group.UpdateFactionSpeed();
		}

		sawSpawner.StartSpawning(sawSpawner.transform);

	}

	public GroupAI GetGroupOfTag(string tag)
	{
		return groups.FirstOrDefault(group => group.factionTag.Equals(tag));
	}

	public IndividualAI[] GetPossibleTargets(string attackerTag)
	{
		return groups
			.Where(group => FactionTags.BeatsFaction(attackerTag, group.factionTag))
			.SelectMany(group => group.members)
			.ToArray();
	}

	public IndividualAI[] GetPossibleEnemies(string attackerTag)
	{
		return groups
			.Where(group => FactionTags.BeatsFaction(group.factionTag, attackerTag))
			.SelectMany(group => group.members)
			.ToArray();
	}

}
