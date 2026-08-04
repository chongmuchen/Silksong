using UnityEngine;

namespace TeamCherry.SharedUtils
{

public interface IVertexColor
{
	Color VertexColor { get; set; }

	GameObject gameObject { get; }
}
}
