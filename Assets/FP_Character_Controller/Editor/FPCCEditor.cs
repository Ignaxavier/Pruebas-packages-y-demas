using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CharacterFPController))]
public class FPCCEditor : Editor
{
    #region Variables
    CharacterFPController _FPCC;

    #region Textures
    Texture2D titleBanner;
    Texture2D basicActionsBanner;
    Texture2D cameraSettingsBanner;
    Texture2D scaleCharacterBanner;
    Texture2D inputsBanner;
    #endregion

    #region Bools
    //Walk
    bool walk;
    bool walkShakeIntensity;

    //Jump
    bool jump;

    //Sprint
    bool sprint;
    bool sprintShakeIntensity;

    //Crouch
    bool crouch;
    bool holdCrouch = true;
    bool pressCrouch = true;

    //Zoom
    bool zoom;
    bool penalizedWalkingSpeedZoom = true;
    bool stopWalkZoom = true;

    //Options
    bool show;
    bool basicActions;
    bool cameraSettings;
    bool characterScale;
    bool inputs;
    #endregion
    #endregion

    private void OnEnable()
    {
        _FPCC = (CharacterFPController)target;

        #region Textures
        titleBanner = Resources.Load<Texture2D>("FPCharacterController");
        basicActionsBanner = Resources.Load<Texture2D>("BasicActions");
        cameraSettingsBanner = Resources.Load<Texture2D>("CameraSettings");
        scaleCharacterBanner = Resources.Load<Texture2D>("ScaleoftheCharacter");
        inputsBanner = Resources.Load<Texture2D>("ShowInputs");
        #endregion
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        EditorGUILayout.Space();
        GUI.DrawTexture(GUILayoutUtility.GetRect(20, 60), titleBanner, ScaleMode.ScaleToFit);

        EditorGUILayout.LabelField("By Ignacio Settembrini        V 0.5", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        BasicActions();
        Separator();
        CameraSettings();
        Separator();
        CharacterScale();
        Separator();
        ShowInputs();

        AnotherButtons();
    }

    private void Separator()
    {
        EditorGUILayout.Space();
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 1), Color.black);
        EditorGUILayout.Space();
    }

    #region Basic Actions
    private void WalkGUI()
    {
        walk = EditorGUILayout.Foldout(walk, "Walk", true, EditorStyles.boldLabel);

        if(walk)
        {
            _FPCC._canWalk = EditorGUILayout.Toggle("Can Walk", _FPCC._canWalk);

            if(_FPCC._canWalk)
            {
                _FPCC._movementSpeed = EditorGUILayout.FloatField("Movement Speed", _FPCC._movementSpeed);
                EditorGUILayout.LabelField("Walk FOV", EditorStyles.boldLabel);
                _FPCC._walkFOV = EditorGUILayout.Slider(_FPCC._walkFOV, 0, 150F);

                walkShakeIntensity = EditorGUILayout.Foldout(walkShakeIntensity, "Walk Shake Intensity");

                if (walkShakeIntensity)
                {
                    _FPCC._walkFrequency = EditorGUILayout.Slider("Frequency", _FPCC._walkFrequency, 0f, 3f);
                    _FPCC._walkMagnitude = EditorGUILayout.Slider("Magnitude", _FPCC._walkMagnitude, 0f, 0.2f);

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Camera Position Y + (Sin(Time * Frequency) * Magnitude)", EditorStyles.boldLabel);
                }
            }
        }
    }

    private void JumpGUI()
    {
        jump = EditorGUILayout.Foldout(jump, "Jump", true, EditorStyles.boldLabel);

        if(jump)
        {
            _FPCC._canJump = EditorGUILayout.Toggle("Can Jump", _FPCC._canJump);

            if(_FPCC._canJump)
            {
                _FPCC._jumpForce = EditorGUILayout.FloatField("Jump Force", _FPCC._jumpForce);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Can Do It Multiple Jumps");
                _FPCC._multipleJumps = EditorGUILayout.Toggle(_FPCC._multipleJumps);
                EditorGUILayout.EndHorizontal();

                if(_FPCC._multipleJumps)
                {
                    _FPCC._countToCanJump = EditorGUILayout.IntField("Count To Can Jump", _FPCC._countToCanJump);
                }
            }
        }
    }

    private void SprintGUI()
    {
        sprint = EditorGUILayout.Foldout(sprint, "Sprint", true, EditorStyles.boldLabel);

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
                    _FPCC._sprintFrequency = EditorGUILayout.Slider("Frequency", _FPCC._sprintFrequency, 0f, 10f);
                    _FPCC._sprintMagnitude = EditorGUILayout.Slider("Magnitude", _FPCC._sprintMagnitude, 0f, 0.3f);

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Camera Position Y + (Sin(Time * Frequency) * Magnitude)", EditorStyles.boldLabel);
                }
            }
        }
    }

    private void CrouchGUI()
    {
        crouch = EditorGUILayout.Foldout(crouch, "Crouch", true, EditorStyles.boldLabel);

        if (crouch)
        {
            _FPCC._canCrouch = EditorGUILayout.Toggle("Can Crouch", _FPCC._canCrouch);

            if (_FPCC._canCrouch)
            {
                EditorGUILayout.Space();
                _FPCC._crouchHeigth = EditorGUILayout.FloatField("Crouch Height", _FPCC._crouchHeigth);
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Penalized Walking Speed in Crouch");
                _FPCC._crouchPenalized = EditorGUILayout.FloatField(_FPCC._crouchPenalized);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Types of Crouch", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                if (holdCrouch)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Hold the Crouch Button");
                    _FPCC._holdCrounchButton = EditorGUILayout.Toggle(_FPCC._holdCrounchButton);
                    EditorGUILayout.EndHorizontal();

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
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Press the Crouch Button");
                    _FPCC._pressCrounchButton = EditorGUILayout.Toggle(_FPCC._pressCrounchButton);
                    EditorGUILayout.EndHorizontal();

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
        zoom = EditorGUILayout.Foldout(zoom, "Zoom", true, EditorStyles.boldLabel);

        if(zoom)
        {
            _FPCC._canZoom = EditorGUILayout.Toggle("Can Zoom", _FPCC._canZoom);

            if(_FPCC._canZoom)
            {
                EditorGUILayout.LabelField("Zoom FOV", EditorStyles.boldLabel);
                _FPCC._zoomFOV = EditorGUILayout.Slider(_FPCC._zoomFOV, 0, 150f);

                if(penalizedWalkingSpeedZoom)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Penalized Walking Speed In The Zoom");
                    _FPCC._penalizedWalkingSpeedInTheZoom = EditorGUILayout.Toggle(_FPCC._penalizedWalkingSpeedInTheZoom);
                    EditorGUILayout.EndHorizontal();

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
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Stop Walking In The Zoom");
                    _FPCC._stopWalkingInTheZoom = EditorGUILayout.Toggle(_FPCC._stopWalkingInTheZoom);
                    EditorGUILayout.EndHorizontal();

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

    #region Options
    private void BasicActions()
    {    
        GUI.DrawTexture(GUILayoutUtility.GetRect(15, 50), basicActionsBanner, ScaleMode.ScaleToFit);

        #region Button Stuff

        var basicActionsButton = GUILayout.Button("Show");

        if(basicActionsButton && !basicActions)
        {
            basicActions = true;
        }
        else if (basicActionsButton && basicActions)
        {
            basicActions = false;
        }

        #endregion

        if (basicActions)
        {
            WalkGUI();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 1), Color.grey);
            JumpGUI();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 1), Color.grey);
            SprintGUI();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 1), Color.grey);
            CrouchGUI();
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 1), Color.grey);
            ZoomGUI();
        }
    }

    private void CharacterScale()
    {
        GUI.DrawTexture(GUILayoutUtility.GetRect(15, 50), scaleCharacterBanner, ScaleMode.ScaleToFit);
        //show = EditorGUILayout.Foldout(show, "Show", true);

        #region Button Stuff

        var characterScaleButton = GUILayout.Button("Show");

        if (characterScaleButton && !characterScale)
        {
            characterScale = true;
        }
        else if (characterScaleButton && characterScale)
        {
            characterScale = false;
        }

        #endregion

        if (characterScale)
        {
            EditorGUILayout.LabelField("This parameters only affect at Awake Frame", EditorStyles.helpBox);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Capsule Settings", EditorStyles.boldLabel);
            _FPCC._colliderHeight = EditorGUILayout.FloatField("Height", _FPCC._colliderHeight);
            _FPCC._colliderRadius = EditorGUILayout.FloatField("Radius", _FPCC._colliderRadius);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Transform Scale", EditorStyles.boldLabel);
            _FPCC._scale = EditorGUILayout.FloatField("Scale", _FPCC._scale);
        }
    }

    private void CameraSettings()
    {       
        GUI.DrawTexture(GUILayoutUtility.GetRect(15, 50), cameraSettingsBanner, ScaleMode.ScaleToFit);

        #region Button Stuff

        var cameraSettingsButton = GUILayout.Button("Show");

        if (cameraSettingsButton && !cameraSettings)
        {
            cameraSettings = true;
        }
        else if (cameraSettingsButton && cameraSettings)
        {
            cameraSettings = false;
        }

        #endregion

        if (cameraSettings)
        {
            EditorGUILayout.LabelField("This parameters only affect at Awake Frame", EditorStyles.helpBox);

            EditorGUILayout.Space();

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

        #region Button Stuff

        var inputsButton = GUILayout.Button("Show");

        if (inputsButton && !inputs)
        {
            inputs = true;
        }
        else if (inputsButton && inputs)
        {
            inputs = false;
        }

        #endregion

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
                _FPCC._crouchInput = EditorGUILayout.TextField(_FPCC._crouchInput);
            }

            if (_FPCC._canZoom)
            {
                EditorGUILayout.LabelField("Zoom", EditorStyles.boldLabel);
                _FPCC._zoomInput = EditorGUILayout.TextField(_FPCC._zoomInput);
            }
        }
    }
    #endregion

    private void AnotherButtons()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("ME", EditorStyles.boldLabel);

        #region Twitter
        var twitter = GUILayout.Button("Follow me on Twitter");

        if (twitter)
        {
            Application.OpenURL("https://twitter.com/ISettembrini");
        }
        #endregion

        EditorGUILayout.Space();

        #region Instagram
        var instagram = GUILayout.Button("Follow me on Instagram");

        if (instagram)
        {
            Application.OpenURL("https://www.instagram.com/ignaxavier/");
        }
        #endregion

        EditorGUILayout.Space();

        #region Paypal
        var paypal = GUILayout.Button("Paypal Donations");

        if (paypal)
        {
            Application.OpenURL("https://www.paypal.com/donate?hosted_button_id=N5H7GF58NM9ZC");
        }
        #endregion
    }
}
