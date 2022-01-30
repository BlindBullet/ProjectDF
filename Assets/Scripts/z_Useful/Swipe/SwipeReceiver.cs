using System;
using UnityEngine;

public abstract class SwipeReceiver : MonoBehaviour
{
	//override this
	protected virtual void OnSwipeLeft()
	{
		
	}

	//override this
	protected virtual void OnSwipeRight()
	{
		
	}

	//override this
	protected virtual void OnSwipeUp()
	{

	}

	//override this
	protected virtual void OnSwipeDown()
	{

	}

	protected virtual void Update()
	{
		if (SwipeManager.Instance.IsSwiping(SwipeManager.SwipeDirection.Right))
		{
			OnSwipeRight();
		}

		if (SwipeManager.Instance.IsSwiping(SwipeManager.SwipeDirection.Left))
		{
			OnSwipeLeft();
		}

		if (SwipeManager.Instance.IsSwiping(SwipeManager.SwipeDirection.Up))
		{
			OnSwipeUp();
		}

		if (SwipeManager.Instance.IsSwiping(SwipeManager.SwipeDirection.Down))
		{
			OnSwipeDown();
		}
	}


}