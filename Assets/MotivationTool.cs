using UnityEditor;
using UnityEngine;

public class MotivationTool : EditorWindow
{
    private     string[]    _motivationQuotes = {"¡Tú puedes!", "You can do it!", "Ganbatte!"};
    private     string[]    _motivationPhotos = { "R1", "R2"};
    private     int         _photosIndex;
    private     int         _quotesIndex;
    private     bool        _isGenerate;
    private     bool        _alreadyGenerate;

    [MenuItem("CustomTool/MotivationTool")]
    public static void OpenWindow()
    {
       MotivationTool moTool = GetWindow<MotivationTool>();
    }

    private void OnGUI()
    {
        if (!_isGenerate)
        {
            var desmotiveButton = GUILayout.Button("Generate", GUILayout.Width(100), GUILayout.Height(100));

            if (desmotiveButton)
            {
                _isGenerate = true;
            }
        }
        else if (_isGenerate)
        {
            if (!_alreadyGenerate)
            {
                _quotesIndex = Random.Range(0, _motivationQuotes.Length);
                _photosIndex = Random.Range(0, _motivationPhotos.Length);
                _alreadyGenerate = true;
            }
            else
            {
                EditorGUILayout.LabelField(_motivationQuotes[_quotesIndex], EditorStyles.boldLabel);
                GUI.DrawTexture(GUILayoutUtility.GetRect(300, 300), Resources.Load<Texture2D>(_motivationPhotos[_photosIndex]));
            }
        }
    }
}
