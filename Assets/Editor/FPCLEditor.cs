using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FPCameraLook))]
public class FPCLEditor : Editor
{
    FPCameraLook _FPCL;

    private void OnEnable()
    {
        _FPCL = (FPCameraLook)target;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        _FPCL._characterBody = EditorGUILayout.ObjectField("Character Body:", _FPCL._characterBody, typeof(Transform), true) as Transform;
    }
}
