namespace TeamCherry.Localization
{

public static class StringExtensions
{
	public static string UnescapeXml(this string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return s;
		}
		return s.Replace("&apos;", "'").Replace("&quot;", "\"").Replace("&gt;", ">")
			.Replace("&lt;", "<")
			.Replace("&amp;", "&");
	}
}
}
