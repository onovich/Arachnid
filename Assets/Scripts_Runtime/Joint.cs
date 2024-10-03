using UnityEngine;

public class Joint : MonoBehaviour {

    public bool isFixedPoint;
    public bool isControllerPoint;
    public Vector2 Pos => transform.position;

    public void SetPos(Vector2 pos) {
        transform.position = pos;
    }

}