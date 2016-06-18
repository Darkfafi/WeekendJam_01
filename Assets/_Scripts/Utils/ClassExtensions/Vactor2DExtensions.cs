using UnityEngine;
using System.Collections;

public static class Vactor2DExtensions {

	public static Vector2 Vec3Add(this Vector2 vec2D,Vector3 vec3D, bool zIsY = false)
	{
		Vector2 adder = new Vector2(vec3D.x,(zIsY) ? vec3D.z : vec3D.y);
		return vec2D += adder;
    }

	public static Vector2 Vec3Substract(this Vector2 vec2D, Vector3 vec3D, bool zIsY = false)
	{
		Vector2 sub = new Vector2(vec3D.x, (zIsY) ? vec3D.z : vec3D.y);
		return vec2D -= sub;
	}

	public static float DistanceTo(this Vector2 own, Vector2 other)
	{
		return (own - other).magnitude;
	}

	public static Vector2 XFlatten(this Vector2 own)
	{
		return new Vector2(0, own.y);
	}

	public static Vector2 YFlatten(this Vector2 own)
	{
		return new Vector2(own.x, 0);
	}
}
