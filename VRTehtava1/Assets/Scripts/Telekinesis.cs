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

    // Controller angle at the moment of ray entering collider 
    float previousRollValueLeft = 0f;
    float previousRollValueRight = 0f;

    // Objects which have been grabbed
    private Rigidbody leftHandGrabbee;
    private Rigidbody rightHandGrabbee;

    // How much hand needs to rotate to activate telekinesis
    private float telekinesisAngle = 45f;


    void Update()
    {
        // Get rotation values
        Vector3 leftRotationAngles = rotationReference_Left.action.ReadValue<Quaternion>().eulerAngles;
        Vector3 rightRotationAngles = rotationReference_Right.action.ReadValue<Quaternion>().eulerAngles;

        float leftAngle = leftRotationAngles.z;
        float rightAngle = rightRotationAngles.z;

        // Convert rotation angles to be between [-180f, 180f] with 0f as the default angle
        leftAngle = leftAngle > 180f ? leftAngle - 360f : leftAngle;
        rightAngle = rightAngle > 180f ? rightAngle - 360f : rightAngle;

        // Left controller raycast
        RaycastHit hit;
        if (Physics.Raycast(leftControllerTr.position, leftControllerTr.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.GetComponent<Rigidbody>())
            {
                if (leftHandGrabbee == null)
                {
                    previousRollValueLeft = leftAngle;
                }
                leftHandGrabbee = hit.collider.gameObject.GetComponent<Rigidbody>();
            }
            else
            {
                leftHandGrabbee = null;
            }
        }
        else
        {
            leftHandGrabbee = null;
        }

        // Right controller raycast
        RaycastHit hit2;
        if (Physics.Raycast(rightControllerTr.position, rightControllerTr.forward, out hit2, Mathf.Infinity))
        {
            if (hit2.collider.gameObject.GetComponent<Rigidbody>())
            {
                if (rightHandGrabbee == null)
                {
                    previousRollValueRight = rightAngle;
                }
                rightHandGrabbee = hit2.collider.gameObject.GetComponent<Rigidbody>();
            }
            else
            {
                rightHandGrabbee = null;
            }
        }
        else
        {
            rightHandGrabbee = null;
        }

        // Debug values
        leftControllerHoveringText.text = "L hovering: " + (leftHandGrabbee != null).ToString();
        rightControllerHoveringText.text = "R hovering: " + (rightHandGrabbee != null).ToString();

        // Left controller telekinesis
        if (
            (previousRollValueLeft - leftAngle >= telekinesisAngle) &&
            leftHandGrabbee != null
        )
        {
            leftHandGrabbee.transform.position = leftControllerTr.position + leftControllerTr.forward * 5f;
        }
        leftControllerRotationText.text = string.Format("L now: {0}, L prev: {1}", leftAngle, previousRollValueLeft);

        // Right controller telekinesis
        if (
            (rightAngle - previousRollValueRight >= telekinesisAngle) &&
            rightHandGrabbee != null
        )
        {
            rightHandGrabbee.transform.position = rightControllerTr.position + rightControllerTr.forward * 5f;
        }
        rightControllerRotationText.text = string.Format("R now: {0}, R prev: {1}", rightAngle, previousRollValueRight);
    }
}
