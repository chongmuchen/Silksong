using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using TeamCherry.SharedUtils;
using UnityEngine;

namespace TeamCherry.Localization;

public static class Language
{
	private const string SETTINGS_ASSET_PATH = "Assets/Localization/Resources/Languages/LocalizationSettings.asset";

	private static LocalizationSettings _settings;

	private static List<string> _availableLanguages;

	private static LanguageCode _currentLanguage;

	private static Dictionary<string, Dictionary<string, string>> _currentEntrySheets;

	private static LocalizationSettings Settings
	{
		get
		{
			if (Application.isPlaying && (Object)(object)_settings != (Object)null)
			{
				return _settings;
			}
			_settings = (LocalizationSettings)(object)Resources.Load("Languages/" + Path.GetFileNameWithoutExtension("Assets/Localization/Resources/Languages/LocalizationSettings.asset"), typeof(LocalizationSettings));
			return _settings;
		}
	}

	static Language()
	{
		LoadAvailableLanguages();
		LoadLanguage();
	}

	public static void LoadLanguage()
	{
		string text = RestoreLanguageSelection();
		Debug.LogFormat("Restored language code '{0}'", new object[1] { text });
		SwitchLanguage(text);
	}

	private static string RestoreLanguageSelection()
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		if (LocalizationProjectSettings.TryGetSavedLanguageCode(out var languageCode))
		{
			Debug.LogFormat("Loaded saved language code '{0}'", new object[1] { languageCode });
			if (_availableLanguages.Contains(languageCode))
			{
				return languageCode;
			}
			Debug.LogErrorFormat("Loaded saved language code '{0}' is not an available language", new object[1] { languageCode });
		}
		if (Settings.useSystemLanguagePerDefault)
		{
			SystemLanguage systemLanguage = LocalizationProjectSettings.GetSystemLanguage();
			Debug.LogFormat("Loaded system language '{0}'", new object[1] { systemLanguage });
			string text = LanguageNameToCode(systemLanguage).ToString();
			Debug.LogFormat("Loaded system language code '{0}'", new object[1] { text });
			if (_availableLanguages.Contains(text))
			{
				return text;
			}
			Debug.LogErrorFormat("System language code '{0}' is not an available language", new object[1] { text });
		}
		Debug.LogFormat("Falling back to default language code '{0}'", new object[1] { Settings.defaultLangCode });
		return LocalizationSettings.GetLanguageEnum(Settings.defaultLangCode).ToString();
	}

	public static void LoadAvailableLanguages()
	{
		_availableLanguages = new List<string>();
		if (Settings.sheetTitles == null || Settings.sheetTitles.Length == 0)
		{
			Debug.Log((object)"None available");
			return;
		}
		foreach (LanguageCode value in Enum.GetValues(typeof(LanguageCode)))
		{
			if (HasLanguageFile(value.ToString() ?? "", Settings.sheetTitles[0]))
			{
				_availableLanguages.Add(value.ToString() ?? "");
			}
		}
		StringBuilder stringBuilder = new StringBuilder("Discovered supported languages: ");
		for (int i = 0; i < _availableLanguages.Count; i++)
		{
			stringBuilder.Append(_availableLanguages[i]);
			if (i < _availableLanguages.Count - 1)
			{
				stringBuilder.Append(", ");
			}
		}
		Debug.Log((object)stringBuilder.ToString());
		Resources.UnloadUnusedAssets();
	}

	public static string[] GetLanguages()
	{
		return _availableLanguages.ToArray();
	}

	public static bool SwitchLanguage(string langCode)
	{
		return SwitchLanguage(LocalizationSettings.GetLanguageEnum(langCode));
	}

	public static bool SwitchLanguage(LanguageCode code)
	{
		if (_availableLanguages.Contains(code.ToString() ?? ""))
		{
			DoSwitch(code);
			return true;
		}
		Debug.LogError((object)("Could not switch from language " + _currentLanguage.ToString() + " to " + code));
		if (_currentLanguage != LanguageCode.N)
		{
			return false;
		}
		if (_availableLanguages.Count > 0)
		{
			DoSwitch(LocalizationSettings.GetLanguageEnum(_availableLanguages[0]));
			Debug.LogError((object)("Switched to " + _currentLanguage.ToString() + " instead"));
		}
		else
		{
			Debug.LogError((object)("Please verify that you have the file: Resources/Languages/" + code));
			Debug.Break();
		}
		return false;
	}

	private static void DoSwitch(LanguageCode newLang)
	{
		LocalizationProjectSettings.OnSwitchedLanguage(newLang);
		_currentLanguage = newLang;
		_currentEntrySheets = new Dictionary<string, Dictionary<string, string>>();
		string[] sheetTitles = Settings.sheetTitles;
		foreach (string text in sheetTitles)
		{
			_currentEntrySheets[text] = new Dictionary<string, string>();
			string languageFileContents = GetLanguageFileContents(text);
			if (string.IsNullOrEmpty(languageFileContents))
			{
				continue;
			}
			using XmlReader xmlReader = XmlReader.Create(new StringReader(languageFileContents));
			while (xmlReader.ReadToFollowing("entry"))
			{
				xmlReader.MoveToFirstAttribute();
				string value = xmlReader.Value;
				xmlReader.MoveToElement();
				string s = xmlReader.ReadElementContentAsString().Trim();
				s = s.UnescapeXml();
				_currentEntrySheets[text][value] = s;
			}
		}
		LocalizedAsset[] array = (LocalizedAsset[])(object)Object.FindObjectsOfType(typeof(LocalizedAsset));
		for (int i = 0; i < array.Length; i++)
		{
			array[i].LocalizeAsset();
		}
		SendMonoMessage("ChangedLanguage", _currentLanguage);
	}

	public static Object GetAsset(string name)
	{
		return Resources.Load("Languages/Assets/" + CurrentLanguage().ToString() + "/" + name);
	}

	private static bool HasLanguageFile(string lang, string sheetTitle)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		return (Object)(TextAsset)Resources.Load("Languages/" + lang + "_" + sheetTitle, typeof(TextAsset)) != (Object)null;
	}

	private static string GetLanguageFileContents(string sheetTitle)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		TextAsset val = (TextAsset)Resources.Load("Languages/" + _currentLanguage.ToString() + "_" + sheetTitle, typeof(TextAsset));
		if ((Object)(object)val == (Object)null)
		{
			return string.Empty;
		}
		return Encryption.Decrypt(val.text);
	}

	public static LanguageCode CurrentLanguage()
	{
		return _currentLanguage;
	}

	public static string Get(string key)
	{
		return Get(key, Settings.sheetTitles[0]);
	}

	public static string Get(string key, string sheetTitle)
	{
		if (_currentEntrySheets == null || !_currentEntrySheets.ContainsKey(sheetTitle))
		{
			Debug.LogError((object)("The sheet with title \"" + sheetTitle + "\" does not exist!"));
			return "";
		}
		if (_currentEntrySheets[sheetTitle].ContainsKey(key))
		{
			return _currentEntrySheets[sheetTitle][key];
		}
		return "#!#" + key + "#!#";
	}

	public static IEnumerable<string> GetSheets()
	{
		return _currentEntrySheets.Keys;
	}

	public static IEnumerable<string> GetKeys(string sheetTitle)
	{
		if (HasSheet(sheetTitle))
		{
			return _currentEntrySheets[sheetTitle].Select((KeyValuePair<string, string> kvp) => kvp.Key);
		}
		return Enumerable.Empty<string>();
	}

	public static bool Has(string key)
	{
		return Has(key, Settings.sheetTitles[0]);
	}

	public static bool Has(string key, string sheetTitle)
	{
		if (_currentEntrySheets == null || !_currentEntrySheets.ContainsKey(sheetTitle))
		{
			return false;
		}
		return _currentEntrySheets[sheetTitle].ContainsKey(key);
	}

	public static bool HasSheet(string sheetTitle)
	{
		if (_currentEntrySheets != null)
		{
			return _currentEntrySheets.ContainsKey(sheetTitle);
		}
		return false;
	}

	private static void SendMonoMessage(string methodString, params object[] parameters)
	{
		if (parameters != null && parameters.Length > 1)
		{
			Debug.LogError((object)"We cannot pass more than one argument currently!");
		}
		GameObject[] array = (GameObject[])(object)Object.FindObjectsOfType(typeof(GameObject));
		foreach (GameObject val in array)
		{
			if (Object.op_Implicit((Object)(object)val) && !((Object)(object)val.transform.parent != (Object)null))
			{
				if (parameters != null && parameters.Length == 1)
				{
					val.gameObject.BroadcastMessage(methodString, parameters[0], (SendMessageOptions)1);
				}
				else
				{
					val.gameObject.BroadcastMessage(methodString, (SendMessageOptions)1);
				}
			}
		}
	}

	public static LanguageCode LanguageNameToCode(SystemLanguage name)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected I4, but got Unknown
		return (int)name switch
		{
			0 => LanguageCode.AF, 
			1 => LanguageCode.AR, 
			2 => LanguageCode.BA, 
			3 => LanguageCode.BE, 
			4 => LanguageCode.BG, 
			5 => LanguageCode.CA, 
			6 => LanguageCode.ZH, 
			7 => LanguageCode.CS, 
			8 => LanguageCode.DA, 
			9 => LanguageCode.NL, 
			10 => LanguageCode.EN, 
			11 => LanguageCode.ET, 
			12 => LanguageCode.FA, 
			13 => LanguageCode.FI, 
			14 => LanguageCode.FR, 
			15 => LanguageCode.DE, 
			16 => LanguageCode.EL, 
			17 => LanguageCode.HE, 
			18 => LanguageCode.HU, 
			19 => LanguageCode.IS, 
			20 => LanguageCode.ID, 
			21 => LanguageCode.IT, 
			22 => LanguageCode.JA, 
			23 => LanguageCode.KO, 
			24 => LanguageCode.LA, 
			25 => LanguageCode.LT, 
			26 => LanguageCode.NO, 
			27 => LanguageCode.PL, 
			28 => LanguageCode.PT, 
			29 => LanguageCode.RO, 
			30 => LanguageCode.RU, 
			31 => LanguageCode.SH, 
			32 => LanguageCode.SK, 
			33 => LanguageCode.SL, 
			34 => LanguageCode.ES, 
			35 => LanguageCode.SW, 
			36 => LanguageCode.TH, 
			37 => LanguageCode.TR, 
			38 => LanguageCode.UK, 
			39 => LanguageCode.VI, 
			40 => LanguageCode.ZH, 
			41 => LanguageCode.ZH_TW, 
			43 => LanguageCode.N, 
			_ => LanguageCode.N, 
		};
	}
}
