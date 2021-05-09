using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(BoxCollider))]
public class CharacterFPController : MonoBehaviour
{
    #region Variable

    #region Component
    Rigidbody _rb;
    CapsuleCollider _col;
    BoxCollider _groundCol;
    Camera _cam;
    FPCameraLook _camLook;
    #endregion

    #region Float
    [Header("Floats")]
    public      float       _life = 100f;
    public      float       _movementSpeed = 150f;
    public      float       _jumpForce = 8f;
    public      float       _scale = 2f;
    public      float       _sprintMultiply = 2f;
    public      float       _sprintFrequency;
    public      float       _sprintMagnitude;
    public      float       _ZoomPenalized = 75f;
    public      float       _cameraSensitive = 100f;
    public      float       _cameraNegativeAngle = -90f;
    public      float       _cameraPositiveAngle = 90f;
    public      float       _walkFOV = 60f;
    public      float       _sprintFOV = 70f;
    public      float       _zoomFOV = 10f;
    public      float       _originalLife;
    public      float       _crouchHeigth = 0.20f;
    private     float       standarHeight;
    private     float       originalScale;
    private     float       movementSpeedRegister;
    #endregion

    #region Int
    [Header("Ints")]
    private     int         countToCanJumpRegister;
    public      int         _countToCanJump = 2;
    #endregion

    #region Bool
    [Header("Bools")]
    private     bool        isGroud;
    private     bool        isSprint;
    private     bool        isZoom;
    private     bool        stop;
    public      bool        _canWalk;
    public      bool        _multipleJumps;
    public      bool        _canJump;
    public      bool        _canSprint;
    public      bool        _canCrouch;
    public      bool        _canZoom;
    public      bool        _stopWalkingInTheZoom;
    public      bool        _penalizedWalkingSpeedInTheZoom;
    public      bool        _holdCrounchButton;
    public      bool        _pressCrounchButton;
    #endregion

    #region Input
    [Header("Inputs")]
    public      string      _axisXInput = "Horizontal";
    public      string      _axisZInput = "Vertical";
    public      string      _jumpInput = "Jump";
    public      string      _sprintInput = "Sprint";
    public      string      _zoomInput = "Zoom";
    public      string      _crouchInput = "Crouch";
    #endregion

    public      ForceMode       _jumpForceMode = ForceMode.Impulse;

    private     Vector3         inputVec;

    #endregion

    void Awake()
    {
        #region GetComponent
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _groundCol = GetComponent<BoxCollider>();
        _cam = GetComponentInChildren<Camera>();
        _camLook = GetComponentInChildren<FPCameraLook>();
        #endregion

        _cam.fieldOfView = _walkFOV;

        countToCanJumpRegister = _countToCanJump;
        movementSpeedRegister = _movementSpeed;

        CameraSettings();
        Scale();
        _originalLife = _life;
        standarHeight = _col.height;
    }

    void Update()
    {
        #region Basic Actions
        Move();
        Jump();
        Sprint();
        Zoom();
        Crouch();
        #endregion

        #region Settings
        CameraSettings();
        ChangeScale(_scale);
        #endregion
    }

    #region Basic Actions
    private void Move()
    {
        inputVec = (transform.forward * Input.GetAxis(_axisZInput) + transform.right * Input.GetAxis(_axisXInput)) * _movementSpeed * Time.deltaTime;

        this.inputVec.y = _rb.velocity.y;
        /*if(inputVec.sqrMagnitude > 1)
        {
            inputVec.Normalize();
        }*/

        if((inputVec.x != 0f || inputVec.z != 0f) && !stop && _canWalk)
        {
            _rb.velocity = inputVec;
        }
    }

    private void Jump()
    {
        if (_canJump)
        {
            if(Input.GetButtonDown(_jumpInput) && isGroud)
            {
                _rb.AddForce(Vector3.up * _jumpForce * Time.deltaTime, _jumpForceMode);

                if (_multipleJumps)
                {
                    if(_countToCanJump != 0)
                    {
                        _countToCanJump--;
                    }
                    else
                    {
                        isGroud = false;
                    }
                }
                else
                {
                    isGroud = false;
                }
            }
        }
    }

    private void Sprint()
    {
        if (_canSprint)
        {
            if(Input.GetButton(_sprintInput) && !isZoom && inputVec.z != 0f)
            {
                if (!isSprint)
                {
                    _movementSpeed *= _sprintMultiply;
                    _cam.fieldOfView = _sprintFOV;
                    isSprint = true;
                    _camLook._isRuning = true;
                }
            }
            else
            {
                _movementSpeed = movementSpeedRegister;
                _cam.fieldOfView = _walkFOV;
                isSprint = false;
                _camLook._isRuning = false;
            }
        }
    }

    private void Crouch()
    {
        if (_canCrouch)
        {
            if(_pressCrounchButton && !_holdCrounchButton)
            {
                if (Input.GetButtonDown(_crouchInput))
                {
                    if(_col.height == standarHeight)
                    {
                        _col.height = _crouchHeigth;
                    }
                    else if (_col.height == _crouchHeigth)
                    {
                        _col.height = standarHeight;
                    }
                }
            }
            else if (!_pressCrounchButton && _holdCrounchButton)
            {
                if (Input.GetButtonDown(_crouchInput))
                {
                    _col.height = _crouchHeigth;
                }
                else if (Input.GetButtonUp(_crouchInput))
                {
                    _col.height = standarHeight;
                }
            }
        }
    }

    private void Zoom()
    {
        if (_canZoom)
        {
            if (Input.GetButton(_zoomInput))
            {
                _cam.fieldOfView = _zoomFOV;
                isZoom = true;

                if (_stopWalkingInTheZoom && !_penalizedWalkingSpeedInTheZoom)
                {
                    stop = true;
                }
                else if(_penalizedWalkingSpeedInTheZoom && !_stopWalkingInTheZoom)
                {
                    _movementSpeed -= _ZoomPenalized;
                }

            }
            else
            {
                _cam.fieldOfView = _walkFOV;
                isZoom = false;

                if (_stopWalkingInTheZoom && !_penalizedWalkingSpeedInTheZoom)
                {
                    stop = false;
                }
                else if (_penalizedWalkingSpeedInTheZoom && !_stopWalkingInTheZoom)
                {
                    _movementSpeed = movementSpeedRegister;
                }
            }
        }
    }

    public void GetDmage(float value)
    {
        if(value != 0)
        {
            _life -= value;
        }
    }

    #endregion

    #region Settings

    #region General Scale

    //This Affect the transform Scale, not the capsule collider
    private void Scale()
    {
        originalScale = _scale;
        transform.localScale = new Vector3(_scale, _scale, _scale);
    }

    private void ChangeScale(float value)
    {
        if(value != originalScale)
        {
            transform.localScale = new Vector3(_scale, _scale, _scale);

            originalScale = _scale;
        }
    }
    #endregion 

    private void CameraSettings()
    {
        _camLook._mouseSensitive = _cameraSensitive;
        _camLook._negativeAngle = _cameraNegativeAngle;
        _camLook._positiveAngle = _cameraPositiveAngle;
    }
    #endregion

    private void OnTriggerEnter(Collider box)
    {
        if(_groundCol.gameObject != null)
        {
            isGroud = true;

            if (_multipleJumps)
            {
                _countToCanJump = countToCanJumpRegister;
            }
        }
    }
}
