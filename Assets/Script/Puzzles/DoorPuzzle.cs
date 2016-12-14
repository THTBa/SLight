using UnityEngine;
using UnityEngine.Assertions;

public class DoorPuzzle : Puzzle {
    private bool acting = false;
    private Vector3 Init_position;
    public Vector3 Moved_position;
    public float Move_Time = 15.0f;
    void Start() {
        //sprite = transform.Find("Sprite");
        //Assert.IsNotNull(sprite);
        Init_position = transform.position;
    }
    
    public override void Update() {
        base.Update();
        if (GetTrigger() && !acting) {
            OnTriggerDown();
            acting = true;
        } else if (!GetTrigger() && acting) {
            OnTriggerUp();
            acting = false;
        }
    }

    void OnTriggerDown() {
        GameKernel.actionManager.RunAction(new ActionMoveBy(gameObject, Moved_position, Move_Time));
    }

    void OnTriggerUp() {
        GameKernel.actionManager.RunAction(new ActionMoveTo(gameObject, Init_position, 2.0f));
    }
}
