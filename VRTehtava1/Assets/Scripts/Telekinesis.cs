using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class Telekinesis : MonoBehaviour
{
    public Transform leftControllerTr;
    public Transform rightControllerTr;
    public InputActionReference rotationReference_Left = null;
    public InputActionReference rotationReference_Right = null;

    [Header("UI")]
    public TMP_Text leftControllerRotationText;
    public TMP_Text rightControllerRotationText;
    public TMP_Text leftControllerHoveringText;
    public TMP_Text rightControllerHoveringText;

    // Keep track of controllers roll-rotations
    List<float> rollRotations_Left = new List<float>();
    List<float> rollRotations_Right = new List<float>();

    // Objects which have been grabbed
    private Rigidbody leftHandGrabbee;
    private Rigidbody rightHandGrabbee;


    void Update()
    {
        leftHandGrabbee = null;
        rightHandGrabbee = null;

        RaycastHit hit;
        if (Physics.Raycast(leftControllerTr.position, leftControllerTr.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.GetComponent<Rigidbody>())
            {
                leftHandGrabbee = hit.collider.gameObject.GetComponent<Rigidbody>();
            }
        }

        RaycastHit hit2;
        if (Physics.Raycast(rightControllerTr.position, rightControllerTr.forward, out hit2, Mathf.Infinity))
        {
            if (hit2.collider.gameObject.GetComponent<Rigidbody>())
            {
                rightHandGrabbee = hit2.collider.gameObject.GetComponent<Rigidbody>();
            }
        }

        // Get rotation values
        Vector3 leftRotationAngles = rotationReference_Left.action.ReadValue<Quaternion>().eulerAngles;
        Vector3 rightRotationAngles = rotationReference_Right.action.ReadValue<Quaternion>().eulerAngles;

        // Show the rotation values on the screen
        leftControllerRotationText.text = "L: " + leftRotationAngles.ToString();
        rightControllerRotationText.text = "R: " + rightRotationAngles.ToString();
        leftControllerHoveringText.text = "L hovering: " + (leftHandGrabbee != null).ToString();
        rightControllerHoveringText.text = "R hovering: " + (rightHandGrabbee != null).ToString();

        // Keep track of controllers roll-rotations over time
        rollRotations_Left.Add(leftRotationAngles.z);
        rollRotations_Right.Add(rightRotationAngles.z);

        // Limit the lists sizes to 60
        if (rollRotations_Left.Count > 60)
        {
            rollRotations_Left.RemoveAt(0);
        }
        if (rollRotations_Right.Count > 60)
        {
            rollRotations_Right.RemoveAt(0);
        }

        // If the left controller was one second ago upright and now tilted to the right -> telekinesis
        float valueSecondAgo = rollRotations_Left.IndexOf(0);
        float currentValue = leftRotationAngles.z;
        if (
            (valueSecondAgo < 20 || valueSecondAgo > 360 - 20) &&
            (currentValue > 60 && currentValue < 120) &&
            leftHandGrabbee != null
        )
        {
            leftHandGrabbee.transform.position = leftControllerTr.position + leftControllerTr.forward * 5f;
        }

        // If the right controller was one second ago upright and now tilted to the left -> telekinesis
        valueSecondAgo = rollRotations_Right.IndexOf(0);
        currentValue = rightRotationAngles.z;
        if (
            (valueSecondAgo < 20 || valueSecondAgo > 360 - 20) &&
            (currentValue > 60 && currentValue < 120) &&
            rightHandGrabbee != null
        )
        {
            rightHandGrabbee.transform.position = rightControllerTr.position + rightControllerTr.forward * 5f;
        }
    }

    float GetMinValueFromList(List<float> list)
    {
        float minValue = Mathf.Infinity;

        foreach (float value in list)
        {
            if (value < minValue)
            {
                minValue = value;
            }
        }

        return minValue;
    }

    float GetMaxValueFromList(List<float> list)
    {
        float maxValue = -Mathf.Infinity;

        foreach (float value in list)
        {
            if (value > maxValue)
            {
                maxValue = value;
            }
        }

        return maxValue;
    }
}
