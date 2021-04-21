using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FPSCameraLook : MonoBehaviour
{
    public      float       _mouseSensitive;
    public      float       _negativeAngle = -90f;
    public      float       _positiveAngle = 90f;
    public      float       _frequency;
    public      float       _magnitude;
    public      Transform   _characterBody;
    public      bool        _isRuning;

    private     float       mouseX;
    private     float       mouseY;
    private CharacterFPSController FPSC;
    private     float       xRotation;
    Vector3 originalPos;


    private void Awake()
    {
        FPSC = GetComponentInParent<CharacterFPSController>();
        _frequency = FPSC._sprintFrequency;
        _magnitude = FPSC._sprintMagnitude;
        originalPos = transform.localPosition;
    }

    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * _mouseSensitive * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * _mouseSensitive * Time.deltaTime;

        _characterBody.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, _negativeAngle, _positiveAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        CameraSprintShake();
    }

    public void CameraSprintShake()
    {
        if (_isRuning)
        {
            if(_frequency != 0 && _magnitude != 0)
            {
                transform.localPosition = new Vector3(Mathf.Clamp((originalPos.x + (Mathf.Sin(Time.time * _frequency) * _magnitude)), originalPos.x - 0.1f, originalPos.x + 0.1f), Mathf.Clamp((originalPos.y + (Mathf.Sin(Time.time * _frequency) * _magnitude)), originalPos.y, originalPos.y + 0.2f), transform.localPosition.z);
            }
        }
    }
}
