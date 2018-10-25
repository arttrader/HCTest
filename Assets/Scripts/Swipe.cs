using System;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    public float deadZone = 1f;
    public float magnatudeAdjuster = 10f;
    public float longPressDuration = 0.5f;
    
    private bool tap;
    private float swipeX, swipeY;
    private Vector2 startTouch, swipeDelta;
    private bool isDragging;
    
    public Vector2 SwipeDelta { get { return swipeDelta; }}
    public float GetXAxis { get { return swipeX; }}
    public float GetYAxis { get { return swipeY; }}


    public void DetectSwipe()
    {
        swipeX = swipeY = 0;
        tap = false;
        var touches = InputHelper.GetTouches();
        if (touches.Count > 0) {
            Touch t = touches[0];

            if (t.phase == TouchPhase.Began) {
                isDragging = true;
                tap = true;
                startTouch = t.position;
            }
            else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) {
                Reset();
            }
            else if (t.phase == TouchPhase.Moved)
            {
                swipeDelta = touches[0].position - startTouch;
            }
        }

        // Did we cross the deadzone?
        if (swipeDelta.magnitude > deadZone) {
            // direction?
            float dx = swipeDelta.x;
            float dy = swipeDelta.y;

            if (Math.Abs(dx) > Math.Abs(dy)) {
                swipeX = dx / magnatudeAdjuster;
            } else {
                swipeY = dy / magnatudeAdjuster;
            }
            
            Reset();
        }
    }
    
    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDragging = false;
    }
}

