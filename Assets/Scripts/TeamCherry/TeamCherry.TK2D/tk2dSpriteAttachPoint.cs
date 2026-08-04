using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[AddComponentMenu("2D Toolkit/Sprite/tk2dSpriteAttachPoint")]
public class tk2dSpriteAttachPoint : MonoBehaviour
{
	private tk2dBaseSprite sprite;

	public List<Transform> attachPoints = new List<Transform>();

	private static bool[] attachPointUpdated = new bool[32];

	public bool deactivateUnusedAttachPoints;

	private Dictionary<Transform, string> cachedInstanceNames = new Dictionary<Transform, string>();

	private void Awake()
	{
		if ((Object)(object)sprite == (Object)null)
		{
			sprite = ((Component)this).GetComponent<tk2dBaseSprite>();
			if ((Object)(object)sprite != (Object)null)
			{
				HandleSpriteChanged(sprite);
			}
		}
	}

	private void OnEnable()
	{
		if ((Object)(object)sprite != (Object)null)
		{
			sprite.SpriteChanged += HandleSpriteChanged;
		}
	}

	private void OnDisable()
	{
		if ((Object)(object)sprite != (Object)null)
		{
			sprite.SpriteChanged -= HandleSpriteChanged;
		}
	}

	private void UpdateAttachPointTransform(tk2dSpriteDefinition.AttachPoint attachPoint, Transform t)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		t.localPosition = Vector3.Scale(attachPoint.position, sprite.scale);
		t.localScale = sprite.scale;
		float num = Mathf.Sign(sprite.scale.x) * Mathf.Sign(sprite.scale.y);
		t.localEulerAngles = new Vector3(0f, 0f, attachPoint.angle * num);
	}

	private string GetInstanceName(Transform t)
	{
		string value = "";
		if (cachedInstanceNames.TryGetValue(t, out value))
		{
			return value;
		}
		cachedInstanceNames[t] = ((Object)t).name;
		return ((Object)t).name;
	}

	private void HandleSpriteChanged(tk2dBaseSprite spr)
	{
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		tk2dSpriteDefinition currentSprite = spr.CurrentSprite;
		if (currentSprite == null)
		{
			Debug.LogErrorFormat("Sprite Definition was null for GameObject: {0}", new object[1] { ((Object)((Component)spr).gameObject).name });
			return;
		}
		if (currentSprite.attachPoints == null)
		{
			Debug.LogErrorFormat("attachPoints was null for SpriteDefinition: {0}, GameObject: {1}", new object[2]
			{
				currentSprite.name,
				((Object)((Component)spr).gameObject).name
			});
			return;
		}
		int num = Mathf.Max(currentSprite.attachPoints.Length, attachPoints.Count);
		if (num > attachPointUpdated.Length)
		{
			attachPointUpdated = new bool[num];
		}
		tk2dSpriteDefinition.AttachPoint[] array = currentSprite.attachPoints;
		foreach (tk2dSpriteDefinition.AttachPoint attachPoint in array)
		{
			bool flag = false;
			int num2 = 0;
			for (int j = 0; j < attachPoints.Count; j++)
			{
				Transform val = attachPoints[j];
				if ((Object)(object)val != (Object)null && GetInstanceName(val) == attachPoint.name)
				{
					attachPointUpdated[num2] = true;
					UpdateAttachPointTransform(attachPoint, val);
					flag = true;
				}
				num2++;
			}
			if (!flag)
			{
				Transform transform = new GameObject(attachPoint.name).transform;
				transform.parent = ((Component)this).transform;
				UpdateAttachPointTransform(attachPoint, transform);
				attachPointUpdated[attachPoints.Count] = true;
				attachPoints.Add(transform);
			}
		}
		if (!deactivateUnusedAttachPoints)
		{
			return;
		}
		for (int k = 0; k < attachPoints.Count; k++)
		{
			if ((Object)(object)attachPoints[k] != (Object)null)
			{
				GameObject gameObject = ((Component)attachPoints[k]).gameObject;
				if (attachPointUpdated[k] && !gameObject.activeSelf)
				{
					gameObject.SetActive(true);
				}
				else if (!attachPointUpdated[k] && gameObject.activeSelf)
				{
					gameObject.SetActive(false);
				}
			}
			attachPointUpdated[k] = false;
		}
	}
}
