using System.Collections.Generic;

public static class FactionTags
{
	public const string _Rock = "Rock";
	public const string _Paper = "Paper";
	public const string _Scissor = "Scissor";
	public const string _Lizard = "Lizard";
	public const string _Spock = "Spock";

	private static readonly Dictionary<string, HashSet<string>> Beats = new()
	{
		{ _Rock, new() { _Scissor, _Lizard } },
		{ _Paper, new() { _Rock, _Spock } },
		{ _Scissor, new() { _Paper, _Lizard } },
		{ _Lizard, new() { _Spock, _Paper } },
		{ _Spock, new() { _Scissor, _Rock } }
	};

	public static bool BeatsFaction(this string attacker, string target)
	{
		return Beats.TryGetValue(attacker, out var targets) && targets.Contains(target);
	}

}
