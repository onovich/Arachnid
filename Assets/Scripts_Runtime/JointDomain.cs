using UnityEngine;

public static class JointDomain {

    public static Vector2 UpdateProjection(Vector2 currentPoint, float radius, Vector2 targetPoint) {
        Vector2 direction = targetPoint - currentPoint;
        float distance = direction.magnitude;
        if (distance <= radius) {
            return targetPoint;
        }
        direction.Normalize();
        return currentPoint + direction * radius;
    }

}