using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAppearanceControls : MonoBehaviour
{
    public SkinnedMeshRenderer slimeMesh;
    public SlimeAppearanceModule fireModule;
    public SlimeAppearanceModule waterModule;
    public SlimeAppearanceModule natureModule;

    public void SetAppearance(SlimeData _data)
    {
        if(_data.archetype == SlimeData.Archetype.Fire)
            slimeMesh.material = fireModule.mat;
        else if (_data.archetype == SlimeData.Archetype.Water)
            slimeMesh.material = waterModule.mat;
        else if (_data.archetype == SlimeData.Archetype.Nature)
            slimeMesh.material = natureModule.mat;
    }
}
