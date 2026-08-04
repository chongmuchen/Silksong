using TeamCherry.SharedUtils;
using UnityEngine;

namespace TeamCherry.Splines;

public class LinearSpline : SplineBase
{
	[Header("Linear Spline")]
	[SerializeField]
	private Transform[] controlPoints;

	public Transform[] ControlPoints => controlPoints;

	public override void UpdatePositions()
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		if (controlPoints == null)
		{
			return;
		}
		int num = controlPoints.Length;
		if (num < 2)
		{
			return;
		}
		if (GetPointCount() != num)
		{
			InternalPoints = new Point[num];
		}
		for (int i = 0; i < num; i++)
		{
			Transform val = controlPoints[i];
			if (Object.op_Implicit((Object)(object)val))
			{
				InternalPoints[i] = new Point
				{
					Position = val.localPosition,
					Color = Color.white
				};
			}
			else
			{
				InternalPoints[i] = new Point
				{
					Color = Color.white
				};
			}
		}
		for (int j = 0; j < num; j++)
		{
			Vector3 tangent;
			if (base.CalculateSplineTangent)
			{
				Vector3 position = InternalPoints[j].Position;
				Vector3 val2 = Vector3.zero;
				float num2 = 0f;
				Vector3 val4;
				if (j > 0)
				{
					Vector3 position2 = InternalPoints[j - 1].Position;
					Vector3 val3 = val2;
					val4 = position - position2;
					val2 = val3 + ((Vector3)(ref val4)).normalized;
					num2 += 1f;
				}
				if (j < num - 1)
				{
					Vector3 position3 = InternalPoints[j + 1].Position;
					Vector3 val5 = val2;
					val4 = position3 - position;
					val2 = val5 + ((Vector3)(ref val4)).normalized;
					num2 += 1f;
				}
				Vector3 val6 = val2 / num2;
				tangent = ((Vector3)(ref val6)).normalized;
			}
			else
			{
				tangent = Vector3.zero;
			}
			InternalPoints[j].Tangent = tangent;
		}
	}

	public void InitDefaults()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		Transform transform = new GameObject("pt1").transform;
		transform.SetParentReset(((Component)this).transform);
		Transform transform2 = new GameObject("pt2").transform;
		transform2.SetParentReset(((Component)this).transform);
		transform2.localPosition += Vector3.one;
		controlPoints = (Transform[])(object)new Transform[2] { transform, transform2 };
	}
}
