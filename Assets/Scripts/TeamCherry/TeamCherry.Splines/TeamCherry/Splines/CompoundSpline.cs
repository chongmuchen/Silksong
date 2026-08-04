using System;
using System.Collections.Generic;
using TeamCherry.SharedUtils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TeamCherry.Splines
{

public class CompoundSpline : MonoBehaviour
{
	public enum AddSplineTypes
	{
		Linear,
		QuadraticBezier,
		Hermite
	}

	[SerializeField]
	[HideInInspector]
	private List<SplineBase> splines;

	private List<SplineBase> activeSplines;

	private Vector2[] positions;

	private float[] distances;

	public float TotalDistance { get; private set; }

	private void OnDrawGizmos()
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		if (splines == null || splines.Count == 0)
		{
			return;
		}
		foreach (SplineBase spline in splines)
		{
			if ((Object)(object)spline == (Object)null)
			{
				return;
			}
		}
		if (!Application.isPlaying)
		{
			UpdateValues();
		}
		if (positions != null)
		{
			for (int i = 1; i < positions.Length; i++)
			{
				Vector2 val = positions[i];
				Gizmos.DrawLine((Vector2)(positions[i - 1]), (Vector2)(val));
			}
		}
	}

	protected virtual void OnEnable()
	{
		foreach (SplineBase spline in splines)
		{
			spline.UpdatePositions();
		}
		UpdateValues();
	}

	public void UpdateValues()
	{
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		if (activeSplines == null)
		{
			activeSplines = new List<SplineBase>();
		}
		else
		{
			activeSplines.Clear();
		}
		if (splines == null)
		{
			return;
		}
		int num = 0;
		foreach (SplineBase spline in splines)
		{
			if ((spline != null) && ((Component)spline).gameObject.activeInHierarchy)
			{
				int pointCount = spline.GetPointCount();
				if (pointCount > 0)
				{
					activeSplines.Add(spline);
					num += pointCount;
				}
			}
		}
		if (num <= 0)
		{
			return;
		}
		if (positions == null || positions.Length != num)
		{
			positions = (Vector2[])(object)new Vector2[num];
		}
		int num2 = num - 1;
		if (distances == null || distances.Length != num2)
		{
			distances = new float[num2];
		}
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < activeSplines.Count; i++)
		{
			SplineBase splineBase = activeSplines[i];
			int pointCount2 = splineBase.GetPointCount();
			for (int j = 0; j < pointCount2; j++)
			{
				SplineBase.Point point = splineBase.GetPoint(j);
				Vector2 val = (Vector2)(((Component)splineBase).transform.TransformPoint(point.Position));
				positions[num3] = val;
				num3++;
				if (j != 0 || i != 0)
				{
					int num5 = j - 1;
					Vector2 val2;
					if (num5 < 0)
					{
						SplineBase splineBase2 = activeSplines[i - 1];
						num5 = splineBase2.GetPointCount() - 1;
						SplineBase.Point point2 = splineBase2.GetPoint(num5);
						val2 = (Vector2)(((Component)splineBase2).transform.TransformPoint(point2.Position));
					}
					else
					{
						SplineBase.Point point2 = splineBase.GetPoint(num5);
						val2 = (Vector2)(((Component)splineBase).transform.TransformPoint(point2.Position));
					}
					float num6 = Vector2.Distance(val2, val);
					distances[num4] = num6;
					num4++;
				}
			}
		}
		TotalDistance = 0f;
		float[] array = distances;
		foreach (float num7 in array)
		{
			TotalDistance += num7;
		}
	}

	public Vector2 GetPositionAlongSpline(float currentDistance)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		currentDistance = Mathf.Clamp(currentDistance, 0f, TotalDistance);
		float num = 0f;
		Vector2 result = Vector2.zero;
		for (int i = 1; i < positions.Length; i++)
		{
			int num2 = i - 1;
			float num3 = distances[num2];
			float num4 = num;
			num += num3;
			if (!(currentDistance > num))
			{
				float num5 = (currentDistance - num4) / num3;
				Vector2 val = positions[i - 1];
				Vector2 val2 = positions[i];
				result = Vector2.Lerp(val, val2, num5);
				break;
			}
		}
		return result;
	}

	public void AddSpline(AddSplineTypes newSplineType)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = new GameObject(newSplineType.ToString());
		val.transform.SetParentReset(((Component)this).transform);
		val.transform.SetPosition2D(GetPositionAlongSpline(TotalDistance));
		SplineBase item;
		switch (newSplineType)
		{
		case AddSplineTypes.Linear:
		{
			LinearSpline linearSpline = val.AddComponent<LinearSpline>();
			linearSpline.InitDefaults();
			item = linearSpline;
			break;
		}
		case AddSplineTypes.QuadraticBezier:
		{
			QuadraticBezierSpline quadraticBezierSpline = val.AddComponent<QuadraticBezierSpline>();
			quadraticBezierSpline.InitDefaults();
			item = quadraticBezierSpline;
			break;
		}
		case AddSplineTypes.Hermite:
		{
			HermiteSpline hermiteSpline = val.AddComponent<HermiteSpline>();
			hermiteSpline.InitDefaults();
			item = hermiteSpline;
			break;
		}
		default:
			throw new NotImplementedException();
		}
		splines.Add(item);
		UpdateValues();
	}
}
}
