using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


public abstract class DialogControllerBase : ObserveBehaviour
{
	[SerializeField]
	public CanvasGroup Canvas;
}


public abstract class DialogController<T> : DialogControllerBase where T : UiModelBase
{
	public enum OrderingGroup
	{
		None, Hud, Full, Popup,
	}
	[SerializeField]
	public OrderingGroup Group;

	public Button popupBackPanelBtn;
	[HideInInspector]
	public bool enabledBackKey;

	bool _isInitalized = false;
	bool _isSceneObject = true;
	private UiModelBase _trackedModel;

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

	public void Close()
	{
		gameObject.SetActive(false);
	}

	protected virtual void OnInit()
	{
		//Please Dont type code in here. I hate write base.XXX things whenever i override methods
	}

	protected override void OnUpdated()
	{

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

	protected abstract void OnOpen(T model, bool isModelChanged);

	public virtual void UseBackKey()
	{

	}

	protected override void OnDisabled()
	{
	}


}
