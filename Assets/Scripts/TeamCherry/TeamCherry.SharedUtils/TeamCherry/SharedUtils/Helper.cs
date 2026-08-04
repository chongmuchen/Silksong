using System.Linq;

namespace TeamCherry.SharedUtils
{

public static class Helper
{
	public static bool CheckMatchingSearchFilter(string text, string filter)
	{
		text = text.ToLower();
		filter = filter.ToLower().Replace('_', ' ');
		return filter.Split(' ').All((string f) => text.Contains(f));
	}
}
}
