using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Motivation Resources", menuName = "Motivation Resources")]
public class MotivationResources : ScriptableObject
{
    public      string[]        _motivationQuotes;
    public      Texture2D[]     _motivationPhotos;
}
