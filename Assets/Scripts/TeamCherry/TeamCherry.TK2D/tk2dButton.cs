using System.Collections;
using UnityEngine;

[AddComponentMenu("2D Toolkit/Deprecated/GUI/tk2dButton")]
public class tk2dButton : MonoBehaviour
{
	public delegate void ButtonHandlerDelegate(tk2dButton source);

	public Camera viewCamera;

	public string buttonDownSprite = "button_down";

	public string buttonUpSprite = "button_up";

	public string buttonPressedSprite = "button_up";

	private int buttonDownSpriteId = -1;

	private int buttonUpSpriteId = -1;

	private int buttonPressedSpriteId = -1;

	public AudioClip buttonDownSound;

	public AudioClip buttonUpSound;

	public AudioClip buttonPressedSound;

	public GameObject targetObject;

	public string messageName = "";

	private tk2dBaseSprite sprite;

	private bool buttonDown;

	public float targetScale = 1.1f;

	public float scaleTime = 0.05f;

	public float pressedWaitTime = 0.3f;

	public event ButtonHandlerDelegate ButtonPressedEvent;

	public event ButtonHandlerDelegate ButtonAutoFireEvent;

	public event ButtonHandlerDelegate ButtonDownEvent;

	public event ButtonHandlerDelegate ButtonUpEvent;

	private void OnEnable()
	{
		buttonDown = false;
	}

