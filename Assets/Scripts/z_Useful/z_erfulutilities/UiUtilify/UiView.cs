using UnityEngine;

public abstract class UiViewBase : ObserveBehaviour
{
	[SerializeField]
	public CanvasGroup Canvas;
}

public abstract class UiView<T> : UiViewBase where T : UiModelBase
{
	[SerializeField]
	public OrderingGroup Group;

	private bool _isInitalized = false;

	private bool _isSceneObject = true;

	private UiModelBase _trackedModel;
	private RectTransform _rtf;

	public enum OrderingGroup
	{
		None, Hud, Full, Popup,
	}

	public RectTransform Rtf
	{
		get
		{
			if (_rtf == null) _rtf = GetComponent<RectTransform>();
			return _rtf;
		}
	}

	public T Model
	{
		get { return (T) _trackedModel; }
	}

	public T GetModel()
	{
		return (T)_trackedModel;
	}

	public void Show()
	{
		gameObject.SetActive(true);
		gameObject.transform.SetAsLastSibling();
	}

	public void Start()
	{
		if (_isSceneObject)
		{
			Destroy(this.gameObject);
		}
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	public void Open(T model)
	{
		_isSceneObject = false;

		bool isModelChanged = _trackedModel != model;
		if (isModelChanged)
		{
			_trackedModel = model;
			this.CacnelAllBroadcasts();
		}

		if (!_isInitalized)
		{
			OnInit();
		}

		OnOpen(model, isModelChanged);
	}

	public virtual void UseBackKey()
	{
	}

	protected virtual void OnInit()
	{
	}

	protected override void OnUpdated()
	{
	}

	protected abstract void OnOpen(T model, bool isModelChanged);
}