using System;
using UnityEngine;

public static class JointDomain {

    public static void UpdateJoints(int maxIterations, Joint[] joints, Joint fixedPoint, Vector2 fixedPos, bool isIK, bool isSoftJoint, bool isDragging) {
        int iterations = 0;
        while (isDragging && iterations < maxIterations) {
            UpdateJoints_Fabric_Once(joints, isIK, isSoftJoint);
            iterations++;

            ResumeFixedPoint(fixedPos, fixedPoint);
            UpdateJoints_Fabric_Once(joints, !isIK, isSoftJoint);
        }
    }

    static void ResumeFixedPoint(Vector2 fixedPos, Joint fixedPoint) {
        fixedPoint.SetPos(fixedPos);
    }

    static void UpdateJoints_Fabric_Once(Joint[] joints, bool isIK, bool isSoftJoint) {
        if (joints == null || joints.Length == 0) {
            return;
        }
        if (isIK) {
            for (int i = joints.Length - 1; i > 0; i--) {
                var current = joints[i].Pos;
                var r = joints[i].R;
                var p = UpdateProjection(current, r, joints[i - 1].Pos, isSoftJoint);
                joints[i - 1].SetPos(p);
            }
        } else {
            for (int i = 0; i < joints.Length - 1; i++) {
                var current = joints[i].Pos;
                var r = joints[i].R;
                var p = UpdateProjection(current, r, joints[i + 1].Pos, isSoftJoint);
                joints[i + 1].SetPos(p);
            }
        }
    }

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