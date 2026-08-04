using UnityEngine;

namespace TeamCherry.Localization
{

public class LocalizedAsset : MonoBehaviour
{
	public Object localizeTarget;

	public void Awake()
	{
		LocalizeAsset(localizeTarget);
	}

	public void LocalizeAsset()
	{
		LocalizeAsset(localizeTarget);
	}

	public static void LocalizeAsset(Object target)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		if (target == (Object)null)
		{
			Debug.LogError((object)"LocalizedAsset target is null");
		}
		else if (((object)target).GetType() == typeof(Material))
		{
			Material val = (Material)target;
			if ((Object)(object)val.mainTexture != (Object)null)
			{
				Texture val2 = (Texture)Language.GetAsset(((Object)val.mainTexture).name);
				if ((Object)(object)val2 != (Object)null)
				{
					val.mainTexture = val2;
				}
			}
		}
		else if (((object)target).GetType() == typeof(MeshRenderer))
		{
			MeshRenderer val3 = (MeshRenderer)target;
			if ((Object)(object)((Renderer)val3).material.mainTexture != (Object)null)
			{
				Texture val4 = (Texture)Language.GetAsset(((Object)((Renderer)val3).material.mainTexture).name);
				if ((Object)(object)val4 != (Object)null)
				{
					((Renderer)val3).material.mainTexture = val4;
				}
			}
		}
		else
		{
			Debug.LogError((object)("Could not localize this object type: " + ((object)target).GetType()));
		}
	}
}
}
