using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuSelectionData
{
    private static int i_nextScene;

    public static int GetNextScene()
    {
        return i_nextScene;
    }

    public static void SetNextScene(int nextSceneIndex)
    {
        i_nextScene = nextSceneIndex;
    }
}
