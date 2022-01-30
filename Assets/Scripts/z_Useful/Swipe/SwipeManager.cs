using UnityEngine;
using System;


public class SwipeManager : MonoBehaviour
{
	[Flags]
	public enum SwipeDirection
	{
		None = 0, Left = 1, Right = 2, Up = 4, Down = 8
	}

	private static SwipeManager m_instance;

	public static SwipeManager Instance
	{
		get
		{
			if (!m_instance)
			{
				m_instance = new GameObject("SwipeManager").AddComponent<SwipeManager>();
			}

			return m_instance;
		}
	}


	public SwipeDirection Direction { get; private set; }

	private Vector3 m_touchPosition, gap;
	private float m_swipeResistanceX = 50.0f;
	private float m_swipeResistanceY = 50.0f;
	private bool wait;

	private void Start()
	{
		if (m_instance != this)
		{
			Debug.LogError("Don't instantiate SwipeManager manually");
			DestroyImmediate(this);
		}
	}

	private void Update()
	{
		Direction = SwipeDirection.None;

		if (Input.GetMouseButtonDown(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began))
		{
			m_touchPosition = Input.GetMouseButton(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position;
			wait = true;
		}
		
		if (Input.GetMouseButtonUp(0) && wait)
		{
			Vector2 deltaSwipe = m_touchPosition - Input.mousePosition;
			float deltaSwipeXDist = Mathf.Abs(deltaSwipe.x);
			float deltaSwipeYDist = Mathf.Abs(deltaSwipe.y);
			deltaSwipe.Normalize();

			//위
			if (deltaSwipe.y < 0 && Mathf.Abs(deltaSwipe.x) < 0.5f && deltaSwipeYDist > m_swipeResistanceY)
			{
				Direction = SwipeDirection.Up;
			}

			//아래
			if (deltaSwipe.y > 0 && Mathf.Abs(deltaSwipe.x) < 0.5f && deltaSwipeYDist > m_swipeResistanceY)
			{
				Direction = SwipeDirection.Down;
			}

			//오른쪽
			if (deltaSwipe.x < 0 && Mathf.Abs(deltaSwipe.y) < 0.5f && deltaSwipeXDist > m_swipeResistanceX)
			{
				Direction = SwipeDirection.Right;
			}

			//왼쪽
			if (deltaSwipe.x > 0 && Mathf.Abs(deltaSwipe.y) < 0.5f && deltaSwipeXDist > m_swipeResistanceX)
			{
				Direction = SwipeDirection.Left;
			}

			wait = false;
		}

	}

	public bool IsSwiping(SwipeDirection dir)
	{
		return (Direction & dir) == dir;
	}

}
