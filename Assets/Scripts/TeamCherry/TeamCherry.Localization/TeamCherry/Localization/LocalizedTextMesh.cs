using UnityEngine;

namespace TeamCherry.Localization
{

[RequireComponent(typeof(TextMesh))]
public class LocalizedTextMesh : MonoBehaviour
{
	public string keyValue;

	public void Awake()
	{
		LocalizeTextMesh(keyValue);
	}

	public void LocalizeTextMesh(string newKeyValue)
	{
		if (newKeyValue == null)
		{
			Debug.LogError((object)("Please set the KeyValue that should be used for this TextMesh (" + ((Object)this).name + ")"));
		}
		else
		{
			((Component)this).gameObject.GetComponent<TextMesh>().text = Language.Get(newKeyValue);
		}
	}
}
}
