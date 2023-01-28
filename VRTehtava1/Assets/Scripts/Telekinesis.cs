using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Telekinesis : MonoBehaviour
{
    public InputActionReference rotationReference_Left = null;
    public InputActionReference rotationReference_Right = null;

    public TMP_Text leftControllerRotationText;
    public TMP_Text rightControllerRotationText;


    // Update is called once per frame
    void Update()
    {
        leftControllerRotationText.text = "L: " + rotationReference_Left.action.ReadValue<Quaternion>().eulerAngles.ToString();
        rightControllerRotationText.text = "R: " + rotationReference_Right.action.ReadValue<Quaternion>().eulerAngles.ToString();
    }
}
