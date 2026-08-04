#if PLAYMAKER
using System;
using HutongGames.PlayMaker;

namespace TeamCherry.Localization
{

[Serializable]
public class LocalisedFsmString
{
	public FsmString Sheet;

	public FsmString Key;

	public static implicit operator LocalisedString(LocalisedFsmString s)
	{
		return new LocalisedString(s.Sheet.Value, s.Key.Value);
	}

	public static implicit operator string(LocalisedFsmString s)
	{
		return new LocalisedString(s.Sheet.Value, s.Key.Value);
	}
}
}

#endif
