using UnityEngine;
using UnityEngine.UI;

public class StatePanel : MonoBehaviour {

    public Text text;
    string textIK = "- Inverse Kinematics -";
    string textFK = "- Forward Kinematics -";
    public void SetText(bool isIK) {
        text.color = isIK ? Color.red : Color.green;
        text.text = isIK ? textIK : textFK;
    }

}