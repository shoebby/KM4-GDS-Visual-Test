using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelecter : MonoBehaviour
{
    public SceneFader fader;

    public void Select (int buildINdex)
    {
        fader.FadeTo(buildINdex);
    }
}
