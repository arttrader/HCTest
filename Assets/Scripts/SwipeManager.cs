using System;
using UnityEngine;

public enum SwipeDirection
{
    None, // invalid swipe
    LeftRight,
    UpDown
}

public struct SwipeAction
{
    public SwipeDirection direction;
    public Vector2 rawDirection;
    public Vector2 startPosition;
    public Vector2 endPosition;
    public float startTime;
    public float endTime;
    public float duration;
    public bool longPress;
    public float distance;
    public float longestDistance;
    
    public override string ToString() {
        return string.Format("[SwipeAction: Delta {0}, Dir {1}, Time {2:0.00}s]", rawDirection, direction, duration);
    }
}
    
/// <summary>
/// Swipe manager.
/// BASED ON: http://forum.unity3d.com/threads/swipe-in-all-directions-touch-and-mouse.165416/#post-1516893
/// </summary>
public class SwipeManager : MonoBehaviour
{
    [Range(0f, 200f)]
    public float minSwipeLength = 1f;
    public float longPressDuration = 0.5f;
    
    private Action<SwipeAction> onSwipe;
    private Action<SwipeAction> onLongPress;
    private Vector2 currentSwipe;
    private SwipeAction currentSwipeAction;

    public void AddSwipe(Action<SwipeAction> handleSwipe)
    {
        onSwipe += handleSwipe;
    }

    public void AddLongPress(Action<SwipeAction> handleLongPress)
    {
        onLongPress += handleLongPress;
    }
    
    void Update()
    {
        DetectSwipe();
    }
    
    public void DetectSwipe()
    {
        var touches = InputHelper.GetTouches();
        if (touches.Count > 0)
        {
            Touch t = touches[0];
            
            if (t.phase == TouchPhase.Began)
                ResetCurrentSwipeAction(t);
            
            if (t.phase == TouchPhase.Stationary) {
                UpdateCurrentSwipeAction(t);
                if (!currentSwipeAction.longPress 
                    && currentSwipeAction.duration > longPressDuration 
                    && currentSwipeAction.longestDistance < minSwipeLength)
                {
                    currentSwipeAction.direction = SwipeDirection.None; // Invalidate current swipe action
                    currentSwipeAction.longPress = true;
                    if (onLongPress != null) {
                        onLongPress(currentSwipeAction); // Fire event
                    }
                    ResetCurrentSwipeAction(t);
                    return;
                }
            }

            if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Ended) {
                UpdateCurrentSwipeAction(t);
                // Make sure it was a legit swipe, not a tap, or long press
                if (currentSwipeAction.distance < minSwipeLength || currentSwipeAction.longPress) // Didn't swipe enough or this is a long press
                {
                    currentSwipeAction.direction = SwipeDirection.None; // Invalidate current swipe action
                    ResetCurrentSwipeAction(t);
                    return;
                }

                if (onSwipe != null) {
                    onSwipe(currentSwipeAction); // Fire event
                    ResetCurrentSwipeAction(t);
                }
            }
        }
    }
    
    void ResetCurrentSwipeAction(Touch t)
    {
        currentSwipeAction.duration = 0f;
        currentSwipeAction.distance = 0f;
        currentSwipeAction.longestDistance = 0f;
        currentSwipeAction.longPress = false;
        currentSwipeAction.startPosition = new Vector2(t.position.x, t.position.y);
        currentSwipeAction.startTime = Time.time;
        currentSwipeAction.endPosition = currentSwipeAction.startPosition;
        currentSwipeAction.endTime = currentSwipeAction.startTime;
    }
    
    void UpdateCurrentSwipeAction(Touch t)
    {
        currentSwipeAction.endPosition = new Vector2(t.position.x, t.position.y);
        currentSwipeAction.endTime = Time.time;
        currentSwipeAction.duration = currentSwipeAction.endTime - currentSwipeAction.startTime;
        currentSwipe = currentSwipeAction.endPosition - currentSwipeAction.startPosition;
        currentSwipeAction.rawDirection = currentSwipe;
        currentSwipeAction.direction = GetSwipeDirection(currentSwipe);
        currentSwipeAction.distance = Vector2.Distance(currentSwipeAction.startPosition, currentSwipeAction.endPosition);
        if (currentSwipeAction.distance > currentSwipeAction.longestDistance) // If new distance is longer than previously longest
        {
            currentSwipeAction.longestDistance = currentSwipeAction.distance; // Update longest distance
        }
    }
    
    SwipeDirection GetSwipeDirection(Vector2 direction)
    {
        return Math.Abs(direction.x) > Math.Abs(direction.y) ? SwipeDirection.LeftRight : SwipeDirection.UpDown;
    }
}