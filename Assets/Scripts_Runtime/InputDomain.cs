using UnityEngine;

public static class InputDomain {

    static Vector2 GetMousePos() {
        var screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f;
        return Camera.main.ScreenToWorldPoint(screenPoint);
    }

    public static bool IsDragging() {
        return Input.GetMouseButton(0);
    }

    public static bool TryDrag(Transform target, float distanceMax) {
        var isPressing = Input.GetMouseButton(0);
        if (!isPressing) {
            return false;
        }
        var mousePos = GetMousePos();
        var distance = Vector2.Distance(mousePos, target.position);
        if (distance >= distanceMax) {
            return false;
        }
        target.position = mousePos;
        return true;
    }

}