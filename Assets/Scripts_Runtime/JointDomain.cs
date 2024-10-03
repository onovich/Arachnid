using System;
using UnityEngine;

public static class JointDomain {

    public static void UpdateJoints_FK(Joint[] joints, Func<int, bool, float> GetDistance, bool isReverse, bool isSoftJoint, bool isDragging) {
        if (!isDragging) {
            return;
        }
        UpdateJoints_Fabric_Once(joints, GetDistance, isReverse, isSoftJoint);
    }

    public static void UpdateJoints_IK(int maxIterations, Joint[] joints, Func<int, bool, float> GetDistance, Joint fixedPoint, Vector2 fixedPos, bool isReverse, bool isSoftJoint, bool isDragging) {
        if (!isDragging) {
            return;
        }
        int iterations = 0;
        while (isDragging && iterations < maxIterations) {
            UpdateJoints_Fabric_Once(joints, GetDistance, isReverse, isSoftJoint);
            iterations++;

            ResumeFixedPoint(fixedPos, fixedPoint);
            UpdateJoints_Fabric_Once(joints, GetDistance, !isReverse, isSoftJoint);
        }
    }

    static void ResumeFixedPoint(Vector2 fixedPos, Joint fixedPoint) {
        fixedPoint.SetPos(fixedPos);
    }

    static void UpdateJoints_Fabric_Once(Joint[] joints, Func<int, bool, float> GetDistance, bool isReverse, bool isSoftJoint) {
        if (joints == null || joints.Length == 0) {
            return;
        }
        if (isReverse) {
            for (int i = joints.Length - 1; i > 0; i--) {
                var current = joints[i].Pos;
                var p = UpdateProjection(current, GetDistance(i, isReverse), joints[i - 1].Pos, isSoftJoint);
                joints[i - 1].SetPos(p);
            }
        } else {
            for (int i = 0; i < joints.Length - 1; i++) {
                var current = joints[i].Pos;
                var p = UpdateProjection(current, GetDistance(i, isReverse), joints[i + 1].Pos, isSoftJoint);
                joints[i + 1].SetPos(p);
            }
        }
    }

    public static Vector2 UpdateProjection(Vector2 currentPoint, float distance, Vector2 targetPoint, bool isSoftJoint) {
        Vector2 direction = targetPoint - currentPoint;
        if (isSoftJoint) {
            float d = direction.magnitude;
            if (d <= distance) {
                return targetPoint;
            }
        }
        direction.Normalize();
        return currentPoint + direction * distance;
    }

}