using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class LightPuzzle : Puzzle {
    public LayerMask raycastLayer;

    protected bool isHit = false;
    protected Vector2 hitPoint;
    protected RaycastHit2D hit;
    protected Rigidbody2D hitBody;
    protected LightDetectorPuzzle hitScript;

    void Start() {
        isTriggered = false;
    }
    
    public override void Update() {
        base.Update();
        ShotLight();
    }

    void FixedUpdate() {
        Vector2 direction = new Vector2(transform.TransformDirection(Vector3.up).x, transform.TransformDirection(Vector3.up).y);
        hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, raycastLayer.value);
        if (hit.rigidbody != null) {
            hitBody = hit.rigidbody;
            hitPoint = hit.point;
        } else {
            hitBody = null;
            hitPoint = new Vector2(0.0f, 0.0f);
        }
    }

    void OnDrawGizmos() {
        float distance = 12.0f;
        if (hitBody != null) {
            distance = Vector3.Distance(transform.position, hitPoint);
        }
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.up) * distance;
        Gizmos.DrawRay(transform.position, direction);
    }

    protected void ShotLight() {
        if (hitBody != null && hitBody.gameObject.layer == LayerMask.NameToLayer("LightDetector")) {
            hitScript = (hitBody.transform.parent.gameObject).GetComponent<LightDetectorPuzzle>();
            Assert.IsNotNull(hitScript);
            LightInfo info = new LightInfo();
            info.name = gameObject.ToString();
            info.eulerAngles = transform.eulerAngles;
            info.raycastLayer = raycastLayer;
            info.hitPoint = hitPoint;
            hitScript.AddLightPoint(gameObject, info);
            isHit = true;
        } else {
            RemoveLightPointInDetector();
        }
    }

    public void RemoveLightPointInDetector() {
        // remove light point in Detector
        if (hitScript && isHit) {
            hitScript.RemoveLightPoint(gameObject);
            isHit = false;
        }
    }
}
