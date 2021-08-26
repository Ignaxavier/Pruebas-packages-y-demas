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
    //Walk
    public      float       _movementSpeed = 150f;
    public      float       _walkFrequency;
    public      float       _walkMagnitude;
    public      float       _walkFOV = 60f;

    //Jump
    public      float       _jumpForce = 8f;

    //Scale
    public      float       _scale = 2f;
    public      float       _colliderHeight = 2f;
    public      float       _colliderRadius = 0.5f;

    //Sprint
    public      float       _sprintMultiply = 2f;  //Speed Multiply
    public      float       _sprintFrequency;
    public      float       _sprintMagnitude;
    public      float       _sprintFOV = 70f;

    //Crouch
    public      float       _crouchHeigth = 0.20f;
    public      float       _crouchPenalized;  //Speed Penalized

    //Zoom
    public      float       _ZoomPenalized = 75f;  //Speed Penalized
    public      float       _zoomFOV = 10f;

    //Camera
    public      float       _cameraSensitive = 100f;
    public      float       _cameraNegativeAngle = -90f;
    public      float       _cameraPositiveAngle = 90f;

    //Others
    private     float       movementSpeedRegister;
    #endregion

    #region Int
    //Jump
    private     int         countToCanJumpRegister;
    public      int         _countToCanJump = 2;
    #endregion

    #region Bool
    //Walk
    public      bool        _canWalk;
    public      bool        _isWalking;
    private     bool        stop;

    //Sprint
    public      bool        _canSprint;
    public      bool        _isSprinting;
    private     bool        isSprint;

    //Jump
    public      bool        _canJump;
    public      bool        _isJumping;
    public      bool        _multipleJumps;
    private     bool        isGroud;

    //Crouch
    public      bool        _canCrouch;
    public      bool        _isCrouching;
    public      bool        _holdCrounchButton;
    public      bool        _pressCrounchButton;

    //Zoom
    private     bool        isZoom;
    public      bool        _canZoom;
    public      bool        _stopWalkingInTheZoom;
    public      bool        _penalizedWalkingSpeedInTheZoom;
    #endregion

    #region Input
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

        #region Settings
        HeightAndRadius();
        CameraSettings();
        Scale();
        #endregion

        _col.isTrigger = false;
        _groundCol.isTrigger = true;

        _cam.fieldOfView = _walkFOV;

        countToCanJumpRegister = _countToCanJump;
        movementSpeedRegister = _movementSpeed;


        if (_canCrouch && !_holdCrounchButton && !_pressCrounchButton)
        {
            _pressCrounchButton = true;
        }
    }

    void Update()
    {
       /* #region Settings
        HeightAndRadius();
        CameraSettings();
        Scale();
        #endregion
       */
        #region Basic Actions
        Move();
        Jump();
        Sprint();
        Zoom();
        Crouch();
        #endregion
    }

    #region Basic Actions
    private void Move()
    {
        inputVec = transform.forward * Input.GetAxis(_axisZInput);
        inputVec += transform.right * Input.GetAxis(_axisXInput);
        inputVec.y = -transform.up.y;

        if (inputVec.sqrMagnitude > 1)
        {
            inputVec.Normalize();
        }

        if ((inputVec.x != 0f || inputVec.z != 0f) && !stop && _canWalk)
        {
            _rb.velocity = inputVec * _movementSpeed * Time.deltaTime;
            _isWalking = true;
        }
        else
        {
            _isWalking = false;
        }
    }

    private void Jump()
    {
        if (_canJump)
        {
            if(Input.GetButtonDown(_jumpInput) && isGroud)
            {
                _rb.AddForce(Vector3.up * _jumpForce * Time.deltaTime, _jumpForceMode);

                _isJumping = true;

                if (_multipleJumps)
                {
                    if(_countToCanJump != 0)
                    {
                        _countToCanJump--;
                    }
                    else
                    {
                        isGroud = false;
                        _isJumping = false;
                    }
                }
                else
                {
                    isGroud = false;
                    _isJumping = false;
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
                    _isSprinting = true;
                    _movementSpeed *= _sprintMultiply;
                    _cam.fieldOfView = _sprintFOV;
                    isSprint = true;
                    _camLook._isRuning = true;
                }
            }
            else if (Input.GetButtonUp(_sprintInput))
            {
                _isSprinting = false;
                _movementSpeed = movementSpeedRegister;
                _cam.fieldOfView = _walkFOV;
                isSprint = false;
                _camLook._isRuning = false;
            }
        }
    }

    #region Crouch
    private void Crouch()
    {
        if (_canCrouch)
        {
            if(_pressCrounchButton && !_holdCrounchButton)
            {
                Crouching();
            }
            else if (!_pressCrounchButton && _holdCrounchButton)
            {
                Crouching();
            }
        }
    }

    private void Crouching()
    {
        if (Input.GetButtonDown(_crouchInput))
        {
            _col.height = _crouchHeigth;
            _movementSpeed -= _crouchPenalized;
            _isCrouching = true;
        }
        else if (Input.GetButtonUp(_crouchInput))
        {
            _col.height = _colliderHeight;
            _movementSpeed = movementSpeedRegister;
            _isCrouching = false;
        }
    }
    #endregion

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
            else if (Input.GetButtonUp(_zoomInput))
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

    #endregion

    #region Settings

    //This Affect the transform Scale, not the capsule collider
    private void Scale()
    {
        transform.localScale = new Vector3(_scale, _scale, _scale);
    }

    //This affect the capsule collider
    private void HeightAndRadius()
    {
        _col.height = _colliderHeight;
        _col.radius = _colliderRadius;
    }

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
