using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BasicLifeSystem))]
public class BasicLifeSystemEditor : Editor
{
    BasicLifeSystem     _BLS;
    GUIStyle            headerStyle;

    private void OnEnable()
    {
        _BLS = (BasicLifeSystem)target;

        headerStyle = new GUIStyle();

        headerStyle.fontSize = 18;
        headerStyle.alignment = TextAnchor.MiddleCenter;
        headerStyle.fontStyle = FontStyle.Bold;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        #region Header
        GUILayout.Space(5);
        EditorGUILayout.LabelField("Basic Life System", headerStyle);
        GUILayout.Space(2);
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 2), Color.black);
        GUILayout.Space(2);
        EditorGUILayout.LabelField("By Ignacio Settembrini", EditorStyles.boldLabel);
        GUILayout.Space(10);
        #endregion

        if (Application.isEditor)
        {
            _BLS._Life = EditorGUILayout.FloatField("Life", _BLS._Life);
            Repaint();
        }
        else if (Application.isPlaying)
        {
            EditorGUI.ProgressBar(new Rect(50, 50, 500, 500), _BLS._Life / _BLS.originalLife, "Life");
            Repaint();
        }
    }
}
