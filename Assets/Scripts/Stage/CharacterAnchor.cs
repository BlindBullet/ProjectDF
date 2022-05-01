using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CharacterAnchor : MonoBehaviour
{
	public List<Anchor> Anchors = new List<Anchor>();
	
	public Transform GetAnchor(string anchorKey)
	{
		return Anchors.FirstOrDefault(elem => elem.Key == anchorKey)?.Trf;
	}

	[Serializable]
	public class Anchor
	{
		public string Key;
		public Transform Trf;
	}

}
