using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneSwicher : Editor
{
    [MenuItem("Custom Tools/Go to Main Menu")]
    static void GoToMenu()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
        }
    }

    [MenuItem("Custom Tools/Go to Main Game Scene")]
    static void GoToGame()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
        }
    }

    [MenuItem("Custom Tools/Go to Game Over Scene")]
    static void GoToGameOverScene()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/GameOver.unity");
        }
    }

    [MenuItem("Custom Tools/Go to Test Scene")]
    static void GoToPrefabTestScene()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Test.unity");
        }
    }
}
