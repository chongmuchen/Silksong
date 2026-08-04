using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[AddComponentMenu("2D Toolkit/Camera/tk2dCamera")]
[ExecuteAlways]
public class tk2dCamera : MonoBehaviour
{
	private static int CURRENT_VERSION = 1;

	public int version;

	[SerializeField]
	private tk2dCameraSettings cameraSettings = new tk2dCameraSettings();

	public tk2dCameraResolutionOverride[] resolutionOverride = new tk2dCameraResolutionOverride[1] { tk2dCameraResolutionOverride.DefaultOverride };

	[SerializeField]
	private tk2dCamera inheritSettings;

	public int nativeResolutionWidth = 960;

	public int nativeResolutionHeight = 640;

	[SerializeField]
	private Camera _unityCamera;

	private static tk2dCamera inst;

	private static List<tk2dCamera> allCameras = new List<tk2dCamera>();

	public bool viewportClippingEnabled;

	public Vector4 viewportRegion = new Vector4(0f, 0f, 100f, 100f);

	private Vector2 _targetResolution = Vector2.zero;

	[SerializeField]
	private float zoomFactor = 1f;

	[HideInInspector]
	public bool forceResolutionInEditor;

	[HideInInspector]
	public Vector2 forceResolution = new Vector2(960f, 640f);

	private Rect _screenExtents;

	private Rect _nativeScreenExtents;

	private Rect unitRect = new Rect(0f, 0f, 1f, 1f);

	private tk2dCamera _settingsRoot;

	public tk2dCameraSettings CameraSettings => cameraSettings;

	public tk2dCameraResolutionOverride CurrentResolutionOverride
	{
		get
		{
			tk2dCamera settingsRoot = SettingsRoot;
			Camera screenCamera = ScreenCamera;
			float num = screenCamera.pixelWidth;
			float num2 = screenCamera.pixelHeight;
			tk2dCameraResolutionOverride tk2dCameraResolutionOverride2 = null;
			if (tk2dCameraResolutionOverride2 == null || (tk2dCameraResolutionOverride2 != null && ((float)tk2dCameraResolutionOverride2.width != num || (float)tk2dCameraResolutionOverride2.height != num2)))
			{
				tk2dCameraResolutionOverride2 = null;
				if (settingsRoot.resolutionOverride != null)
				{
					tk2dCameraResolutionOverride[] array = settingsRoot.resolutionOverride;
					foreach (tk2dCameraResolutionOverride tk2dCameraResolutionOverride3 in array)
					{
						if (tk2dCameraResolutionOverride3.Match((int)num, (int)num2))
						{
							tk2dCameraResolutionOverride2 = tk2dCameraResolutionOverride3;
							break;
						}
					}
				}
			}
			return tk2dCameraResolutionOverride2;
		}
	}

	public tk2dCamera InheritConfig
	{
		get
		{
			return inheritSettings;
		}
		set
		{
			if ((Object)(object)inheritSettings != (Object)(object)value)
			{
				inheritSettings = value;
				_settingsRoot = null;
			}
		}
	}

	private Camera UnityCamera
	{
		get
		{
			if ((Object)(object)_unityCamera == (Object)null)
			{
				_unityCamera = ((Component)this).GetComponent<Camera>();
				if ((Object)(object)_unityCamera == (Object)null)
				{
					Debug.LogError((object)"A unity camera must be attached to the tk2dCamera script");
				}
			}
			return _unityCamera;
		}
	}

	public static tk2dCamera Instance => inst;

	public Rect ScreenExtents => _screenExtents;

	public Rect NativeScreenExtents => _nativeScreenExtents;

	public Vector2 TargetResolution => _targetResolution;

	public Vector2 NativeResolution => new Vector2((float)nativeResolutionWidth, (float)nativeResolutionHeight);

