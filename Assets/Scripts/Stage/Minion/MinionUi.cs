using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MinionUi : MonoBehaviour
{
	public SpriteRenderer Bg;
	public SpriteRenderer MinionImg;
	public SpriteRenderer Frame;

	public void Setup(MinionChart chart)
	{
		Bg.sprite = Resources.Load<Sprite>("Sprites/Heroes/Bgs/" + chart.Attr.ToString());
		MinionImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Minions/Minions").GetSprite(chart.Model);
		Frame.sprite = Resources.Load<Sprite>("Sprites/Minions/Frames/" + chart.Attr.ToString());
	}

}
