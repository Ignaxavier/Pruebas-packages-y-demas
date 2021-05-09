using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterFPController))]
public class FPCCEditor : Editor
{
    CharacterFPController _FPCC;

    #region Textures
    Texture2D titleBanner;
    Texture2D basicActionsBanner;
    Texture2D cameraSettingsBanner;
    Texture2D scaleCharacterBanner;
    Texture2D inputsBanner;
    #endregion

    #region Bools
    bool basicActions;
    bool walk;
    bool jump;
    bool sprint;
    bool crouch;
    bool holdCrouch = true;
    bool pressCrouch = true;
    bool zoom;
    bool penalizedWalkingSpeedZoom = true;
    bool stopWalkZoom = true;
    bool inputs;
    bool cameraSettings;
    bool characterScale;
    bool sprintShakeIntensity;
    #endregion

    private void OnEnable()
    {
        _FPCC = (CharacterFPController)target;

        titleBanner = Resources.Load<Texture2D>("FPCharacterController");
        basicActionsBanner = Resources.Load<Texture2D>("BasicActions");
        cameraSettingsBanner = Resources.Load<Texture2D>("CameraSettings");
        scaleCharacterBanner = Resources.Load<Texture2D>("ScaleoftheCharacter");
        inputsBanner = Resources.Load<Texture2D>("ShowInputs");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        EditorGUILayout.Space();
        GUI.DrawTexture(GUILayoutUtility.GetRect(20, 60), titleBanner, ScaleMode.ScaleToFit);

        EditorGUILayout.LabelField("By Ignacio Settembrini", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        _FPCC._life = EditorGUILayout.FloatField("Life", _FPCC._life);
        EditorGUI.ProgressBar(GUILayoutUtility.GetRect(15, 15), _FPCC._life / _FPCC._originalLife, "Life " + _FPCC._life);

        EditorGUILayout.Space();
        BasicActions();
        Separator();
        CameraSettings();
        Separator();
        CharacterScale();
        Separator();
        ShowInputs();

    }

    private void Separator()
    {
        EditorGUILayout.Space();
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 10), Color.black);
        EditorGUILayout.Space();
    }

    #region Basic Actions
    private void BasicActions()
    {    
        GUI.DrawTexture(GUILayoutUtility.GetRect(15, 50), basicActionsBanner, ScaleMode.ScaleToFit);
        
        basicActions = EditorGUILayout.Foldout(basicActions, "");

        if (basicActions)
        {
            WalkGUI();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 5), Color.red);
            JumpGUI();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 5), Color.red);
            SprintGUI();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 5), Color.red);
            CrouchGUI();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 5), Color.red);
            ZoomGUI();
        }
    }

    private void WalkGUI()
    {
        walk = EditorGUILayout.Foldout(walk, "Walk");

        if(walk)
        {
            _FPCC._canWalk = EditorGUILayout.Toggle("Can Walk", _FPCC._canWalk);

            if(_FPCC._canWalk && walk)
            {
                _FPCC._movementSpeed = EditorGUILayout.FloatField("Movement Speed", _FPCC._movementSpeed);
                EditorGUILayout.LabelField("Walk FOV", EditorStyles.boldLabel);
                _FPCC._walkFOV = EditorGUILayout.Slider(_FPCC._walkFOV, 0, 150F);
            }
        }
    }

    private void JumpGUI()
    {
        jump = EditorGUILayout.Foldout(jump, "Jump");

        if(jump)
        {
            _FPCC._canJump = EditorGUILayout.Toggle("Can Jump", _FPCC._canJump);

            if(_FPCC._canJump)
            {
                _FPCC._jumpForce = EditorGUILayout.FloatField("Jump Force", _FPCC._jumpForce);

                _FPCC._multipleJumps = EditorGUILayout.Toggle("Can Do It Multiple Jumps", _FPCC._multipleJumps);

                if(_FPCC._multipleJumps)
                {
                    _FPCC._countToCanJump = EditorGUILayout.IntField("Count To Can Jump", _FPCC._countToCanJump);
                }
            }
        }
    }

    private void SprintGUI()
    {
        sprint = EditorGUILayout.Foldout(sprint, "Sprint");

        if(sprint)
        {
            _FPCC._canSprint = EditorGUILayout.Toggle("Can Sprint", _FPCC._canSprint);

            if(_FPCC._canSprint)
            {
                _FPCC._sprintMultiply = EditorGUILayout.FloatField("Sprint Multiplyer", _FPCC._sprintMultiply);
                EditorGUILayout.LabelField("Sprint FOV", EditorStyles.boldLabel);
                _FPCC._sprintFOV = EditorGUILayout.Slider(_FPCC._sprintFOV, 0, 150f);
                
                sprintShakeIntensity = EditorGUILayout.Foldout(sprintShakeIntensity, "Sprint Shake Intensity");

                if (sprintShakeIntensity)
                {
                    _FPCC._sprintFrequency = EditorGUILayout.Slider("Frequency", _FPCC._sprintFrequency, 0f, 50f);
                    _FPCC._sprintMagnitude = EditorGUILayout.Slider("Magnitude", _FPCC._sprintMagnitude, 0f, 1f);
                }
            }
        }
    }

    private void CrouchGUI()
    {
        crouch = EditorGUILayout.Foldout(crouch, "Crouch");

        if (crouch)
        {
            _FPCC._canCrouch = EditorGUILayout.Toggle("Can Crouch", _FPCC._canCrouch);

            if (_FPCC._canCrouch)
            {

                if (holdCrouch)
                {
                    _FPCC._holdCrounchButton = EditorGUILayout.Toggle("Hold the Crouch Buutton", _FPCC._holdCrounchButton);

                    if (_FPCC._holdCrounchButton)
                    {
                        pressCrouch = false;
                    }
                    else
                    {
                        pressCrouch = true;
                    }
                }

                if (pressCrouch)
                {
                    _FPCC._pressCrounchButton = EditorGUILayout.Toggle("Press the Crouch Buutton", _FPCC._pressCrounchButton);

                    if (_FPCC._pressCrounchButton)
                    {
                        holdCrouch = false;
                    }
                    else
                    {
                        holdCrouch = true;
                    }
                }
            }
        }
    }

    private void ZoomGUI()
    {
        zoom = EditorGUILayout.Foldout(zoom, "Zoom");

        if(zoom)
        {
            _FPCC._canZoom = EditorGUILayout.Toggle("Can Zoom", _FPCC._canZoom);

            if(_FPCC._canZoom)
            {
                EditorGUILayout.LabelField("Zoom FOV", EditorStyles.boldLabel);
                _FPCC._zoomFOV = EditorGUILayout.Slider(_FPCC._zoomFOV, 0, 150f);

                if(penalizedWalkingSpeedZoom)
                {
                    _FPCC._penalizedWalkingSpeedInTheZoom = EditorGUILayout.Toggle("Penalized Waking Speed In The Zoom", _FPCC._penalizedWalkingSpeedInTheZoom);

                    if(_FPCC._penalizedWalkingSpeedInTheZoom)
                    {
                        stopWalkZoom = false;
                        _FPCC._ZoomPenalized = EditorGUILayout.FloatField("Zoom Penalized", _FPCC._ZoomPenalized);
                    }
                    else
                    {
                        stopWalkZoom = true;
                    }
                }
                if(stopWalkZoom)
                {
                    _FPCC._stopWalkingInTheZoom = EditorGUILayout.Toggle("Stop Walking In The Zoom", _FPCC._stopWalkingInTheZoom);

                    if(_FPCC._stopWalkingInTheZoom)
                    {
                        penalizedWalkingSpeedZoom = false;
                    }
                    else
                    {
                        penalizedWalkingSpeedZoom = true;
                    }
                }
            }
        }
    }
    #endregion

    private void CharacterScale()
    {
        GUI.DrawTexture(GUILayoutUtility.GetRect(15, 50), scaleCharacterBanner, ScaleMode.ScaleToFit);
            
        characterScale = EditorGUILayout.Foldout(characterScale, "");

        if (characterScale)
        {
            _FPCC._scale = EditorGUILayout.FloatField("Scale", _FPCC._scale);
        }
    }

    private void CameraSettings()
    {
            
        GUI.DrawTexture(GUILayoutUtility.GetRect(15, 50), cameraSettingsBanner, ScaleMode.ScaleToFit);
        
        cameraSettings = EditorGUILayout.Foldout(cameraSettings, "");

        if (cameraSettings)
        {
            _FPCC._cameraSensitive = EditorGUILayout.FloatField("Camera Sensitive", _FPCC._cameraSensitive);
            EditorGUILayout.LabelField("Negative & Positive Angle", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            _FPCC._cameraNegativeAngle = EditorGUILayout.Slider(_FPCC._cameraNegativeAngle, -90f, 0f);
            _FPCC._cameraPositiveAngle = EditorGUILayout.Slider(_FPCC._cameraPositiveAngle, 0f, 90f);
            EditorGUILayout.EndHorizontal();
        }
    }

    private void ShowInputs()
    {    
        GUI.DrawTexture(GUILayoutUtility.GetRect(15, 50), inputsBanner, ScaleMode.ScaleToFit);
        
        inputs = EditorGUILayout.Foldout(inputs, "");

        if (inputs)
        {
            if (_FPCC._canWalk)
            {
                EditorGUILayout.LabelField("Axis X", EditorStyles.boldLabel);
                _FPCC._axisXInput = EditorGUILayout.TextField(_FPCC._axisXInput);
                EditorGUILayout.LabelField("Axis Z", EditorStyles.boldLabel);
                _FPCC._axisZInput = EditorGUILayout.TextField(_FPCC._axisZInput);
            }

            if (_FPCC._canJump)
            {
                EditorGUILayout.LabelField("Jump", EditorStyles.boldLabel);
                _FPCC._jumpInput = EditorGUILayout.TextField(_FPCC._jumpInput);
            }

            if (_FPCC._canSprint)
            {
                EditorGUILayout.LabelField("Sprint", EditorStyles.boldLabel);
                _FPCC._sprintInput = EditorGUILayout.TextField(_FPCC._sprintInput);
            }

            if (_FPCC._canCrouch)
            {
                EditorGUILayout.LabelField("Crouch", EditorStyles.boldLabel);
                _FPCC._zoomInput = EditorGUILayout.TextField(_FPCC._crouchInput);
            }

            if (_FPCC._canZoom)
            {
                EditorGUILayout.LabelField("Zoom", EditorStyles.boldLabel);
                _FPCC._zoomInput = EditorGUILayout.TextField(_FPCC._zoomInput);
            }
        }
    }
}