	[Obsolete]
	public Vector2 ScreenOffset
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			Rect val = ScreenExtents;
			float xMin = val.xMin;
			val = NativeScreenExtents;
			float num = xMin - val.xMin;
			val = ScreenExtents;
			float yMin = val.yMin;
			val = NativeScreenExtents;
			return new Vector2(num, yMin - val.yMin);
		}
	}

	[Obsolete]
	public Vector2 resolution
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			Rect screenExtents = ScreenExtents;
			float xMax = screenExtents.xMax;
			screenExtents = ScreenExtents;
			return new Vector2(xMax, screenExtents.yMax);
		}
	}

	[Obsolete]
	public Vector2 ScreenResolution
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			Rect screenExtents = ScreenExtents;
			float xMax = screenExtents.xMax;
			screenExtents = ScreenExtents;
			return new Vector2(xMax, screenExtents.yMax);
		}
	}

	[Obsolete]
	public Vector2 ScaledResolution
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			Rect screenExtents = ScreenExtents;
			float width = screenExtents.width;
			screenExtents = ScreenExtents;
			return new Vector2(width, screenExtents.height);
		}
	}

	public float ZoomFactor
	{
		get
		{
			return zoomFactor;
		}
		set
		{
			zoomFactor = Mathf.Max(0.01f, value);
		}
	}

	[Obsolete]
	public float zoomScale
	{
		get
		{
			return 1f / Mathf.Max(0.001f, zoomFactor);
		}
		set
		{
			ZoomFactor = 1f / Mathf.Max(0.001f, value);
		}
	}

	public Camera ScreenCamera
	{
		get
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			if (!viewportClippingEnabled || !((Object)(object)inheritSettings != (Object)null) || !(inheritSettings.UnityCamera.rect == unitRect))
			{
				return UnityCamera;
			}
			return inheritSettings.UnityCamera;
		}
	}

	public tk2dCamera SettingsRoot
	{
		get
		{
			if ((Object)(object)_settingsRoot == (Object)null)
			{
				_settingsRoot = (((Object)(object)inheritSettings == (Object)null || (Object)(object)inheritSettings == (Object)(object)this) ? this : inheritSettings.SettingsRoot);
			}
			return _settingsRoot;
		}
	}

	public static tk2dCamera CameraForLayer(int layer)
	{
		int num = 1 << layer;
		int count = allCameras.Count;
		for (int i = 0; i < count; i++)
		{
			tk2dCamera tk2dCamera2 = allCameras[i];
			if ((tk2dCamera2.UnityCamera.cullingMask & num) == num)
			{
				return tk2dCamera2;
			}
		}
		return null;
	}

	private void Awake()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		Upgrade();
		if (allCameras.IndexOf(this) == -1)
		{
			allCameras.Add(this);
		}
		tk2dCameraSettings tk2dCameraSettings2 = SettingsRoot.CameraSettings;
		if (tk2dCameraSettings2.projection == tk2dCameraSettings.ProjectionType.Perspective && UnityCamera.transparencySortMode != tk2dCameraSettings2.transparencySortMode)
		{
			UnityCamera.transparencySortMode = tk2dCameraSettings2.transparencySortMode;
		}
	}

	private void OnEnable()
	{
		if ((Object)(object)UnityCamera != (Object)null)
		{
			UpdateCameraMatrix();
		}
		else
		{
			((Behaviour)((Component)this).GetComponent<Camera>()).enabled = false;
		}
		if (!viewportClippingEnabled)
		{
			inst = this;
		}
		if (allCameras.IndexOf(this) == -1)
		{
			allCameras.Add(this);
		}
	}

	private void OnDestroy()
	{
		int num = allCameras.IndexOf(this);
		if (num != -1)
		{
			allCameras.RemoveAt(num);
		}
	}

	private void OnPreCull()
	{
		tk2dUpdateManager.FlushQueues();
		UpdateCameraMatrix();
	}

	public float GetSizeAtDistance(float distance)
	{
		tk2dCameraSettings tk2dCameraSettings2 = SettingsRoot.CameraSettings;
		switch (tk2dCameraSettings2.projection)
		{
		case tk2dCameraSettings.ProjectionType.Orthographic:
			if (tk2dCameraSettings2.orthographicType == tk2dCameraSettings.OrthographicType.PixelsPerMeter)
			{
				return 1f / tk2dCameraSettings2.orthographicPixelsPerMeter;
			}
			return 2f * tk2dCameraSettings2.orthographicSize / (float)SettingsRoot.nativeResolutionHeight;
		case tk2dCameraSettings.ProjectionType.Perspective:
			return Mathf.Tan(CameraSettings.fieldOfView * (MathF.PI / 180f) * 0.5f) * distance * 2f / (float)SettingsRoot.nativeResolutionHeight;
		default:
			return 1f;
		}
	}

	public Matrix4x4 OrthoOffCenter(Vector2 scale, float left, float right, float bottom, float top, float near, float far)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		float num = 2f / (right - left) * scale.x;
		float num2 = 2f / (top - bottom) * scale.y;
		float num3 = -2f / (far - near);
		float num4 = (0f - (right + left)) / (right - left);
		float num5 = (0f - (bottom + top)) / (top - bottom);
		float num6 = (0f - (far + near)) / (far - near);
		Matrix4x4 result = default(Matrix4x4);
		result[0, 0] = num;
		result[0, 1] = 0f;
		result[0, 2] = 0f;
		result[0, 3] = num4;
		result[1, 0] = 0f;
		result[1, 1] = num2;
		result[1, 2] = 0f;
		result[1, 3] = num5;
		result[2, 0] = 0f;
		result[2, 1] = 0f;
		result[2, 2] = num3;
		result[2, 3] = num6;
		result[3, 0] = 0f;
		result[3, 1] = 0f;
		result[3, 2] = 0f;
		result[3, 3] = 1f;
		return result;
	}

	private Vector2 GetScaleForOverride(tk2dCamera settings, tk2dCameraResolutionOverride currentOverride, float width, float height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		Vector2 one = Vector2.one;
		float num = 1f;
		if (currentOverride == null)
		{
			return one;
		}
		switch (currentOverride.autoScaleMode)
		{
		case tk2dCameraResolutionOverride.AutoScaleMode.PixelPerfect:
			num = 1f;
			one.Set(num, num);
			break;
		case tk2dCameraResolutionOverride.AutoScaleMode.FitHeight:
			num = height / (float)settings.nativeResolutionHeight;
			one.Set(num, num);
			break;
		case tk2dCameraResolutionOverride.AutoScaleMode.FitWidth:
			num = width / (float)settings.nativeResolutionWidth;
			one.Set(num, num);
			break;
		case tk2dCameraResolutionOverride.AutoScaleMode.FitVisible:
		case tk2dCameraResolutionOverride.AutoScaleMode.ClosestMultipleOfTwo:
		{
			float num2 = (float)settings.nativeResolutionWidth / (float)settings.nativeResolutionHeight;
			num = ((!(width / height < num2)) ? (height / (float)settings.nativeResolutionHeight) : (width / (float)settings.nativeResolutionWidth));
			if (currentOverride.autoScaleMode == tk2dCameraResolutionOverride.AutoScaleMode.ClosestMultipleOfTwo)
			{
				num = ((!(num > 1f)) ? Mathf.Pow(2f, Mathf.Floor(Mathf.Log(num, 2f))) : Mathf.Floor(num));
			}
			one.Set(num, num);
			break;
		}
		case tk2dCameraResolutionOverride.AutoScaleMode.StretchToFit:
			one.Set(width / (float)settings.nativeResolutionWidth, height / (float)settings.nativeResolutionHeight);
			break;
		case tk2dCameraResolutionOverride.AutoScaleMode.Fill:
			num = Mathf.Max(width / (float)settings.nativeResolutionWidth, height / (float)settings.nativeResolutionHeight);
			one.Set(num, num);
			break;
		default:
			num = currentOverride.scale;
			one.Set(num, num);
			break;
		}
		return one;
	}

	private Vector2 GetOffsetForOverride(tk2dCamera settings, tk2dCameraResolutionOverride currentOverride, Vector2 scale, float width, float height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 result = Vector2.zero;
		if (currentOverride == null)
		{
			return result;
		}
		tk2dCameraResolutionOverride.FitMode fitMode = currentOverride.fitMode;
		if (fitMode != tk2dCameraResolutionOverride.FitMode.Constant && fitMode == tk2dCameraResolutionOverride.FitMode.Center)
		{
			if (settings.cameraSettings.orthographicOrigin == tk2dCameraSettings.OrthographicOrigin.BottomLeft)
			{
				result = new Vector2(Mathf.Round(((float)settings.nativeResolutionWidth * scale.x - width) / 2f), Mathf.Round(((float)settings.nativeResolutionHeight * scale.y - height) / 2f));
			}
		}
		else
		{
			result = -currentOverride.offsetPixels;
		}
		return result;
	}

	private Matrix4x4 GetProjectionMatrixForOverride(tk2dCamera settings, tk2dCameraResolutionOverride currentOverride, float pixelWidth, float pixelHeight, bool halfTexelOffset, out Rect screenExtents, out Rect unscaledScreenExtents)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Invalid comparison between Unknown and I4
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Invalid comparison between Unknown and I4
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		Vector2 scaleForOverride = GetScaleForOverride(settings, currentOverride, pixelWidth, pixelHeight);
		Vector2 offsetForOverride = GetOffsetForOverride(settings, currentOverride, scaleForOverride, pixelWidth, pixelHeight);
		float num = offsetForOverride.x;
		float num2 = offsetForOverride.y;
		float num3 = pixelWidth + offsetForOverride.x;
		float num4 = pixelHeight + offsetForOverride.y;
		Vector2 zero = Vector2.zero;
		bool flag = false;
		Vector4 val = default(Vector4);
		Rect rect = default(Rect);
		Rect rect2;
		if (viewportClippingEnabled && (Object)(object)InheritConfig != (Object)null)
		{
			float num5 = (num3 - num) / scaleForOverride.x;
			float num6 = (num4 - num2) / scaleForOverride.y;
			val = new Vector4((float)(int)viewportRegion.x, (float)(int)viewportRegion.y, (float)(int)viewportRegion.z, (float)(int)viewportRegion.w);
			flag = true;
			float num7 = (0f - offsetForOverride.x) / pixelWidth + val.x / num5;
			float num8 = (0f - offsetForOverride.y) / pixelHeight + val.y / num6;
			float num9 = val.z / num5;
			float num10 = val.w / num6;
			if (settings.cameraSettings.orthographicOrigin == tk2dCameraSettings.OrthographicOrigin.Center)
			{
				num7 += (pixelWidth - (float)settings.nativeResolutionWidth * scaleForOverride.x) / pixelWidth / 2f;
				num8 += (pixelHeight - (float)settings.nativeResolutionHeight * scaleForOverride.y) / pixelHeight / 2f;
			}
			rect = new Rect(num7, num8, num9, num10);
			rect2 = UnityCamera.rect;
			if (rect2.x == num7)
			{
				rect2 = UnityCamera.rect;
				if (rect2.y == num8)
				{
					rect2 = UnityCamera.rect;
					if (rect2.width == num9)
					{
						rect2 = UnityCamera.rect;
						if (rect2.height == num10)
						{
							goto IL_01c5;
						}
					}
				}
			}
			UnityCamera.rect = rect;
			goto IL_01c5;
		}
		if (UnityCamera.rect != CameraSettings.rect)
		{
			UnityCamera.rect = CameraSettings.rect;
		}
		if (settings.cameraSettings.orthographicOrigin == tk2dCameraSettings.OrthographicOrigin.Center)
		{
			float num11 = (num3 - num) * 0.5f;
			num -= num11;
			num3 -= num11;
			float num12 = (num4 - num2) * 0.5f;
			num4 -= num12;
			num2 -= num12;
			zero.Set((float)(-nativeResolutionWidth) / 2f, (float)(-nativeResolutionHeight) / 2f);
		}
		goto IL_038a;
		IL_01c5:
		float num13 = Mathf.Min(1f - rect.x, rect.width);
		float num14 = Mathf.Min(1f - rect.y, rect.height);
		float num15 = val.x * scaleForOverride.x - offsetForOverride.x;
		float num16 = val.y * scaleForOverride.y - offsetForOverride.y;
		if (settings.cameraSettings.orthographicOrigin == tk2dCameraSettings.OrthographicOrigin.Center)
		{
			num15 -= (float)settings.nativeResolutionWidth * 0.5f * scaleForOverride.x;
			num16 -= (float)settings.nativeResolutionHeight * 0.5f * scaleForOverride.y;
		}
		if (rect.x < 0f)
		{
			num15 += (0f - rect.x) * pixelWidth;
			num13 = rect.x + rect.width;
		}
		if (rect.y < 0f)
		{
			num16 += (0f - rect.y) * pixelHeight;
			num14 = rect.y + rect.height;
		}
		num += num15;
		num2 += num16;
		num3 = pixelWidth * num13 + offsetForOverride.x + num15;
		num4 = pixelHeight * num14 + offsetForOverride.y + num16;
		goto IL_038a;
		IL_038a:
		float num17 = 1f / ZoomFactor;
		bool flag2 = (int)Application.platform == 2 || (int)Application.platform == 7;
		float num18 = ((halfTexelOffset && flag2 && SystemInfo.graphicsShaderLevel < 40) ? 0.5f : 0f);
		float num19 = settings.cameraSettings.orthographicSize;
		switch (settings.cameraSettings.orthographicType)
		{
		case tk2dCameraSettings.OrthographicType.OrthographicSize:
			num19 = 2f * settings.cameraSettings.orthographicSize / (float)settings.nativeResolutionHeight;
			break;
		case tk2dCameraSettings.OrthographicType.PixelsPerMeter:
			num19 = 1f / settings.cameraSettings.orthographicPixelsPerMeter;
			break;
		}
		if (!flag)
		{
			rect2 = UnityCamera.rect;
			float width = rect2.width;
			rect2 = UnityCamera.rect;
			float num20 = Mathf.Min(width, 1f - rect2.x);
			rect2 = UnityCamera.rect;
			float height = rect2.height;
			rect2 = UnityCamera.rect;
			float num21 = Mathf.Min(height, 1f - rect2.y);
			if (num20 > 0f && num21 > 0f)
			{
				scaleForOverride.x /= num20;
				scaleForOverride.y /= num21;
			}
		}
		float num22 = num19 * num17;
		screenExtents = new Rect(num * num22 / scaleForOverride.x, num2 * num22 / scaleForOverride.y, (num3 - num) * num22 / scaleForOverride.x, (num4 - num2) * num22 / scaleForOverride.y);
		unscaledScreenExtents = new Rect(zero.x * num22, zero.y * num22, (float)nativeResolutionWidth * num22, (float)nativeResolutionHeight * num22);
		return OrthoOffCenter(scaleForOverride, num19 * (num + num18) * num17, num19 * (num3 + num18) * num17, num19 * (num2 - num18) * num17, num19 * (num4 - num18) * num17, UnityCamera.nearClipPlane, UnityCamera.farClipPlane);
	}

	private Vector2 GetScreenPixelDimensions(tk2dCamera settings)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)ScreenCamera.pixelWidth, (float)ScreenCamera.pixelHeight);
	}

	private void Upgrade()
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		if (version == CURRENT_VERSION)
		{
			return;
		}
		if (version == 0)
		{
			cameraSettings.orthographicPixelsPerMeter = 1f;
			cameraSettings.orthographicType = tk2dCameraSettings.OrthographicType.PixelsPerMeter;
			cameraSettings.orthographicOrigin = tk2dCameraSettings.OrthographicOrigin.BottomLeft;
			cameraSettings.projection = tk2dCameraSettings.ProjectionType.Orthographic;
			tk2dCameraResolutionOverride[] array = resolutionOverride;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Upgrade(version);
			}
			Camera component = ((Component)this).GetComponent<Camera>();
			if ((Object)(object)component != (Object)null)
			{
				cameraSettings.rect = component.rect;
				if (!component.orthographic)
				{
					cameraSettings.projection = tk2dCameraSettings.ProjectionType.Perspective;
					cameraSettings.fieldOfView = component.fieldOfView * ZoomFactor;
				}
				((Object)component).hideFlags = (HideFlags)3;
			}
		}
		Debug.Log((object)("tk2dCamera '" + ((Object)this).name + "' - Upgraded from version " + version));
		version = CURRENT_VERSION;
	}

	public void UpdateCameraMatrix()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Invalid comparison between Unknown and I4
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Invalid comparison between Unknown and I4
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Invalid comparison between Unknown and I4
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Invalid comparison between Unknown and I4
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		Upgrade();
		if (!viewportClippingEnabled)
		{
			inst = this;
		}
		Camera unityCamera = UnityCamera;
		tk2dCamera settingsRoot = SettingsRoot;
		tk2dCameraSettings tk2dCameraSettings2 = settingsRoot.CameraSettings;
		if (unityCamera.rect != cameraSettings.rect && !Tk2dGlobalEvents.IsFrozenCameraRendering())
		{
			unityCamera.rect = cameraSettings.rect;
		}
		_targetResolution = GetScreenPixelDimensions(settingsRoot);
		if (tk2dCameraSettings2.projection == tk2dCameraSettings.ProjectionType.Perspective)
		{
			if (unityCamera.orthographic)
			{
				unityCamera.orthographic = false;
			}
			float num = Mathf.Min(179.9f, tk2dCameraSettings2.fieldOfView / Mathf.Max(0.001f, ZoomFactor));
			if (unityCamera.fieldOfView != num)
			{
				unityCamera.fieldOfView = num;
			}
			_screenExtents.Set(0f - unityCamera.aspect, -1f, unityCamera.aspect * 2f, 2f);
			_nativeScreenExtents = _screenExtents;
			unityCamera.ResetProjectionMatrix();
			return;
		}
		if (!unityCamera.orthographic)
		{
			unityCamera.orthographic = true;
		}
		Matrix4x4 val = GetProjectionMatrixForOverride(settingsRoot, settingsRoot.CurrentResolutionOverride, _targetResolution.x, _targetResolution.y, halfTexelOffset: true, out _screenExtents, out _nativeScreenExtents);
		if ((int)Application.platform == 21 && ((int)Screen.orientation == 3 || (int)Screen.orientation == 4))
		{
			float num2 = (((int)Screen.orientation == 4) ? 90f : (-90f));
			val = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, num2), Vector3.one) * val;
		}
		if (unityCamera.projectionMatrix != val)
		{
			unityCamera.projectionMatrix = val;
		}
	}
}
