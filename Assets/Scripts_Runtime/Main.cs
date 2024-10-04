using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public Joint[] joints;
    public float[] jointsDistance;
    public bool isSoftJoint;
    public int maxIterations = 10;
    public float inputDistanceMax = 0.5f;
    public bool setIK;
    public StatePanel statePanel;

    bool isIK;
    bool isReverse = true;
    int controllerPointIndex;
    Joint controllerPoint => joints[controllerPointIndex];
    int fixedPointIndex;
    Joint fixedPoint => joints[fixedPointIndex];
    int temp;
    Vector2 fixedPos;

    void SetIK(bool isIK) {
        if (this.isIK == false && isIK == true) {
            fixedPos = fixedPoint.Pos;
        }
        if (this.isIK != isIK) {
            statePanel.SetText(isIK);
            this.isIK = isIK;
        }
    }

    float GetNextJointDistance(int jointIndex, bool isReverse) {
        if (isReverse) {
            if (jointIndex == 0) {
                return 0;
            }
            return jointsDistance[jointIndex - 1];
        } else {
            if (jointIndex == joints.Length - 1) {
                return 0;
            }
            return jointsDistance[jointIndex];
        }
    }

    void Start() {
        if (joints == null || joints.Length == 0) {
            return;
        }
        fixedPointIndex = 0;
        controllerPointIndex = joints.Length - 1;
        fixedPoint.isFixedPoint = true;
        controllerPoint.isControllerPoint = true;
        fixedPos = fixedPoint.Pos;
        if (isIK) {
            JointDomain.UpdateJoints_IK(maxIterations, joints, (index, isReverse) => GetNextJointDistance(index, isReverse), fixedPoint, fixedPos, isReverse, isSoftJoint, true);
        } else {
            JointDomain.UpdateJoints_FK(joints, (index, isReverse) => GetNextJointDistance(index, isReverse), isReverse, isSoftJoint, true);
        }
        setIK = isIK;
        statePanel.SetText(isIK);
    }

    void Update() {
        var isClickingC = InputDomain.IsClicking(controllerPoint.transform, inputDistanceMax);
        if (isClickingC) {
            controllerPoint.SetPos(InputDomain.GetMousePos());
        } else {
            var isClickingF = InputDomain.IsClicking(fixedPoint.transform, inputDistanceMax);
            if (isClickingF) {
                fixedPoint.SetPos(InputDomain.GetMousePos());
                Swap();
                isReverse = !isReverse;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            setIK = !setIK;
        }

        if (setIK != isIK) {
            SetIK(setIK);
        }

        if (isIK) {
            JointDomain.UpdateJoints_IK(maxIterations, joints, (index, isReverse) => GetNextJointDistance(index, isReverse), fixedPoint, fixedPos, isReverse, isSoftJoint, isClickingC);
        } else {
            JointDomain.UpdateJoints_FK(joints, (index, isReverse) => GetNextJointDistance(index, isReverse), isReverse, isSoftJoint, isClickingC);
        }
    }

    void Swap() {
        temp = fixedPointIndex;
        fixedPointIndex = controllerPointIndex;
        controllerPointIndex = temp;
        fixedPos = fixedPoint.Pos;
    }

    void OnDestroy() {
    }

    void OnDrawGizmos() {
        if (joints == null || joints.Length == 0) {
            return;
        }
        for (int i = 0; i < joints.Length - 1; i++) {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(joints[i].Pos, joints[i + 1].Pos);
            if (i == 0) {
                continue;
            }
            Gizmos.DrawCube(joints[i].Pos, Vector3.one * 0.2f);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawCube(fixedPoint.Pos, Vector3.one * 0.2f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(controllerPoint.Pos, Vector3.one * 0.2f);
    }

}