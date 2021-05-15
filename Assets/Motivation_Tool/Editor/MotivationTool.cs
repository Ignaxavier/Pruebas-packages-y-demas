using UnityEditor;
using UnityEngine;

public class MotivationTool : EditorWindow
{
    private     string[]    _motivationQuotes = {"¡Tú puedes!", "You can do it!", "Ganbatte!", "ты можешь", "Vos podes Rey"};
    private     string[]    _motivationPhotos = { "R1", "R2", "R3", "R4", "R5"};
    private     int         _photosIndex;
    private     int         _quotesIndex;

    private     GUIStyle    myStyle;

    [MenuItem("CustomTool/MotivationTool")]
    public static void OpenWindow()
    {
       MotivationTool w = GetWindow<MotivationTool>();

        w.minSize = new Vector2(300, 350);
        w.maxSize = w.minSize;
    }

    private void OnEnable()
    {
        _quotesIndex = Random.Range(0, _motivationQuotes.Length);
        _photosIndex = Random.Range(0, _motivationPhotos.Length);

        myStyle = new GUIStyle();
        myStyle.fontSize = 27;
        myStyle.alignment = TextAnchor.MiddleCenter;
        myStyle.fontStyle = FontStyle.Bold;
    }

    private void OnGUI()
    {
        BackGround();

        var generateButton = GUILayout.Button("Generate Motivation");

        if (generateButton)
        {
            _quotesIndex = Random.Range(0, _motivationQuotes.Length);
            _photosIndex = Random.Range(0, _motivationPhotos.Length);
        }

        Generate();
    }

    private void BackGround()
    {
        EditorGUI.DrawRect(new Rect(new Vector2(5, 25), new Vector2(290, 325)), Color.grey);
    }

    private void Generate()
    {
        GUILayout.Space(6);
        GUILayout.Label(_motivationQuotes[_quotesIndex], myStyle);
        GUI.DrawTexture(new Rect(new Vector2(0, 65), new Vector2(300, 280)), Resources.Load<Texture2D>(_motivationPhotos[_photosIndex]), ScaleMode.ScaleToFit);
    }
}
