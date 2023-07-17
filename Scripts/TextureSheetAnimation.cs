using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Texture Animation", menuName = "Texture Sheet Animation")]

public class TextureSheetAnimation : ScriptableObject
{
    public float frameInterval;
    public Texture[] frames;
}
