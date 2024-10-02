using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public Joint[] joints;
    Joint controllerPoint;
    Joint fixedPoint;
    Vector2 fixedPos;

    void Start() {
        if (joints == null || joints.Length == 0) {
            return;
        }
        fixedPoint = joints[0];
        controllerPoint = joints[joints.Length - 1];
        fixedPoint.isFixedPoint = true;
        controllerPoint.isControllerPoint = true;
        fixedPos = fixedPoint.Pos;
    }

    void Update() {
        var isDragging = InputDomain.TryDrag(controllerPoint.transform, controllerPoint.R);
        int maxIterations = 10; // 最大迭代次数
        int iterations = 0;
        while (isDragging && iterations < maxIterations) {
            UpdateAllJoint_IK(isDragging);
            iterations++;

            ResumeFixedPoint();
            UpdateAllJoint_FK(isDragging);
        }
        UpdateAllJoint_IK(isDragging);
    }

    void ResumeFixedPoint() {
        fixedPoint.SetPos(fixedPos);
    }

    void UpdateAllJoint_IK(bool isDragging) {
        if (!isDragging) {
            return;
        }
        for (int i = joints.Length - 1; i > 0; i--) {
            var current = joints[i].Pos;
            var r = joints[i].R;
            var p = JointDomain.UpdateProjection(current, r, joints[i - 1].Pos);
            joints[i - 1].SetPos(p);
        }
    }

    void UpdateAllJoint_FK(bool isDragging) {
        if (!isDragging) {
            return;
        }
        for (int i = 0; i < joints.Length - 1; i++) {
            var current = joints[i].Pos;
            var r = joints[i].R;
            var p = JointDomain.UpdateProjection(current, r, joints[i + 1].Pos);
            joints[i + 1].SetPos(p);
        }
    }


    void OnDestroy() {
        fixedPoint = null;
        controllerPoint = null;
    }

}