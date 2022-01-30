using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Vector3 LobbyPos;
    public Vector3 LobbyRot;
    public Vector3 StagePos;
    public Vector3 StageRot;    

    public void SetLobbyCam()
	{
        this.transform.position = LobbyPos;
        this.transform.rotation = Quaternion.Euler(LobbyRot);
	}

    public void SetStageCam()
    {
        this.transform.DOMove(StagePos, 1f);
        this.transform.DORotate(StageRot, 1f);        
    }

    public void SetBattleCam(Vector3 pos)
	{
        this.transform.DOMove(pos, 1.5f);
	}

}
