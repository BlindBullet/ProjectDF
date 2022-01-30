using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ComponentHelper
{
    public static Rect CanvasRectToScreenRect(RectTransform rectTrf)
    {        
        Vector2 size = Vector2.Scale(rectTrf.rect.size, rectTrf.lossyScale);
        Rect rect = new Rect(rectTrf.position.x, Screen.height - rectTrf.position.y, size.x, size.y);
        rect.x -= (rectTrf.pivot.x * size.x);
        rect.y -= ((1.0f - rectTrf.pivot.y) * size.y);
        return rect;
    }

    public static Vector2 ScreenToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position)
    {
        //Normalized Vector position considering camera size. (0,0) is lower left, (1,1) top right
        Vector2 temp = camera.ScreenToViewportPoint(position);

        //Calculate position considering our percentage, using our canvas size
        //So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)
        temp.x *= canvas.sizeDelta.x;
        temp.y *= canvas.sizeDelta.y;

        //The result is ready, but, this result is correct if canvas recttransform pivot is 0,0 - left lower corner.
        //But in reality its middle (0.5,0.5) by default, so we remove the amount considering cavnas rectransform pivot.
        //We could multiply with constant 0.5, but we will actually read the value, so if custom rect transform is passed(with custom pivot) ,
        //returned value will still be correct.

        temp.x -= canvas.sizeDelta.x * canvas.pivot.x;
        temp.y -= canvas.sizeDelta.y * canvas.pivot.y;

        return temp;
    }

    public static Vector2 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position)
    {
        Vector2 temp = camera.WorldToViewportPoint(position);

        temp.x *= canvas.sizeDelta.x;
        temp.y *= canvas.sizeDelta.y;

        temp.x -= canvas.sizeDelta.x * canvas.pivot.x;
        temp.y -= canvas.sizeDelta.y * canvas.pivot.y;

        return temp;
    }

    /// <summary>
    /// Relocate target rect when target overcomes boundary
    /// </summary>
    public static Vector3 CorrectBoundaryWorldPos(RectTransform boundary, RectTransform target)
    {
        var boundaryRect = GetWorldRect(boundary);
        var targetRect = GetWorldRect(target);

        Vector3 worldPosRes = targetRect.center.WithZ(target.transform.position.z);

        if (targetRect.xMax > boundaryRect.xMax)
            worldPosRes.x = boundaryRect.xMax - targetRect.width * 0.5f;
        else if (targetRect.xMin < boundaryRect.xMin)
            worldPosRes.x = boundaryRect.xMin + targetRect.width * 0.5f;

        if (targetRect.yMax > boundaryRect.yMax)
            worldPosRes.y = boundaryRect.yMax - targetRect.height * 0.5f;
        else if (targetRect.yMin < boundaryRect.yMin)
            worldPosRes.y = boundaryRect.yMin + targetRect.height * 0.5f;

        worldPosRes.z = boundary.position.z;

        return worldPosRes;
    }


    /// <summary>
    /// Relocate target pos when target overcomes boundary
    /// </summary>
    public static Vector3 CorrectBoundaryWorldPos(RectTransform boundary, Vector2 pos)
    {
        var boundaryRect = GetWorldRect(boundary);

        Vector3 worldPosRes = boundary.rect.center.WithZ(boundary.transform.position.z);

        if (pos.x > boundaryRect.xMax)
            worldPosRes.x = boundaryRect.xMax;
        else if (pos.x < boundaryRect.xMin)
            worldPosRes.x = boundaryRect.xMin;

        if (pos.y > boundaryRect.yMax)
            worldPosRes.y = boundaryRect.yMax;
        else if (pos.y < boundaryRect.yMin)
            worldPosRes.y = boundaryRect.yMin;

        worldPosRes.z = boundary.position.z;

        return worldPosRes;
    }

    public static Vector2 CorrectBoundaryScreenPos(RectTransform boundary, Vector2 pos)
    {
        var boundaryRect = boundary.rect;

        if (pos.x > boundaryRect.xMax)
            pos.x = boundaryRect.xMax;
        else if (pos.x < boundaryRect.xMin)
            pos.x = boundaryRect.xMin;

        if (pos.y > boundaryRect.yMax)
            pos.y = boundaryRect.yMax;
        else if (pos.y < boundaryRect.yMin)
            pos.y = boundaryRect.yMin;

        return pos;
    }

    static public Rect GetWorldRect(RectTransform rt)
    {
        // Convert the rectangle to world corners and grab the top left
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        Vector2 size = corners[2] - corners[0];
        Rect rec = new Rect(corners[0].x, corners[0].y, size.x, size.y);
        return rec;
    }
}