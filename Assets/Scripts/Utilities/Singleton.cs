using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				throw new System.Exception($"No Instance of {typeof(T)} found");
			}

			return instance;
		}
	}

	protected virtual void Awake()
	{
		if (instance == null)
		{
			instance = this as T;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

}