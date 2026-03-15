using UnityEngine;
using UnityEngine.UI;

public class BoostUI : MonoBehaviour
{
	[SerializeField] private Slider boostSlider;
	[SerializeField] private IndividualAI individualAI;
	[SerializeField] private Color nullStateColor;
	[SerializeField] private Color fleeStateColor;
	[SerializeField] private Color seekStateColor;

	private void Update()
	{
		boostSlider.value = individualAI.GetBoostPercentage();
		UpdateSliderColor();
	}

	private void UpdateSliderColor()
	{
		State<IndividualAI> currentState = individualAI.GetCurrentState();

		ColorBlock colors = boostSlider.colors;

		if (currentState == null)
		{
			colors.disabledColor = nullStateColor;
		}
		else if (currentState.GetType() == typeof(Flee))
		{
			colors.disabledColor = fleeStateColor;

		}
		else if (currentState.GetType() == typeof(Seek))
		{
			colors.disabledColor = seekStateColor;
		}

		boostSlider.colors = colors;
	}
}
