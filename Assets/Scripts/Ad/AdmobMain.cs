using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobMain : MonoBehaviour
{    
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
    }

}
