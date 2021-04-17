using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterFPSController))]
public class FPSCCEditor : Editor
{
    CharacterFPSController _FPSCC;
    bool basicActions;
    bool walk;
    bool jump;
    bool sprint;
    bool zoom;
    bool penalizedWalkingSpeedZoom = true;
    bool stopWalkZoom = true;
    bool inputs;
    bool cameraSettings;
    bool characterScale;
    bool sprintShakeIntensity;

    private void OnEnable()
    {
        _FPSCC = (CharacterFPSController)target;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        EditorGUILayout.LabelField("By Ignacio Settembrini", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        _FPSCC._life = EditorGUILayout.FloatField("Life", _FPSCC._life);
        EditorGUI.ProgressBar(GUILayoutUtility.GetRect(15, 15), _FPSCC._life / _FPSCC._originalLife, "Life " + _FPSCC._life);

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
        basicActions = EditorGUILayout.Foldout(basicActions, "Basic Actions");

        if (basicActions)
        {
            WalkGUI();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 5), Color.red);
            JumpGUI();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 5), Color.red);
            SprintGUI();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 5), Color.red);
            ZoomGUI();
        }
    }

    private void WalkGUI()
    {
        walk = EditorGUILayout.Foldout(walk, "Walk");

        if(walk)
        {
            _FPSCC._canWalk = EditorGUILayout.Toggle("Can Walk", _FPSCC._canWalk);

            if(_FPSCC._canWalk && walk)
            {
                _FPSCC._movementSpeed = EditorGUILayout.FloatField("Movement Speed", _FPSCC._movementSpeed);
                EditorGUILayout.LabelField("Walk FOV", EditorStyles.boldLabel);
                _FPSCC._walkFOV = EditorGUILayout.Slider(_FPSCC._walkFOV, 0, 150F);
            }
        }
    }

    private void JumpGUI()
    {
        jump = EditorGUILayout.Foldout(jump, "Jump");

        if(jump)
        {
            _FPSCC._canJump = EditorGUILayout.Toggle("Can Jump", _FPSCC._canJump);

            if(_FPSCC._canJump)
            {
                _FPSCC._jumpForce = EditorGUILayout.FloatField("Jump Force", _FPSCC._jumpForce);

                _FPSCC._multipleJumps = EditorGUILayout.Toggle("Can Do It Multiple Jumps", _FPSCC._multipleJumps);

                if(_FPSCC._multipleJumps)
                {
                    _FPSCC._countToCanJump = EditorGUILayout.IntField("Count To Can Jump", _FPSCC._countToCanJump);
                }
            }
        }
    }

    private void SprintGUI()
    {
        sprint = EditorGUILayout.Foldout(sprint, "Sprint");

        if(sprint)
        {
            _FPSCC._canSprint = EditorGUILayout.Toggle("Can Sprint", _FPSCC._canSprint);

            if(_FPSCC._canSprint)
            {
                _FPSCC._sprintMultiply = EditorGUILayout.FloatField("Sprint Multiplyer", _FPSCC._sprintMultiply);
                EditorGUILayout.LabelField("Sprint FOV", EditorStyles.boldLabel);
                _FPSCC._sprintFOV = EditorGUILayout.Slider(_FPSCC._sprintFOV, 0, 150f);
                
                sprintShakeIntensity = EditorGUILayout.Foldout(sprintShakeIntensity, "Sprint Shake Intensity");

                if (sprintShakeIntensity)
                {
                    _FPSCC._sprintFrequency = EditorGUILayout.Slider("Frequency", _FPSCC._sprintFrequency, 0f, 50f);
                    _FPSCC._sprintMagnitude = EditorGUILayout.Slider("Magnitude", _FPSCC._sprintMagnitude, 0f, 1f);
                }
            }
        }
    }

    private void ZoomGUI()
    {
        zoom = EditorGUILayout.Foldout(zoom, "Zoom");

        if(zoom)
        {
            _FPSCC._canZoom = EditorGUILayout.Toggle("Can Zoom", _FPSCC._canZoom);

            if(_FPSCC._canZoom)
            {
                EditorGUILayout.LabelField("Zoom FOV", EditorStyles.boldLabel);
                _FPSCC._zoomFOV = EditorGUILayout.Slider(_FPSCC._zoomFOV, 0, 150f);

                if(penalizedWalkingSpeedZoom)
                {
                    _FPSCC._penalizedWalkingSpeedInTheZoom = EditorGUILayout.Toggle("Penalized Waking Speed In The Zoom", _FPSCC._penalizedWalkingSpeedInTheZoom);

                    if(_FPSCC._penalizedWalkingSpeedInTheZoom)
                    {
                        stopWalkZoom = false;
                        _FPSCC._ZoomPenalized = EditorGUILayout.FloatField("Zoom Penalized", _FPSCC._ZoomPenalized);
                    }
                    else
                    {
                        stopWalkZoom = true;
                    }
                }
                if(stopWalkZoom)
                {
                    _FPSCC._stopWalkingInTheZoom = EditorGUILayout.Toggle("Stop Walking In The Zoom", _FPSCC._stopWalkingInTheZoom);

                    if(_FPSCC._stopWalkingInTheZoom)
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
        characterScale = EditorGUILayout.Foldout(characterScale, "Scale of the Character");

        if (characterScale)
        {
            _FPSCC._height = EditorGUILayout.FloatField("Scale", _FPSCC._height);
        }
    }

    private void CameraSettings()
    {
        cameraSettings = EditorGUILayout.Foldout(cameraSettings, "Camera Settings");

        if (cameraSettings)
        {
            _FPSCC._cameraSensitive = EditorGUILayout.FloatField("Camera Sensitive", _FPSCC._cameraSensitive);
            EditorGUILayout.LabelField("Negative & Positive Angle", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            _FPSCC._cameraNegativeAngle = EditorGUILayout.Slider(_FPSCC._cameraNegativeAngle, -90f, 0f);
            _FPSCC._cameraPositiveAngle = EditorGUILayout.Slider(_FPSCC._cameraPositiveAngle, 0f, 90f);
            EditorGUILayout.EndHorizontal();
        }
    }

    private void ShowInputs()
    {
        inputs = EditorGUILayout.Foldout(inputs, "Show Inputs");

        if (inputs)
        {
            EditorGUILayout.LabelField("Axis X", EditorStyles.boldLabel);
            _FPSCC._axisXInput = EditorGUILayout.TextField(_FPSCC._axisXInput);
            EditorGUILayout.LabelField("Axis Z", EditorStyles.boldLabel);
            _FPSCC._axisZInput = EditorGUILayout.TextField(_FPSCC._axisZInput);
            EditorGUILayout.LabelField("Jump", EditorStyles.boldLabel);
            _FPSCC._jumpInput = EditorGUILayout.TextField(_FPSCC._jumpInput);
            EditorGUILayout.LabelField("Sprint", EditorStyles.boldLabel);
            _FPSCC._sprintInput = EditorGUILayout.TextField(_FPSCC._sprintInput);
            EditorGUILayout.LabelField("Zoom", EditorStyles.boldLabel);
            _FPSCC._zoomInput = EditorGUILayout.TextField(_FPSCC._zoomInput);
        }
    }
}
