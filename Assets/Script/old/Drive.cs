using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drive : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have

    public void FixedUpdate()
    {
        Debug.Log(Input.GetAxis("Move") + ":" + Input.GetAxis("Brake"));
        float motor = maxMotorTorque * Mathf.Max(0, Input.GetAxis("Move"));
        float brake = maxMotorTorque * -Mathf.Min(0, Input.GetAxis("Brake"));
        Debug.Log("dd" + motor + ":" + brake);
        float steering = maxSteeringAngle * Input.GetAxis("Turn");

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.leftWheel.brakeTorque = brake;
                axleInfo.rightWheel.motorTorque = motor;
                axleInfo.rightWheel.brakeTorque = brake;
            }
        }
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}
