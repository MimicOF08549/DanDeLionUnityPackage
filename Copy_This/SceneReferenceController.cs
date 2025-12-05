using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;



#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "SceneReferenceController", menuName = "SceneController/SceneReferenceController")]
public class SceneReferenceController : ScriptableObject
{
    [HideInInspector] public string sceneRefName = "";

    public void GoToScene()
    {
        SceneManager.LoadScene(sceneRefName);
    }

    public void AddScene()
    {
        SceneManager.LoadScene(sceneRefName, LoadSceneMode.Additive);
    }

    public void SetFPS(int target)
    {
        //Debug.Log(Application.targetFrameRate);
        //Debug.Log(QualitySettings.vSyncCount);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;
    }


#if UNITY_EDITOR
    [SerializeField] private SceneAsset sceneAsset;

    private void OnValidate()
    {
        if (sceneAsset)
            sceneRefName = sceneAsset.name;
    }

    public static class SceneRefContextMenu
    {
        [MenuItem("Assets/Create/SceneController/SceneReferenceControllerAssigned")]
        private static void CreateObjectFromAudioResource(MenuCommand menuCommand)
        {
            SceneAsset resource = Selection.activeObject as SceneAsset;

            if (resource == null)
            {
                Debug.LogWarning("No Scene Selected.");
                return;
            }

            SceneReferenceController createRef = ScriptableObject.CreateInstance<SceneReferenceController>();
            createRef.sceneRefName = resource.name;
            createRef.sceneAsset = resource;

            string path = AssetDatabase.GetAssetPath(resource);

            path = path.Replace(".unity", ".asset");
            path = AssetDatabase.GenerateUniqueAssetPath(path);

            AssetDatabase.CreateAsset(createRef, path);
            AssetDatabase.RenameAsset(path, $"{createRef.sceneRefName} Reference");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = createRef;

        }
    }
#endif

}
