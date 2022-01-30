using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {

	public Button _button;
	public Image _image;
	public Text cooltimeText;
	const float _coolTIme = 3f;



	CoolTimer _coolTimer = null;


	// Use this for initialization
	void Start()
	{
		_coolTimer = CoolTimer.CreateCoolTimer<RealCoolTimer>(_coolTIme, false);
		_coolTimer.RestartCooltime();
		_button.onClick.AddListener(() => 
		{
			_coolTimer.RestartCooltime();
		});
	}


	private void Update()
	{
		_image.fillAmount = _coolTimer.GetRatio();
		cooltimeText.text = _coolTimer.GetRemainingCoolTime().ToString();
	}
	[ContextMenu("Do double scale")]
	void Check()
	{
		_coolTimer.ScaleTimer(2);
	}

	[ContextMenu("Do Reset")]
	void ResetTimer()
	{
		_coolTimer.RestartCooltime();
		
	}

	
	
}
