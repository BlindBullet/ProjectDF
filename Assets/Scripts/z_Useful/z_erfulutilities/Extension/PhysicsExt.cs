using System.Linq;
using UnityEngine;

public static class PhysicsExt
{
	public static void Spring(ref Vector2 curPos, ref Vector2 curVel, Vector2 dstPos, float frequency, float deltaTime, float dampingRatio = 1)
	{
		float f = 1.0f + 2.0f * deltaTime * dampingRatio * frequency;
		float oo = frequency * frequency;
		float hoo = deltaTime * oo;
		float hhoo = deltaTime * hoo;
		float detInv = 1.0f / (f + hhoo);
		var detX = f * curPos + deltaTime * curVel + hhoo * dstPos;
		var detV = curVel + hoo * (dstPos - curPos);
		curPos = detX * detInv;
		curVel = detV * detInv;
	}

	/// <summary> {downward gravity < 0} </summary>
	public static Vector3 MuzzleVelocity2D(Vector3 direction, float angle, float gravirtyForce)
	{
		float h = direction.y;                                            // get height difference
		direction.y = 0;                                                // remove height
		float distance = direction.magnitude;                            // get horizontal distance
		float a = angle * Mathf.Deg2Rad;                                // Convert angle to radians
		direction.y = distance * Mathf.Tan(a);                            // Set direction to elevation angle
		distance += h / Mathf.Tan(a);                                        // Correction for small height differences

		// calculate velocity
		float velocity = Mathf.Sqrt(distance * Mathf.Abs(gravirtyForce) / Mathf.Sin(2 * a));
		return velocity * direction.normalized;
	}

	public static void RotateToDirection2D(Transform f_trf, Vector2 f_dir)
	{
		f_trf.rotation = Quaternion.identity;
		f_trf.Rotate(Vector3.forward, Mathf.Atan2(f_dir.y, f_dir.x) * Mathf.Rad2Deg);
	}

	public static Rect WorldRect(this BoxCollider2D box)
	{
		return new Rect(box.transform.TransformPoint(box.offset), box.size);
	}

	public static (GameObject, Vector3) FindClickPoint(Camera cam, LayerMask layer)
	{
		RaycastHit hitInfo = new RaycastHit();
		if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hitInfo, layer))
		{
			return (hitInfo.transform.gameObject, hitInfo.point);
		}
		return (null, Vector3.zero);
	}

	public static (T, Vector3) FineNearestFromMouseRay<T>(Camera cam, LayerMask layer, params GameObject[] ignores) where T : MonoBehaviour
	{
		const float determineRadius = 0.3f;
		var hits = Physics.SphereCastAll(cam.ScreenPointToRay(Input.mousePosition), determineRadius, 1000f, layer);
		if (hits.Length <= 0)
		{
			return default;
		}
		var candidates = (ignores.GetSize() > 0 ? hits.Where(elem => !ignores.Contains(elem.collider.gameObject)) : hits).Where(elem=> elem.collider.GetComponent<T>() != null);
		
		var nearest = candidates.MinBy(elem => elem.distance);
		return nearest.collider != null ? (nearest.transform.gameObject.GetComponent<T>(), nearest.point) : default;
	}
}