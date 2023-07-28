using UnityEngine;

public static class ScreenUtil
{
    /// <summary>
    /// The screen's world space 2D bounds
    /// </summary>
    public static Rect ScreenBounds { get; private set; }

    static ScreenUtil()
    {
        var mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("No camera tagged as main camera was found!");
            return;
        }

        var topLeftBound = mainCamera.ViewportToWorldPoint(Vector3.zero);
        var bottomRightBound = mainCamera.ViewportToWorldPoint(new Vector3(1f,1f, mainCamera.nearClipPlane));
        var delta = bottomRightBound - topLeftBound;
        ScreenBounds = new Rect(topLeftBound.x, topLeftBound.y, delta.x, delta.y);
    }

}