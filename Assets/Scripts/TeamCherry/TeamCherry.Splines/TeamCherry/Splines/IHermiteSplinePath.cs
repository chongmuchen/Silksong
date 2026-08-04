using UnityEngine;

namespace TeamCherry.Splines;

public interface IHermiteSplinePath
{
	int ControlPointCount { get; }

	void Reverse();

	Vector3 GetControlPoint(int i);

	bool CanDeleteControlPoint();

	void DeleteControlPoint(int i);

	void InsertControlPoint(int i, Vector3 mouseWorldPos, out int capturedIndex);

	void SetControlPoint(int i, Vector3 mouseWorldPos);
}
