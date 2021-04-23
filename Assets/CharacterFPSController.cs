using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(BoxCollider))]
public class CharacterFPSController : MonoBehaviour
{
    #region Variable

    #region Component
    Rigidbody _rb;
    CapsuleCollider _col;
    BoxCollider _groundCol;
    Camera _cam;
    FPSCameraLook _camLook;
    #endregion

    #region Float
    [Header("Floats")]
    public      float       _life = 100f;
    public      float       _movementSpeed = 150f;
    public      float       _jumpForce = 8f;
    public      float       _height = 2f;
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
    public      float        _originalLife;
    private     float       originalHeight;
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
    public      bool        _canZoom;
    public      bool        _stopWalkingInTheZoom;
    public      bool        _penalizedWalkingSpeedInTheZoom;
    #endregion

    #region Input
    [Header("Inputs")]
    public      string      _axisXInput = "Horizontal";
    public      string      _axisZInput = "Vertical";
    public      string      _jumpInput = "Jump";
    public      string      _sprintInput = "Sprint";
    public      string      _zoomInput = "Zoom";
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
        _camLook = GetComponentInChildren<FPSCameraLook>();
        #endregion

        _cam.fieldOfView = _walkFOV;

        countToCanJumpRegister = _countToCanJump;
        movementSpeedRegister = _movementSpeed;

        CameraSettings();
        Height();
        _originalLife = _life;
    }

    void Update()
    {
        #region Basic Actions
        Move();
        Jump();
        Sprint();
        Zoom();
        #endregion

        #region Settings
        CameraSettings();
        ChangeHeight(_height);
        #endregion
    }

    #region Basic Actions
    private void Move()
    {
        this.inputVec.x = Input.GetAxis(_axisXInput);
        this.inputVec.y = _rb.velocity.y;
        this.inputVec.z = Input.GetAxis(_axisZInput);

        if(inputVec.sqrMagnitude > 1)
        {
            inputVec.Normalize();
        }

        inputVec *= _movementSpeed * Time.deltaTime;

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
                    //StartCoroutine(_camLook.CameraShake(true, _sprintShakeIntensityX, _sprintShakeIntensityY));
                    _camLook._isRuning = true;
                }
            }
            else
            {
                _movementSpeed = movementSpeedRegister;
                _cam.fieldOfView = _walkFOV;
                isSprint = false;
                _camLook._isRuning = false;
                //StopAllCoroutines();
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
    #region General Height
    private void Height()
    {
        originalHeight = _height;
        transform.localScale = new Vector3(_height, _height, _height);
    }

    private void ChangeHeight(float value)
    {
        if(value != originalHeight)
        {
            transform.localScale = new Vector3(_height, _height, _height);

            originalHeight = _height;
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
