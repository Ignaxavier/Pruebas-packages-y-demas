using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PreviewWindow : EditorWindow
{
    public static void OpenWindow(int widht, int height, RenderTexture render)
    {
        PreviewWindow window = GetWindow<PreviewWindow>();
        window.minSize = new Vector2(widht, height + 10);

        GUI.DrawTexture(new Rect(new Vector2(0, 0), new Vector2(widht, height)), render);

    }

    private void OnGUI()
    {
        if (GUILayout.Button("Close"))
        {
            Close();
        }
    }
}
