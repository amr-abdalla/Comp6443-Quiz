using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	private GroupAI[] groups;
	[SerializeField] private CharactersSpawner sawSpawner;
	[SerializeField] private CharactersSpawner jumpSpawner;
	public GroupAI PlayerGroup { get; private set; }

	private void OnEnable()
	{
		groups = GetComponentsInChildren<GroupAI>();
	}

	public void SetPlayerFaction(string factionTag)
	{
		PlayerGroup = groups.FirstOrDefault(group => group.CompareTag(factionTag));
		PlayerGroup.AddComponent<PlayerSpawner>();
	}

	public void SpawnAll()
	{
		foreach(GroupAI group in groups)
		{
			if (group == PlayerGroup)
			{
				continue;
			}

			group.SpawnAllMembers();
		}

		sawSpawner.StartSpawning(sawSpawner.transform);
		jumpSpawner.StartSpawning(jumpSpawner.transform);
	}

	public void initAll()
	{
		foreach (GroupAI group in groups)
		{
			group.init();
		}
	}

	public void StartMoving()
	{
		foreach (GroupAI group in groups)
		{
			group.StartMoving();
		}
	}

	public bool isReady() => groups.All(group => group.isReady);

	public GroupAI GetGroupOfTag(string tag)
	{
		return groups.FirstOrDefault(group => group.CompareTag(tag));
	}

	public IndividualAI[] GetPossibleTargets(string attackerTag)
	{
		return groups
			.Where(group => FactionTags.BeatsFaction(attackerTag, group.tag))
			.SelectMany(group => group.members)
			.ToArray();
	}

	public IndividualAI[] GetPossibleEnemies(string attackerTag)
	{
		return groups
			.Where(group => FactionTags.BeatsFaction(group.tag, attackerTag))
			.SelectMany(group => group.members)
			.ToArray();
	}

}
