using System;
using UnityEngine;

namespace TeamCherry.NestedFadeGroup;

[ExecuteAlways]
[NestedFadeGroupBridge(new Type[] { typeof(ParticleSystem) })]
[RequireComponent(typeof(ParticleSystem))]
public class NestedFadeGroupParticleSystem : NestedFadeGroupBase
{
	private ParticleSystem system;

	private Particle[] particles;

	private bool hasSystem;

	private bool hasParticles;

	protected override void GetMissingReferences()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		if (hasSystem)
		{
			return;
		}
		hasSystem = Object.op_Implicit((Object)(object)(system = ((Component)this).GetComponent<ParticleSystem>()));
		MainModule main = system.main;
		int num = ((MainModule)(ref main)).maxParticles;
		if (!((MainModule)(ref main)).loop)
		{
			EmissionModule emission = system.emission;
			if (IsCurveZero(((EmissionModule)(ref emission)).rateOverDistance))
			{
				float num2 = 0f;
				MinMaxCurve rateOverTime = ((EmissionModule)(ref emission)).rateOverTime;
				ParticleSystemCurveMode mode = ((MinMaxCurve)(ref rateOverTime)).mode;
				if ((int)mode != 0)
				{
					if ((int)mode == 3)
					{
						rateOverTime = ((EmissionModule)(ref emission)).rateOverTime;
						num2 = ((MinMaxCurve)(ref rateOverTime)).constantMax;
					}
				}
				else
				{
					rateOverTime = ((EmissionModule)(ref emission)).rateOverTime;
					num2 = ((MinMaxCurve)(ref rateOverTime)).constant;
				}
				if (num2 > 0f)
				{
					num = Mathf.CeilToInt(num2 * ((MainModule)(ref main)).duration);
				}
			}
		}
		if (num <= 1000)
		{
			particles = (Particle[])(object)new Particle[num];
		}
		else
		{
			particles = null;
		}
		hasParticles = particles != null;
	}

	public void UpdateParticlesArraySize()
	{
		hasSystem = false;
		hasParticles = false;
		GetMissingReferences();
	}

	private bool IsCurveZero(MinMaxCurve curve)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected I4, but got Unknown
		ParticleSystemCurveMode mode = ((MinMaxCurve)(ref curve)).mode;
		switch ((int)mode)
		{
		case 0:
			return ((MinMaxCurve)(ref curve)).constant <= 0f;
		case 1:
			return IsCurveZero(((MinMaxCurve)(ref curve)).curve);
		case 3:
			if (((MinMaxCurve)(ref curve)).constantMin <= 0f)
			{
				return ((MinMaxCurve)(ref curve)).constantMax <= 0f;
			}
			return false;
		case 2:
			if (IsCurveZero(((MinMaxCurve)(ref curve)).curveMin))
			{
				return IsCurveZero(((MinMaxCurve)(ref curve)).curveMax);
			}
			return false;
		default:
			return false;
		}
	}

	private bool IsCurveZero(AnimationCurve curve)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (curve == null || curve.length == 0)
		{
			return true;
		}
		Keyframe[] keys = curve.keys;
		for (int i = 0; i < keys.Length; i++)
		{
			Keyframe val = keys[i];
			if (((Keyframe)(ref val)).value > 0f)
			{
				return false;
			}
		}
		return true;
	}

	protected override void OnAlphaChanged(float alpha)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		if (!hasParticles || !hasSystem)
		{
			return;
		}
		MainModule main = system.main;
		MinMaxGradient startColor = ((MainModule)(ref main)).startColor;
		Color color = ((MinMaxGradient)(ref startColor)).color;
		color.a = alpha;
		((MinMaxGradient)(ref startColor)).color = color;
		((MainModule)(ref main)).startColor = startColor;
		Color32 startColor2 = Color32.op_Implicit(color);
		int num = system.GetParticles(particles);
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				((Particle)(ref particles[i])).startColor = startColor2;
			}
		}
		system.SetParticles(particles, num);
	}
}
