using UnityEngine;

namespace TeamCherry.Localization;

internal class LocalizationProjectSettingsDefault : LocalizationProjectSettingsBase
{
	public override bool TryGetSavedLanguageCode(out string languageCode)
	{
		languageCode = LanguageCode.EN.ToString();
		return true;
	}

	public override SystemLanguage GetSystemLanguage()
	{
		return (SystemLanguage)10;
	}

	public override void OnSwitchedLanguage(LanguageCode newLang)
	{
	}
}
