using UnityEngine;

public static class CameraExtension
{
	public static Bounds OrthographicBounds(this Camera camera)
	{
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = camera.orthographicSize * 2;
		Bounds bounds = new Bounds(
			camera.transform.position,
			new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
		return bounds;
	}

	public static bool Contains2D(this Bounds boundary, Vector3 point)
	{
		return boundary.Contains(point.WithZ(boundary.center.z));
	}
}