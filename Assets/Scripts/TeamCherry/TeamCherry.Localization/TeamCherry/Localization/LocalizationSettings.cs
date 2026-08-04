using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TeamCherry.Localization
{

[Serializable]
public class LocalizationSettings : ScriptableObject
{
	[Serializable]
	public struct CharListRef
	{
		public LanguageCode LangCode;

		public TextAsset TextAsset;
	}

	public string[] sheetTitles;

	public bool useSystemLanguagePerDefault = true;

	public string defaultLangCode = "EN";

	public string gDocsId;

	public CharListRef[] charListRefs;

	public static LanguageCode GetLanguageEnum(string langCode)
	{
		langCode = langCode.ToUpper();
		foreach (LanguageCode value in Enum.GetValues(typeof(LanguageCode)))
		{
			if (value.ToString().Equals(langCode, StringComparison.InvariantCultureIgnoreCase))
			{
				return value;
			}
		}
		Debug.LogError((object)("ERORR: There is no language: [" + langCode + "]"));
		return LanguageCode.EN;
	}
}
}
