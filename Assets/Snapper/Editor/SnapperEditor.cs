using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Snapper))]
public class SnapperEditor : Editor
{
    Snapper _snapper;

    Texture2D titleBanner;

    private void OnEnable()
    {
        _snapper = (Snapper)target;

        titleBanner = Resources.Load<Texture2D>("Snapper");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        BackGround();
        Header();

        _snapper._onoff = EditorGUILayout.Toggle("On / Off", _snapper._onoff);
        EditorGUILayout.Space();

        if (_snapper._onoff)
        {
            RayComponets();
        }
    }

    private void RayComponets()
    {
        _snapper._DirectionOfTheRay = (Snapper.dirRay)EditorGUILayout.EnumPopup("Direction of the Ray", _snapper._DirectionOfTheRay);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Ray Distance", EditorStyles.boldLabel);
        _snapper._rayDistance = EditorGUILayout.Slider(_snapper._rayDistance, -5f, 5f);
        EditorGUILayout.Space();
        _snapper._positionOffset = EditorGUILayout.FloatField("Position Offset", _snapper._positionOffset);
        EditorGUILayout.Space();
        _snapper._layerMask = EditorGUILayout.LayerField("Layer Mask", _snapper._layerMask);

    }

    private void Header()
    {
        EditorGUILayout.Space();
        GUI.DrawTexture(GUILayoutUtility.GetRect(20, 60), titleBanner, ScaleMode.ScaleToFit);
        EditorGUILayout.LabelField("By Ignacio Settembrini", EditorStyles.boldLabel);
        EditorGUILayout.Space();
    }

    private void BackGround()
    {
        if (_snapper._onoff)
        {
            EditorGUI.DrawRect(new Rect(4, 3, 5000, 232), Color.grey);
        }
        else
        {
            EditorGUI.DrawRect(new Rect(4, 3, 5000, 114), Color.grey);
        }
    }
}
