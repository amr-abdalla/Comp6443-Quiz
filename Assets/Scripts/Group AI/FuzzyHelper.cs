using UnityEngine;

public static class FuzzyHelper
{
	public static float GetLowDegree(float value, Vector2Int bounds)
	{
		if (value <= bounds.x) return 1;
		if (value >= bounds.y) return 0;
		return (bounds.y - value) / (bounds.y - bounds.x);
	}

	public static float GetHighDegree(float value, Vector2Int bounds)
	{
		if (value <= bounds.x) return 0;
		if (value >= bounds.y) return 1;
		return (value - bounds.x) / (bounds.y - bounds.x);
	}

	public static float AND(float a, float b) => Mathf.Min(a, b);
	public static float OR(float a, float b) => Mathf.Max(a, b);
	public static float NOT(float a) => 1f - a;
}
