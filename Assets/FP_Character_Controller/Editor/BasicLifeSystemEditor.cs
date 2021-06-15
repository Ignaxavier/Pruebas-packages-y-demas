using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BasicLifeSystem))]
public class BasicLifeSystemEditor : Editor
{
    BasicLifeSystem     _BLS;
    GUIStyle            headerStyle;
    Texture2D           headerTexture;

    private void OnEnable()
    {
        _BLS = (BasicLifeSystem)target;

        headerTexture = Resources.Load<Texture2D>("BasicLifeSystem");

        headerStyle = new GUIStyle();

        headerStyle.fontSize = 18;
        headerStyle.alignment = TextAnchor.MiddleCenter;
        headerStyle.fontStyle = FontStyle.Bold;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        #region Background
        EditorGUI.DrawRect(new Rect(new Vector2(6, 3), new Vector2(900, 106)), Color.white);
        #endregion

        #region Header
        GUILayout.Space(5);
        GUI.DrawTexture(GUILayoutUtility.GetRect(15, 50), headerTexture, ScaleMode.ScaleToFit);
        GUILayout.Space(2);
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 2), Color.black);
        GUILayout.Space(2);
        EditorGUILayout.LabelField("By Ignacio Settembrini", EditorStyles.boldLabel);
        GUILayout.Space(8);
        #endregion

        #region Stuff
        if (!EditorApplication.isPlaying)
        {
            _BLS._Life = EditorGUILayout.FloatField("Life", _BLS._Life);
            Repaint();
        }
        else if (EditorApplication.isPlaying)
        {
            EditorGUI.ProgressBar(GUILayoutUtility.GetRect(1, 18), _BLS._Life / _BLS.originalLife, "Life");
            Repaint();
        }
        #endregion
    }
}
