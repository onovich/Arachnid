using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public Joint[] joints;
    Joint controllerPoint;
    Joint fixedPoint;
    Joint temp;
    Vector2 fixedPos;
    public bool isSoftJoint;
    bool isIK = true;

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
        var isClickingC = InputDomain.IsClicking(controllerPoint.transform, controllerPoint.R);
        if (isClickingC) {
            controllerPoint.SetPos(InputDomain.GetMousePos());
        } else {
            var isClickingF = InputDomain.IsClicking(fixedPoint.transform, fixedPoint.R);
            if (isClickingF) {
                fixedPoint.SetPos(InputDomain.GetMousePos());
                Swap();
                isIK = !isIK;
            }
        }

        int maxIterations = 10; // 最大迭代次数
        JointDomain.UpdateJoints(maxIterations, joints, fixedPoint, fixedPos, isIK, isSoftJoint, isClickingC);
    }

    void Swap() {
        temp = fixedPoint;
        fixedPoint = controllerPoint;
        controllerPoint = temp;
        fixedPos = fixedPoint.Pos;
    }

    void OnDestroy() {
        fixedPoint = null;
        controllerPoint = null;
        temp = null;
    }

}