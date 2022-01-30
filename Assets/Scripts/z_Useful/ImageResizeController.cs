using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ImageResizeController
{	
	public static void ImageResize(Image img)
	{
		RectTransform rect = img.GetComponent<RectTransform>();
		Vector2 originSize = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y);
		float imgWidth = img.sprite.rect.width;
		float imgHeight = img.sprite.rect.height;
		
		rect.sizeDelta = new Vector2(imgWidth * originSize.y / imgHeight, originSize.y);
		
	}

}
