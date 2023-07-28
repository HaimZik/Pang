#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class SceneChangerUtils
{

    [MenuItem("File/Play in editor _F5")]
    public static void PlayInEditor()
    {
        if (EditorApplication.isPlaying == true)
        {
            EditorApplication.isPlaying = false;
            return;
        }
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Level0.unity");
            EditorApplication.isPlaying = true;
        }
    }
    [MenuItem("File/Play fullscreen _F4", false, 0)]
    public static void PlayInFullscreen()
    {
        PlayInEditor();
        EditorWindow gameView = GetMainGameView();
        gameView.maximized = !EditorApplication.isPlaying;
    }

    private static void OpenScene(string sceneName)
    {
        bool isOk = EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        if (isOk)
            EditorSceneManager.OpenScene("Assets/" + sceneName + ".unity");
    }

    [MenuItem("Window/Maximize Game View _F11", false, 0)]
    public static void MaximizeGameView()
    {
        EditorWindow gameView = GetMainGameView();
        if (gameView != null)
        {
            gameView.maximized = !gameView.maximized;
        }
    }

    static EditorWindow GetMainGameView()
    {
        EditorWindow[] windows = (EditorWindow[])Resources.FindObjectsOfTypeAll(typeof(UnityEditor.EditorWindow));
        for (int i = 0; i < windows.Length; i++)
        {
            if (windows[i].GetType().FullName == "UnityEditor.GameView")
            {
                return windows[i];
            }
        }
        return null;
    }

}
#endif