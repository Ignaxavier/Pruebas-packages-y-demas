    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera), typeof(AudioListener))]
public class FPCameraLook : MonoBehaviour
{
    #region Variables
    public      float                   _mouseSensitive;
    public      float                   _negativeAngle = -90f;
    public      float                   _positiveAngle = 90f;
    public      float                   _sprintFrequency;
    public      float                   _sprintMagnitude;
    public      float                   _walkFrequency;
    public      float                   _walkMagnitude;
    public      Transform               _characterBody;
    public      bool                    _isRuning;

    private     float                   mouseX;
    private     float                   mouseY;
    private     CharacterFPController   FPSC;
    private     float                   xRotation;
    private     Vector3                 originalPos;
    #endregion

    private void Awake()
    {
        FPSC = GetComponentInParent<CharacterFPController>();
        originalPos = transform.localPosition;
    }

    void Update()
    {
        CameraControl();
        CameraWalkShake();
        CameraSprintShake();
    }

    public void CameraControl()
    {
        mouseX = Input.GetAxis("Mouse X") * _mouseSensitive * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * _mouseSensitive * Time.deltaTime;

        _characterBody.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, _negativeAngle, _positiveAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public void CameraWalkShake()
    {
        _walkFrequency = FPSC._walkFrequency;
        _walkMagnitude = FPSC._walkMagnitude;

        if (_walkFrequency != 0 && _walkMagnitude != 0)
        {
            transform.localPosition = new Vector3(originalPos.x, originalPos.y + (Mathf.Sin(Time.time * _walkFrequency) * _walkMagnitude), originalPos.z); 
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }

    public void CameraSprintShake()
    {
        if (_isRuning)
        {
            _sprintFrequency = FPSC._sprintFrequency;
            _sprintMagnitude = FPSC._sprintMagnitude;

            if (_sprintFrequency != 0 && _sprintMagnitude != 0)
            {
                transform.localPosition = new Vector3(originalPos.x, originalPos.y + (Mathf.Sin(Time.time * _sprintFrequency) * _sprintMagnitude), originalPos.z);
            }
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }
}
