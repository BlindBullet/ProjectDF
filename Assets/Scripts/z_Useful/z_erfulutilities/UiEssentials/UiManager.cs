using System;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : SingletonObject<UiManager>
{
	public UiCamera CameraPrefab;

	private UiModelBase _defaultModelCache = new UiModelBase();

	private Dictionary<Type, UiViewBase> _dialogs = new Dictionary<Type, UiViewBase>();

	private UiCamera __uiCam;

	public UiCamera UiCam
	{
		get
		{
			if (ApplicationIsQuitting)
			{
				return null;
			}
			if (__uiCam == null)
			{
				__uiCam = FindObjectOfType<UiCamera>();
				if (__uiCam == null)
				{
					__uiCam = Instantiate(UiPrefabResource.Ins.CameraPrefab);
				}
			}
			return __uiCam;
		}
	}

	private Camera __mainCam;

	public Camera MainCam
	{
		get
		{
			if (__mainCam == null)
			{
				__mainCam = Camera.main;
			}
			return __mainCam;
		}
	}


	public Vector3 GetMainCameraWorldPos(Vector3 uiPosotion)
	{
		var uiScreenPos = RectTransformUtility.WorldToScreenPoint(UiCam.Camera, uiPosotion);
		var ray = RectTransformUtility.ScreenPointToRay(MainCam, uiScreenPos);
		return ray.origin;
	}

	public T SetView<T, V>(V model = null) where T : UiView<V> where V : UiModelBase
	{
		if (model == null)
		{
			model = (V)_defaultModelCache;
		}

		var type = typeof(T);
		if (!_dialogs.ContainsKey(type))
		{
			T dialog = (T)UiPrefabResource.Ins.GetDialogPrefab(typeof(T));
			_dialogs[type] = Instantiate(dialog, UiCam.MainCanvas.transform);
		}
		var res = (T)_dialogs[type];
		res.Open(model);

		return res;
	}

	public T GetDialog<T>() where T : UiViewBase
	{
		if (_dialogs.ContainsKey(typeof(T)))
		{
			return (T)_dialogs[typeof(T)];
		}
		return null;
	}
}