using UnityEngine;

[AddComponentMenu("2D Toolkit/Camera/tk2dCameraAnchor")]
[ExecuteAlways]
public class tk2dCameraAnchor : MonoBehaviour
{
	[SerializeField]
	private int anchor = -1;

	[SerializeField]
	private tk2dBaseSprite.Anchor _anchorPoint = tk2dBaseSprite.Anchor.UpperLeft;

	[SerializeField]
	private bool anchorToNativeBounds;

	[SerializeField]
	private Vector2 offset = Vector2.zero;

	[SerializeField]
	private tk2dCamera tk2dCamera;

	[SerializeField]
	private Camera _anchorCamera;

	private Camera _anchorCameraCached;

	private tk2dCamera _anchorTk2dCamera;

	private Transform _myTransform;

	public tk2dBaseSprite.Anchor AnchorPoint
	{
		get
		{
			if (anchor != -1)
			{
				if (anchor >= 0 && anchor <= 2)
				{
					_anchorPoint = (tk2dBaseSprite.Anchor)(anchor + 6);
				}
				else if (anchor >= 6 && anchor <= 8)
				{
					_anchorPoint = (tk2dBaseSprite.Anchor)(anchor - 6);
				}
				else
				{
					_anchorPoint = (tk2dBaseSprite.Anchor)anchor;
				}
				anchor = -1;
			}
			return _anchorPoint;
		}
		set
		{
			_anchorPoint = value;
		}
	}

	public Vector2 AnchorOffsetPixels
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return offset;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			offset = value;
		}
	}

	public bool AnchorToNativeBounds
	{
		get
		{
			return anchorToNativeBounds;
		}
		set
		{
			anchorToNativeBounds = value;
		}
	}

	public Camera AnchorCamera
	{
		get
		{
			if ((Object)(object)tk2dCamera != (Object)null)
			{
				_anchorCamera = ((Component)tk2dCamera).GetComponent<Camera>();
				tk2dCamera = null;
			}
			return _anchorCamera;
		}
		set
		{
			_anchorCamera = value;
			_anchorCameraCached = null;
		}
	}

	private tk2dCamera AnchorTk2dCamera
	{
		get
		{
			if ((Object)(object)_anchorCameraCached != (Object)(object)_anchorCamera)
			{
				_anchorTk2dCamera = ((Component)_anchorCamera).GetComponent<tk2dCamera>();
				_anchorCameraCached = _anchorCamera;
			}
			return _anchorTk2dCamera;
		}
	}

	private Transform myTransform
	{
		get
		{
			if ((Object)(object)_myTransform == (Object)null)
			{
				_myTransform = ((Component)this).transform;
			}
			return _myTransform;
		}
	}

	private void Start()
	{
		UpdateTransform();
	}

	private void UpdateTransform()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)AnchorCamera == (Object)null)
		{
			return;
		}
		float num = 1f;
		Vector3 localPosition = myTransform.localPosition;
		tk2dCamera tk2dCamera2 = (((Object)(object)AnchorTk2dCamera != (Object)null && AnchorTk2dCamera.CameraSettings.projection != tk2dCameraSettings.ProjectionType.Perspective) ? AnchorTk2dCamera : null);
		Rect val = default(Rect);
		if ((Object)(object)tk2dCamera2 != (Object)null)
		{
			val = (anchorToNativeBounds ? tk2dCamera2.NativeScreenExtents : tk2dCamera2.ScreenExtents);
			num = tk2dCamera2.GetSizeAtDistance(1f);
		}
		else
		{
			((Rect)(ref val)).Set(0f, 0f, (float)AnchorCamera.pixelWidth, (float)AnchorCamera.pixelHeight);
		}
		float yMin = ((Rect)(ref val)).yMin;
		float yMax = ((Rect)(ref val)).yMax;
		float num2 = (yMin + yMax) * 0.5f;
		float xMin = ((Rect)(ref val)).xMin;
		float xMax = ((Rect)(ref val)).xMax;
		float num3 = (xMin + xMax) * 0.5f;
		Vector3 zero = Vector3.zero;
		switch (AnchorPoint)
		{
		case tk2dBaseSprite.Anchor.UpperLeft:
			((Vector3)(ref zero))._002Ector(xMin, yMax, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.UpperCenter:
			((Vector3)(ref zero))._002Ector(num3, yMax, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.UpperRight:
			((Vector3)(ref zero))._002Ector(xMax, yMax, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.MiddleLeft:
			((Vector3)(ref zero))._002Ector(xMin, num2, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.MiddleCenter:
			((Vector3)(ref zero))._002Ector(num3, num2, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.MiddleRight:
			((Vector3)(ref zero))._002Ector(xMax, num2, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.LowerLeft:
			((Vector3)(ref zero))._002Ector(xMin, yMin, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.LowerCenter:
			((Vector3)(ref zero))._002Ector(num3, yMin, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
			((Vector3)(ref zero))._002Ector(xMax, yMin, localPosition.z);
			break;
		}
		Vector3 val2 = zero + new Vector3(num * offset.x, num * offset.y, 0f);
		if ((Object)(object)tk2dCamera2 == (Object)null)
		{
			Vector3 val3 = AnchorCamera.ScreenToWorldPoint(val2);
			if (myTransform.position != val3)
			{
				myTransform.position = val3;
			}
		}
		else if (myTransform.localPosition != val2)
		{
			myTransform.localPosition = val2;
		}
	}

	public void ForceUpdateTransform()
	{
		UpdateTransform();
	}

	private void LateUpdate()
	{
		UpdateTransform();
	}
}
