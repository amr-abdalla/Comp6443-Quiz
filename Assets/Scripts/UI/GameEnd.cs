using TMPro;
using UnityEngine;

public class GameEnd : Singleton<GameEnd>
{
	[SerializeField] private TextMeshProUGUI textMeshPro;
	[SerializeField] private GameObject UI;

    public void ToggleEndGame(string losingFaction)
	{
		UI.SetActive(true);

		if (GameManager.Instance.PlayerGroup.tag == losingFaction)
		{
			textMeshPro.text = $"{losingFaction} has Lost!\n" + " You Lost";
		}
		else
		{
			textMeshPro.text = $"{losingFaction} has Lost!\n" + " You Win";
		}

		GamePauser.canPause = false;
		Time.timeScale = 0;
	}
}
