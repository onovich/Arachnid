using UnityEngine;

public class Joint : MonoBehaviour {

    public float radius;
    public bool isFixedPoint;
    public bool isControllerPoint;
    public Vector2 Pos => transform.position;
    public float R => radius;

    public void SetPos(Vector2 pos) {
        transform.position = pos;
    }

    void OnDrawGizmos() {
        if (radius <= 0) {
            return;
        }

        // Draw Circle
        Gizmos.color = Color.green;
        if (isFixedPoint) {
            Gizmos.color = Color.red;
        }
        if (isControllerPoint) {
            Gizmos.color = Color.white;
        }
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}