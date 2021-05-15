using UnityEditor;
using UnityEngine;

public class MotivationTool : EditorWindow
{
    public      MotivationResources _mr;
    private     int                 _photosIndex;
    private     int                 _quotesIndex;

    private     GUIStyle            myStyle;

    [MenuItem("CustomTool/MotivationTool")]
    public static void OpenWindow()
    {
       MotivationTool w = GetWindow<MotivationTool>();

        w.minSize = new Vector2(300, 350);
        w.maxSize = w.minSize;
    }

    private void OnEnable()
    {
        myStyle = new GUIStyle();
        myStyle.fontSize = 27;
        myStyle.alignment = TextAnchor.MiddleCenter;
        myStyle.fontStyle = FontStyle.Bold;
    }

    private void OnGUI()
    {
        BackGround();

        if(_mr != null)
        {
            var generateButton = GUILayout.Button("Generate Motivation");

            if (generateButton)
            {
                _quotesIndex = Random.Range(0, _mr._motivationQuotes.Length);
                _photosIndex = Random.Range(0, _mr._motivationPhotos.Length);
            }
            
            Generate();
        }
        else if (_mr == null)
        {
            _mr = EditorGUILayout.ObjectField("", _mr, typeof(MotivationResources), true) as MotivationResources;
        }
    }

    private void BackGround()
    {
        EditorGUI.DrawRect(new Rect(new Vector2(5, 25), new Vector2(290, 325)), Color.grey);
    }

    private void Generate()
    {
        GUILayout.Space(6);
        GUILayout.Label(_mr._motivationQuotes[_quotesIndex], myStyle);
        GUI.DrawTexture(new Rect(new Vector2(0, 65), new Vector2(300, 280)), _mr._motivationPhotos[_photosIndex], ScaleMode.ScaleToFit);
    }
}