	private void Start()
	{
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)viewCamera == (Object)null)
		{
			Transform val = ((Component)this).transform;
			while (Object.op_Implicit((Object)(object)val) && (Object)(object)((Component)val).GetComponent<Camera>() == (Object)null)
			{
				val = val.parent;
			}
			if (Object.op_Implicit((Object)(object)val) && (Object)(object)((Component)val).GetComponent<Camera>() != (Object)null)
			{
				viewCamera = ((Component)val).GetComponent<Camera>();
			}
			if ((Object)(object)viewCamera == (Object)null && Object.op_Implicit((Object)(object)tk2dCamera.Instance))
			{
				viewCamera = ((Component)tk2dCamera.Instance).GetComponent<Camera>();
			}
			if ((Object)(object)viewCamera == (Object)null)
			{
				viewCamera = Camera.main;
			}
		}
		sprite = ((Component)this).GetComponent<tk2dBaseSprite>();
		if (Object.op_Implicit((Object)(object)sprite))
		{
			UpdateSpriteIds();
		}
		if ((Object)(object)((Component)this).GetComponent<Collider>() == (Object)null)
		{
			BoxCollider obj = ((Component)this).gameObject.AddComponent<BoxCollider>();
			Vector3 size = obj.size;
			size.z = 0.2f;
			obj.size = size;
		}
		if (((Object)(object)buttonDownSound != (Object)null || (Object)(object)buttonPressedSound != (Object)null || (Object)(object)buttonUpSound != (Object)null) && (Object)(object)((Component)this).GetComponent<AudioSource>() == (Object)null)
		{
			((Component)this).gameObject.AddComponent<AudioSource>().playOnAwake = false;
		}
	}

	public void UpdateSpriteIds()
	{
		buttonDownSpriteId = ((buttonDownSprite.Length > 0) ? sprite.GetSpriteIdByName(buttonDownSprite) : (-1));
		buttonUpSpriteId = ((buttonUpSprite.Length > 0) ? sprite.GetSpriteIdByName(buttonUpSprite) : (-1));
		buttonPressedSpriteId = ((buttonPressedSprite.Length > 0) ? sprite.GetSpriteIdByName(buttonPressedSprite) : (-1));
	}

	private void PlaySound(AudioClip source)
	{
		if (Object.op_Implicit((Object)(object)((Component)this).GetComponent<AudioSource>()) && Object.op_Implicit((Object)(object)source))
		{
			((Component)this).GetComponent<AudioSource>().PlayOneShot(source);
		}
	}

	private IEnumerator coScale(Vector3 defaultScale, float startScale, float endScale)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		float t0 = Time.realtimeSinceStartup;
		for (float num = 0f; num < scaleTime; num = Time.realtimeSinceStartup - t0)
		{
			float num2 = Mathf.Clamp01(num / scaleTime);
			float num3 = Mathf.Lerp(startScale, endScale, num2);
			Vector3 localScale = defaultScale * num3;
			((Component)this).transform.localScale = localScale;
			yield return 0;
		}
		((Component)this).transform.localScale = defaultScale * endScale;
	}

	private IEnumerator LocalWaitForSeconds(float seconds)
	{
		float t0 = Time.realtimeSinceStartup;
		for (float num = 0f; num < seconds; num = Time.realtimeSinceStartup - t0)
		{
			yield return 0;
		}
	}

	private IEnumerator coHandleButtonPress(int fingerId)
	{
		buttonDown = true;
		bool buttonPressed = true;
		Vector3 defaultScale = ((Component)this).transform.localScale;
		if (targetScale != 1f)
		{
			yield return ((MonoBehaviour)this).StartCoroutine(coScale(defaultScale, 1f, targetScale));
		}
		PlaySound(buttonDownSound);
		if (buttonDownSpriteId != -1)
		{
			sprite.spriteId = buttonDownSpriteId;
		}
		if (this.ButtonDownEvent != null)
		{
			this.ButtonDownEvent(this);
		}
		RaycastHit val3 = default(RaycastHit);
		while (true)
		{
			Vector3 val = Vector3.zero;
			bool flag = true;
			if (fingerId != -1)
			{
				bool flag2 = false;
				for (int i = 0; i < Input.touchCount; i++)
				{
					Touch touch = Input.GetTouch(i);
					if (((Touch)(ref touch)).fingerId == fingerId)
					{
						if ((int)((Touch)(ref touch)).phase == 3 || (int)((Touch)(ref touch)).phase == 4)
						{
							break;
						}
						val = Vector2.op_Implicit(((Touch)(ref touch)).position);
						flag2 = true;
					}
				}
				if (!flag2)
				{
					flag = false;
				}
			}
			else
			{
				if (!Input.GetMouseButton(0))
				{
					flag = false;
				}
				val = Input.mousePosition;
			}
			if (!flag)
			{
				break;
			}
			Ray val2 = viewCamera.ScreenPointToRay(val);
			bool flag3 = ((Component)this).GetComponent<Collider>().Raycast(val2, ref val3, float.PositiveInfinity);
			if (buttonPressed && !flag3)
			{
				if (targetScale != 1f)
				{
					yield return ((MonoBehaviour)this).StartCoroutine(coScale(defaultScale, targetScale, 1f));
				}
				PlaySound(buttonUpSound);
				if (buttonUpSpriteId != -1)
				{
					sprite.spriteId = buttonUpSpriteId;
				}
				if (this.ButtonUpEvent != null)
				{
					this.ButtonUpEvent(this);
				}
				buttonPressed = false;
			}
			else if (!buttonPressed && flag3)
			{
				if (targetScale != 1f)
				{
					yield return ((MonoBehaviour)this).StartCoroutine(coScale(defaultScale, 1f, targetScale));
				}
				PlaySound(buttonDownSound);
				if (buttonDownSpriteId != -1)
				{
					sprite.spriteId = buttonDownSpriteId;
				}
				if (this.ButtonDownEvent != null)
				{
					this.ButtonDownEvent(this);
				}
				buttonPressed = true;
			}
			if (buttonPressed && this.ButtonAutoFireEvent != null)
			{
				this.ButtonAutoFireEvent(this);
			}
			yield return 0;
		}
		if (buttonPressed)
		{
			if (targetScale != 1f)
			{
				yield return ((MonoBehaviour)this).StartCoroutine(coScale(defaultScale, targetScale, 1f));
			}
			PlaySound(buttonPressedSound);
			if (buttonPressedSpriteId != -1)
			{
				sprite.spriteId = buttonPressedSpriteId;
			}
			if (Object.op_Implicit((Object)(object)targetObject))
			{
				targetObject.SendMessage(messageName);
			}
			if (this.ButtonUpEvent != null)
			{
				this.ButtonUpEvent(this);
			}
			if (this.ButtonPressedEvent != null)
			{
				this.ButtonPressedEvent(this);
			}
			if (((Component)this).gameObject.activeInHierarchy)
			{
				yield return ((MonoBehaviour)this).StartCoroutine(LocalWaitForSeconds(pressedWaitTime));
			}
			if (buttonUpSpriteId != -1)
			{
				sprite.spriteId = buttonUpSpriteId;
			}
		}
		buttonDown = false;
	}

	private void Update()
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		if (buttonDown)
		{
			return;
		}
		bool flag = false;
		if (Input.multiTouchEnabled)
		{
			RaycastHit val2 = default(RaycastHit);
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				if ((int)((Touch)(ref touch)).phase == 0)
				{
					Ray val = viewCamera.ScreenPointToRay(Vector2.op_Implicit(((Touch)(ref touch)).position));
					if (((Component)this).GetComponent<Collider>().Raycast(val, ref val2, 100000000f) && !Physics.Raycast(val, ((RaycastHit)(ref val2)).distance - 0.01f))
					{
						((MonoBehaviour)this).StartCoroutine(coHandleButtonPress(((Touch)(ref touch)).fingerId));
						flag = true;
						break;
					}
				}
			}
		}
		if (!flag && Input.GetMouseButtonDown(0))
		{
			Ray val3 = viewCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit val4 = default(RaycastHit);
			if (((Component)this).GetComponent<Collider>().Raycast(val3, ref val4, 100000000f) && !Physics.Raycast(val3, ((RaycastHit)(ref val4)).distance - 0.01f))
			{
				((MonoBehaviour)this).StartCoroutine(coHandleButtonPress(-1));
			}
		}
	}
}
