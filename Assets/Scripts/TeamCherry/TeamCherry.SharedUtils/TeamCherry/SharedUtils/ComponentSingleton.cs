using UnityEngine;

namespace TeamCherry.SharedUtils;

[ExecuteInEditMode]
public abstract class ComponentSingleton<T> : MonoBehaviour where T : ComponentSingleton<T>
{
	private static T s_Instance;

	public static bool Exists => (Object)(object)s_Instance != (Object)null;

	public static T Instance
	{
		get
		{
			if ((Object)(object)s_Instance == (Object)null)
			{
				s_Instance = FindInstance() ?? CreateNewSingleton();
			}
			return s_Instance;
		}
	}

	private static T FindInstance()
	{
		return Object.FindObjectOfType<T>();
	}

	protected virtual string GetGameObjectName()
	{
		return typeof(T).Name;
	}

	private static T CreateNewSingleton()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GameObject val = new GameObject();
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad((Object)(object)val);
			((Object)val).hideFlags = (HideFlags)52;
		}
		else
		{
			((Object)val).hideFlags = (HideFlags)61;
		}
		T val2 = val.AddComponent<T>();
		((Object)val).name = val2.GetGameObjectName();
		return val2;
	}

	private void Awake()
	{
		if ((Object)(object)s_Instance != (Object)null && (Object)(object)s_Instance != (Object)(object)this)
		{
			Object.DestroyImmediate((Object)(object)((Component)this).gameObject);
		}
		else
		{
			s_Instance = this as T;
		}
	}

	public static void DestroySingleton()
	{
		if (Exists)
		{
			Object.DestroyImmediate((Object)(object)((Component)Instance).gameObject);
			s_Instance = null;
		}
	}
}
