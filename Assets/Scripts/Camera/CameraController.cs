using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public float LobbySize;
    public float StageSize;

    public void SetLobbyCam()
	{
        this.GetComponent<Camera>().orthographicSize = LobbySize;
	}

    public void SetStageCam()
    {
        Camera cam = this.GetComponent<Camera>();
        cam.DOOrthoSize(StageSize, 1f).SetEase(Ease.Linear);
    }


}
