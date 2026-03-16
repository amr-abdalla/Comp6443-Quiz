using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
	[SerializeField] private GroupAI groupAI;
	private TextMeshProUGUI textMeshPro;

	private void Awake()
	{
		textMeshPro = GetComponent<TextMeshProUGUI>();
	}

	private void Update()
	{
		if (groupAI.members.Count == 0)
		{
			return;
		}

		textMeshPro.text =
			$"{groupAI.tag}:\n" +
			$"Count = {groupAI.members.Count}\n" +
			$"Speed = {groupAI.members[0].maxSpeed:F1}\n";
	}

}
