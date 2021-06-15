using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraScreenShoot : EditorWindow
{
    RenderTexture   render;

    GUIStyle        titleStyle;

    string          exportPath;

    int             renderWidth;
    int             renderHeight;

    [MenuItem("CustomTool/Gameplay Screenshoot")]
    public static void OpenWindow()
    {
        CameraScreenShoot window = GetWindow<CameraScreenShoot>();

        window.minSize = new Vector2(250, 500);
        window.maxSize = window.minSize;
    }

    private void OnEnable()
    {
        titleStyle = new GUIStyle();

        titleStyle.alignment = TextAnchor.UpperCenter;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 16;
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Camera Screen", titleStyle);
        GUILayout.Space(5);

        EditorGUILayout.LabelField("Import Texture Render");
        render = EditorGUILayout.ObjectField("", render, typeof(RenderTexture), true) as RenderTexture;

        EditorGUILayout.LabelField("Resolution");
        EditorGUILayout.BeginHorizontal();
        renderWidth = EditorGUILayout.IntField(renderWidth);
        GUILayout.Label("X");
        renderHeight = EditorGUILayout.IntField(renderHeight);
        EditorGUILayout.EndHorizontal();

        
        /*
        EditorGUILayout.LabelField("Export Path", titleStyle);
        exportPath = EditorGUI.TextField(new Rect(new Vector2(527, 100), new Vector2(165, 18)), exportPath);
        */
    }
}
