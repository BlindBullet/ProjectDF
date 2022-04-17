using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
    public Button HeroBtn;
    public Button RelicBtn;

    private void Start()
    {
        HeroBtn.onClick.RemoveAllListeners();
        HeroBtn.onClick.AddListener(() => DialogManager.Ins.OpenHero());

        RelicBtn.onClick.RemoveAllListeners();
        RelicBtn.onClick.AddListener(() => DialogManager.Ins.OpenRelic());
    }
}
