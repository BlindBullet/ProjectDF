using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickUi : MonoSingleton<JoystickUi>, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	// private bool isInput; // Ãß°¡
	public UnityEvent<Vector2, JoystickState> JoystickControlledDelegate = new UnityEvent<Vector2, JoystickState>();
		
	[Header("Settings")]	
	[SerializeField] private float radio;
	[SerializeField] private RectTransform joystick;	
	[SerializeField] private RectTransform lever;

	private RectTransform rectTransform;
		
	Vector3 startPos;
	Vector3 _pos;
	private Canvas m_Canvas; 
	private int lastId = -2;
	
	void Start()
	{		
		if (transform.root.GetComponent<Canvas>() != null)
		{
			m_Canvas = transform.root.GetComponent<Canvas>();
		}
		else if (transform.root.GetComponentInChildren<Canvas>() != null)
		{
			m_Canvas = transform.root.GetComponentInChildren<Canvas>();
		}
		else
		{
			Debug.LogError("Required at lest one canvas for joystick work.!");
			this.enabled = false;
			return;
		}

		joystick.gameObject.SetActive(false);		
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		joystick.gameObject.SetActive(true);
		Vector3 position = bl_JoystickUtils.TouchPosition(m_Canvas, 0);
		joystick.transform.position = position;		

		startPos = position;
		ControlJoystickLever(eventData, JoystickState.BeginDrag);
	}

	public void OnDrag(PointerEventData eventData)
	{	
		ControlJoystickLever(eventData, JoystickState.OnDrag);
	}

	public void ControlJoystickLever(PointerEventData eventData, JoystickState state)
	{
		Vector3 position = bl_JoystickUtils.TouchPosition(m_Canvas, 0);		
		var inputDir = (position - startPos).normalized;
		var dist = Vector3.Distance(position, startPos);

		if(dist > radio)
		{
			lever.position = startPos + (inputDir * radio);
		}
		else
		{
			lever.position = position;
		}
		
		JoystickControlledDelegate.Invoke(inputDir, state);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		lever.position = Vector2.zero;
		ControlJoystickLever(eventData, JoystickState.EndDrag);
		//JoystickControlledDelegate.Invoke(Vector2.zero, JoystickState.EndDrag);		
		joystick.gameObject.SetActive(false);
	}

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	/// <summary>
	/// Get the touch by the store touchID 
	/// </summary>
	public int GetTouchID
	{
		get
		{
			//find in all touches
			for (int i = 0; i < Input.touches.Length; i++)
			{
				if (Input.touches[i].fingerId == lastId)
				{
					return i;
				}
			}
			return -1;
		}
	}

}

public enum JoystickState
{
	None,
	BeginDrag,
	OnDrag,
	EndDrag,

}