using UnityEngine;

public class Tk2dSpriteSetKeywords : MonoBehaviour
{
	[SerializeField]
	private tk2dSprite sprite;

	[SerializeField]
	private string[] keywords;

	private void Reset()
	{
		sprite = ((Component)this).GetComponent<tk2dSprite>();
	}

	private void Awake()
	{
		if ((Object)(object)sprite == (Object)null)
		{
			sprite = ((Component)this).GetComponent<tk2dSprite>();
			if ((Object)(object)sprite == (Object)null)
			{
				((Behaviour)this).enabled = false;
				return;
			}
		}
		string[] array = keywords;
		foreach (string keyword in array)
		{
			sprite.EnableKeyword(keyword);
		}
	}
}
