using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;

[RequireComponent(typeof(SwipeManager))]
public class InputController : MonoBehaviour
{
    public float magnatudeAdjuster = 1000f;
    
    private Vector2 swipeDelta;

    public Paddle paddle;
    
    void Start()
    {
        SwipeManager swipeManager = GetComponent<SwipeManager>();
        swipeManager.AddSwipe(HandleSwipe);
        swipeManager.AddLongPress(HandleLongPress);
    }

    void HandleSwipe(SwipeAction swipeAction)
    {
        if (swipeAction.direction == SwipeDirection.LeftRight) {
            //Debug.LogFormat("HandleSwipe: {0}", swipeAction);
            paddle.Move(swipeAction.rawDirection.x / (swipeAction.duration * 1000f) / magnatudeAdjuster);
        }
    }

    void HandleLongPress(SwipeAction swipeAction)
    {
        //Debug.LogFormat("HandleLongPress: {0}", swipeAction);
        GM.instance.StartGame();
    }
}
