using System;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

namespace TeamCherry.SharedUtils;

public static class Extensions
{
	public static bool IsAny(this string value, params string[] others)
	{
		foreach (string value2 in others)
		{
			if (value.Equals(value2))
			{
				return true;
			}
		}
		return false;
	}

	public static bool AddIfNotPresent<T>(this List<T> list, T item)
	{
		if (list.Contains(item))
		{
			return false;
		}
		list.Add(item);
		return true;
	}

	public static void SetParentReset(this Transform t, Transform parent)
	{
		t.SetParent(parent);
		t.Reset();
	}

	public static void Reset(this Transform t)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		t.localScale = Vector3.one;
		t.localRotation = Quaternion.identity;
		t.localPosition = Vector3.zero;
	}

	public static void SetPosition2D(this Transform t, float x, float y)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		t.position = new Vector3(x, y, t.position.z);
	}

	public static void SetPosition2D(this Transform t, Vector2 position)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		t.position = new Vector3(position.x, position.y, t.position.z);
	}

	public static Vector3 MultiplyElements(this Vector3 self, Vector3 other)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		Vector3 result = self;
		result.x *= other.x;
		result.y *= other.y;
		result.z *= other.z;
		return result;
	}

	public static Vector3 MultiplyElements(this Vector3 self, float? x = null, float? y = null, float? z = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		Vector3 result = self;
		result.x *= x ?? 1f;
		result.y *= y ?? 1f;
		result.z *= z ?? 1f;
		return result;
	}

	public static Vector2 MultiplyElements(this Vector2 self, Vector2 other)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		Vector2 result = self;
		result.x *= other.x;
		result.y *= other.y;
		return result;
	}

	public static Vector2 MultiplyElements(this Vector2 self, float? x = null, float? y = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 result = self;
		result.x *= x ?? 1f;
		result.y *= y ?? 1f;
		return result;
	}

	public static Vector4 MultiplyElements(this Vector4 self, Vector4 other)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		Vector4 result = self;
		result.x *= other.x;
		result.y *= other.y;
		result.z *= other.z;
		result.w += other.w;
		return result;
	}

	public static Vector4 MultiplyElements(this Vector4 self, float? x = null, float? y = null, float? z = null, float? w = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		Vector4 result = self;
		result.x *= x ?? 1f;
		result.y *= y ?? 1f;
		result.z *= z ?? 1f;
		result.w *= w ?? 1f;
		return result;
	}

	public static Vector3 DivideElements(this Vector3 self, Vector3 other)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		Vector3 result = self;
		result.x /= other.x;
		result.y /= other.y;
		result.z /= other.z;
		return result;
	}

	public static Vector3 DivideElements(this Vector3 self, float? x = null, float? y = null, float? z = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		Vector3 result = self;
		result.x /= x ?? 1f;
		result.y /= y ?? 1f;
		result.z /= z ?? 1f;
		return result;
	}

	public static Vector2 DivideElements(this Vector2 self, Vector2 other)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		Vector2 result = self;
		result.x /= other.x;
		result.y /= other.y;
		return result;
	}

	public static Vector2 DivideElements(this Vector2 self, float? x = null, float? y = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 result = self;
		result.x /= x ?? 1f;
		result.y /= y ?? 1f;
		return result;
	}

	public static Vector4 DivideElements(this Vector4 self, Vector4 other)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		Vector4 result = self;
		result.x /= other.x;
		result.y /= other.y;
		result.z /= other.z;
		result.w /= other.w;
		return result;
	}

	public static Vector4 DivideElements(this Vector4 self, float? x = null, float? y = null, float? z = null, float? w = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		Vector4 result = self;
		result.x /= x ?? 1f;
		result.y /= y ?? 1f;
		result.z /= z ?? 1f;
		result.w /= w ?? 1f;
		return result;
	}

	public static Vector3 Abs(this Vector3 self)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(Mathf.Abs(self.x), Mathf.Abs(self.y), Mathf.Abs(self.z));
	}

	public static Vector2 Abs(this Vector2 self)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(Mathf.Abs(self.x), Mathf.Abs(self.y));
	}

	public static Color MultiplyElements(this Color original, float? r = null, float? g = null, float? b = null, float? a = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		return original * (r ?? 1f) * (g ?? 1f) * (b ?? 1f) * (a ?? 1f);
	}

	public static Color MultiplyElements(this Color original, Color other)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		Color result = original;
		result.r *= other.r;
		result.g *= other.g;
		result.b *= other.b;
		result.a *= other.a;
		return result;
	}

	public static Color Where(this Color original, float? r = null, float? g = null, float? b = null, float? a = null)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		return new Color(r ?? original.r, g ?? original.g, b ?? original.b, a ?? original.a);
	}

	public static Vector3 Where(this Vector3 original, float? x = null, float? y = null, float? z = null)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
	}

	public static Vector2 Where(this Vector2 original, float? x = null, float? y = null)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(x ?? original.x, y ?? original.y);
	}

	public static Vector3 ToVector3(this Vector2 original, float z)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(original.x, original.y, z);
	}

	public static Coroutine StartTimerRoutine(this MonoBehaviour self, float delay, float duration, Action<float> handler, Action onAfterDelay = null, Action onTimerEnd = null, bool isRealtime = false)
	{
		if (duration > 0f)
		{
			return self.StartCoroutine(TimerRoutine(delay, duration, handler, onAfterDelay, onTimerEnd, isRealtime));
		}
		TimerRoutine(delay, duration, handler, onAfterDelay, onTimerEnd, isRealtime).MoveNext();
		return null;
	}

	private static IEnumerator TimerRoutine(float delay, float duration, Action<float> handler, Action onAfterDelay, Action onTimerEnd, bool isRealtime)
	{
		handler?.Invoke(0f);
		if (delay > 0f)
		{
			if (isRealtime)
			{
				yield return (object)new WaitForSecondsRealtime(delay);
			}
			else
			{
				yield return (object)new WaitForSeconds(delay);
			}
		}
		onAfterDelay?.Invoke();
		if (handler != null)
		{
			for (float elapsed = 0f; elapsed < duration; elapsed = ((!isRealtime) ? (elapsed + Time.deltaTime) : (elapsed + Time.unscaledDeltaTime)))
			{
				handler(elapsed / duration);
				yield return null;
			}
			handler(1f);
		}
		onTimerEnd?.Invoke();
	}

	public static bool IsBitSet(this int bitmask, int index)
	{
		int num = 1 << index;
		return (bitmask & num) == num;
	}

	public static int SetBitAtIndex(this int bitMask, int index)
	{
		bitMask |= 1 << index;
		return bitMask;
	}

	public static int ResetBitAtIndex(this int bitMask, int index)
	{
		bitMask &= ~(1 << index);
		return bitMask;
	}

	public static bool IsBitSet(this long bitmask, int index)
	{
		long num = 1L << index;
		return (bitmask & num) == num;
	}

	public static long SetBitAtIndex(this long bitMask, int index)
	{
		bitMask |= 1L << index;
		return bitMask;
	}

	public static long ResetBitAtIndex(this long bitMask, int index)
	{
		bitMask &= ~(1L << index);
		return bitMask;
	}

	public static GameObject GetSafe(this FsmOwnerDefault ownerDefault, FsmStateAction stateAction)
	{
		if (ownerDefault.OwnerOption != OwnerDefaultOption.UseOwner)
		{
			return ownerDefault.GameObject.Value;
		}
		return stateAction.Owner;
	}
}
