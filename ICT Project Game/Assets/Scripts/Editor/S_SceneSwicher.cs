using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace EditorScripts
{
    public class SceneSwicher : Editor
    {
        [MenuItem("Scene Switcher/Go to Main Menu")]
        static void GoToMenu()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
            }
        }

        [MenuItem("Scene Switcher/Go to Main Game Scene")]
        static void GoToGame()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
            }
        }

        [MenuItem("Scene Switcher/Go to Game Over Scene")]
        static void GoToGameOverScene()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/Scenes/GameOver.unity");
            }
        }

        [MenuItem("Scene Switcher/Go to Test Scene")]
        static void GoToPrefabTestScene()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/Scenes/Test.unity");
            }
        }
    }
}
