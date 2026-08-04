using System;

namespace TeamCherry.Localization
{

[Serializable]
public struct LocalisedString : IEquatable<LocalisedString>
{
	public class NotRequiredAttribute : Attribute
	{
	}

	public class NoKeyValidation : Attribute
	{
	}

	public string Sheet;

	public string Key;

	public bool Exists
	{
		get
		{
			if (string.IsNullOrEmpty(Sheet) || string.IsNullOrEmpty(Key))
			{
				return false;
			}
			return Language.Has(Key, Sheet);
		}
	}

	public bool SheetExists
	{
		get
		{
			if (!string.IsNullOrEmpty(Sheet))
			{
				return Language.HasSheet(Sheet);
			}
			return false;
		}
	}

	public bool IsEmpty
	{
		get
		{
			if (string.IsNullOrEmpty(Sheet))
			{
				return string.IsNullOrEmpty(Key);
			}
			return false;
		}
	}

	public LocalisedString(string sheet, string key)
	{
		Sheet = sheet;
		Key = key;
	}

	public override string ToString()
	{
		return ToString(allowBlankText: true);
	}

	public string ToString(bool allowBlankText)
	{
		if (!Exists)
		{
			return "!!" + Sheet + "/" + Key + "!!";
		}
		string text = ReplaceTags(Language.Get(Key, Sheet));
		if (string.IsNullOrWhiteSpace(text) && !allowBlankText)
		{
			return "BLANK CELL (" + Sheet + "/" + Key + ")";
		}
		return text;
	}

	public static string ReplaceTags(string source)
	{
		return source.Replace("<br>", "\n");
	}

	public static implicit operator string(LocalisedString s)
	{
		return s.ToString();
	}

	public bool Equals(LocalisedString other)
	{
		if (Sheet == other.Sheet)
		{
			return Key == other.Key;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is LocalisedString other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Sheet, Key);
	}
}
}
