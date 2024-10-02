using UnityEngine;

public static class JointDomain {

    public static Vector2 UpdateProjection(Vector2 currentPoint, float radius, Vector2 targetPoint, bool isSoftJoint) {
        Vector2 direction = targetPoint - currentPoint;
        if (isSoftJoint) {
            float distance = direction.magnitude;
            if (distance <= radius) {
                return targetPoint;
            }
        }
        direction.Normalize();
        return currentPoint + direction * radius;
    }

}