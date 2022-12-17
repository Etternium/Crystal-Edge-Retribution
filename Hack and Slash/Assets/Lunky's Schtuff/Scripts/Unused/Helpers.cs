using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Helpers
{
    private static Camera camera;

    //use this for better performance, gets refernce to main camera
    public static Camera Camera
    {
        get
        {
            if (camera == null)
                camera = Camera.main;

            return camera;
        }
    }

    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();

    //use this instead of regular WaitForSeconds when using Coroutines, better for performance
    public static WaitForSeconds GetWait(float time)
    {
        if (WaitDictionary.TryGetValue(time, out var wait))
            return wait;

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }

    private static PointerEventData eventDataCurrentPosition;
    private static List<RaycastResult> results;

    //is mouse over a UI element
    public static bool IsOverUi()
    {
        eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    //spawning particles or 3D objects on canvas
    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera, out var result);
        return result;
    }

    //delete children game objects
    public static void DeleteChildren(this Transform t)
    {
        foreach (Transform child in t)
            Object.Destroy(child.gameObject);
    }
}
