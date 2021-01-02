using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Interaction
{
    public InputAction inputAction;
    [TextArea (3, 10)]
    public string[] textResponse;
    public float[] textPause;
    [TextArea(3, 10)]
    public string[] textResponseAlternate;
    public float[] textPauseAlternate;
    [TextArea(3, 10)]
    public string[] secondQuestTextResponse;
    public float[] secondQuestTextPause;
    [TextArea(3, 10)]
    public string[] secondQuestTextResponseAlternate;
    public float[] secondQuestTextPauseAlternate;
}
