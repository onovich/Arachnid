using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public Joint[] joints;
    Joint controllerPoint;
    Joint fixedPoint;

    void Start() {
        if (joints == null || joints.Length == 0) {
            return;
        }
        fixedPoint = joints[0];
        controllerPoint = joints[joints.Length - 1];
        fixedPoint.isFixedPoint = true;
        controllerPoint.isControllerPoint = true;
    }

    void Update() {
        _ = InputDomain.TryDrag(controllerPoint.transform);
        var isDragging = InputDomain.IsDragging();
        if (isDragging) {
            UpdateAllJoint();
        }
    }

    void UpdateAllJoint() {
        for (int i = joints.Length - 1; i > 0; i--) {
            if (i == 0) {
                continue;
            }
            var current = joints[i].Pos;
            var r = joints[i].R;
            var p = JointDomain.UpdateProjection(current, r, joints[i - 1].Pos);
            joints[i - 1].SetPos(p);
        }
    }

    void OnDestroy() {
        fixedPoint = null;
        controllerPoint = null;
    }

}