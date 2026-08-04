using UnityEngine;

namespace TeamCherry.Localization
{

public abstract class LocalizationProjectSettingsBase : ScriptableObject
{
	public abstract bool TryGetSavedLanguageCode(out string languageCode);

	public abstract SystemLanguage GetSystemLanguage();

	public abstract void OnSwitchedLanguage(LanguageCode newLang);

	public virtual bool CanPullSheet(string sheetTitle)
	{
		return true;
	}

	public virtual bool CanPullText(string sheetTitle, string key)
	{
		return true;
	}

	public virtual bool ShouldCheckText(string sheetTitle, string key)
	{
		return true;
	}

	public virtual bool IsTextOverflowing(string sheetTitle, string text)
	{
		return false;
	}
}
}
