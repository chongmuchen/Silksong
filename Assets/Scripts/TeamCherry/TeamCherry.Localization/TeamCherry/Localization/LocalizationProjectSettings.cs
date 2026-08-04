using UnityEngine;

namespace TeamCherry.Localization
{

public static class LocalizationProjectSettings
{
	public const string RESOURCE_PATH = "LocalizationProjectSettings";

	private static LocalizationProjectSettingsDefault _defaultSettings;

	private static LocalizationProjectSettingsBase Get()
	{
		LocalizationProjectSettingsBase localizationProjectSettingsBase = Resources.Load<LocalizationProjectSettingsBase>("LocalizationProjectSettings");
		if ((Object)(object)localizationProjectSettingsBase != (Object)null)
		{
			return localizationProjectSettingsBase;
		}
		Debug.LogError((object)"Could not load Localisation Project settings asset at path: LocalizationProjectSettings");
		if (!(_defaultSettings != null))
		{
			_defaultSettings = ScriptableObject.CreateInstance<LocalizationProjectSettingsDefault>();
		}
		return _defaultSettings;
	}

	public static bool TryGetSavedLanguageCode(out string languageCode)
	{
		return Get().TryGetSavedLanguageCode(out languageCode);
	}

	public static SystemLanguage GetSystemLanguage()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return Get().GetSystemLanguage();
	}

	public static void OnSwitchedLanguage(LanguageCode newLang)
	{
		Get().OnSwitchedLanguage(newLang);
	}

	public static bool CanPullSheet(string sheetTitle)
	{
		return Get().CanPullSheet(sheetTitle);
	}

	public static bool CanPullText(string sheetTitle, string key)
	{
		return Get().CanPullText(sheetTitle, key);
	}

	public static bool ShouldCheckText(string sheetTitle, string key)
	{
		return Get().ShouldCheckText(sheetTitle, key);
	}

	public static bool IsTextOverflowing(string sheetTitle, string text)
	{
		return Get().IsTextOverflowing(sheetTitle, text);
	}
}
}
