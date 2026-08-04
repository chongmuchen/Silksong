using System;
using UnityEngine;

[Serializable]
public class tk2dSpriteCollectionPlatform
{
	public string name = "";

	public tk2dSpriteCollection spriteCollection;

	public bool Valid
	{
		get
		{
			if (name.Length > 0)
			{
				return (Object)(object)spriteCollection != (Object)null;
			}
			return false;
		}
	}

	public void CopyFrom(tk2dSpriteCollectionPlatform source)
	{
		name = source.name;
		spriteCollection = source.spriteCollection;
	}
}
