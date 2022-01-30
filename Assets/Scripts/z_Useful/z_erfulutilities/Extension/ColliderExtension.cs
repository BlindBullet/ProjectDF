using UnityEngine;

public static class ColliderExtension
{
	public static Bounds GetWorldBounds(this BoxCollider2D collider)
	{
		return new Bounds()
		{
			center = collider.bounds.center + collider.transform.position,
			size = collider.bounds.size,
		};

	}
}