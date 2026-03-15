using System.Linq;

public class GameManager : Singleton<GameManager>
{
	private GroupAI[] groups;

	private void OnEnable()
	{
		groups = GetComponentsInChildren<GroupAI>();
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
