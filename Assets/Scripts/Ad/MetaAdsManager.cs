using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudienceNetwork;

public class MetaAdsManager : MonoBehaviour
{

    void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AudienceNetworkAds.Initialize();
#endif
    }

}
