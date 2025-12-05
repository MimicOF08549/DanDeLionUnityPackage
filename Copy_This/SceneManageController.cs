using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "SceneManageController", menuName = "SceneController/SceneManageController")]
public class SceneManageController : ScriptableObject
{

    [SerializeField] private SceneReferenceController UIscene;
    [SerializeField] private SceneControllerOutGame Transition_Prefab;
    [SerializeField] List<SceneCollection> sceneCollection;

    private static SceneReferenceController UISCENE;


    private void OnValidate()
    {
        if (UIscene) UISCENE = this.UIscene;
    }


    [HideInCallstack]
    public static AsyncOperation[] UnLoadSceneForScript()
    {
        List<AsyncOperation> asyncOperation = new();
        int sceneCount = SceneManager.sceneCount;

        List<Scene> scenes = new();

        for (int i = 0; i < sceneCount; i++)
        {
            Scene rc = SceneManager.GetSceneAt(i);

            if (rc.name != UISCENE.sceneRefName)
            {
                scenes.Add(rc);
            }
        }

        foreach (Scene x in scenes)
        {
            asyncOperation.Add(SceneManager.UnloadSceneAsync(x.name));
        }

        if (asyncOperation.Count > 0)
            return asyncOperation.ToArray();

        else return null;
    }

    [HideInCallstack]
    public static AsyncOperation LoadSceneForScript(SceneReferenceController referenceController)
    {

        var openScene = SceneManager.LoadSceneAsync(referenceController.sceneRefName, LoadSceneMode.Additive);

        return openScene;
    }


    [HideInCallstack]
    public static void SetActiveScene(SceneReferenceController sceneReferenceController)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneReferenceController.sceneRefName));
    }


    public void LoadScene(SceneReferenceController referenceController)
    {
        var sceneloader = GameObject.FindAnyObjectByType<SceneControllerInGame>();

        UISCENE = this.UIscene;

        if (UISCENE && !SceneManager.GetSceneByName(UISCENE.sceneRefName).isLoaded)
        {
            SceneManager.LoadScene(UISCENE.sceneRefName, LoadSceneMode.Additive);
        }

        if (!sceneloader)
        {
            Debug.LogError($"There is no SceneControllerInGame, Cancle Loading Progess");
            return;
        }

        int sceneBuild = SceneManager.sceneCountInBuildSettings;

        List<string> scenes = new();
        for (int i = 0; i < sceneBuild; i++)
        {
            scenes.Add(System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i)));
        }


        for (int i = 0; i < sceneBuild; i++)
        {
            Debug.Log(scenes[i]);
            if (scenes[i] != referenceController.sceneRefName)
            {
                if (i == sceneBuild - 1)
                {
                    Debug.LogError($"There is no {referenceController.sceneRefName} in build, Cancle Loading Progess");
                    return;
                }
            }
            else
            {
                break;
            }
        }

        Debug.Log("Condition Check Complete. Begin Loading");

        sceneloader.StartLoadSceneNow(referenceController, Transition_Prefab);
    }

    public void LoadSceneWithForce(SceneReferenceController referenceController)
    {
        int sceneBuild = SceneManager.sceneCountInBuildSettings;

        UISCENE = this.UIscene;

        if (UISCENE && !SceneManager.GetSceneByName(UISCENE.sceneRefName).isLoaded)
        {
            SceneManager.LoadScene(UISCENE.sceneRefName, LoadSceneMode.Additive);
        }

        List<string> scenes = new();
        for (int i = 0; i < sceneBuild; i++)
        {
            scenes.Add(System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i)));
        }


        for (int i = 0; i < sceneBuild; i++)
        {
            Debug.Log(scenes[i]);
            if (scenes[i] != referenceController.sceneRefName)
            {
                if (i == sceneBuild - 1)
                {
                    Debug.LogError($"There is no {referenceController.sceneRefName} in build, Cancle Loading Progess");
                    return;
                }
            }
            else
            {
                break;
            }
        }

        Debug.Log("Condition Check Complete. Begin Loading");

        SceneControllerInGame.StartLoadSceneByForce(referenceController, Transition_Prefab);
    }

    public void LoadRandomSceneFromCollection(int collectionIndex)
    {
        SceneCollection collectionDestinate = sceneCollection[collectionIndex];
        int collectionCount = collectionDestinate.collection.Count;
        int randomRef = UnityEngine.Random.Range(0, collectionCount);
        SceneReferenceController refDes = collectionDestinate.collection[randomRef];
        LoadScene(refDes);
    }


    [Serializable]
    public class SceneCollection
    {

        public List<SceneReferenceController> collection = new();

        public SceneCollection(params SceneReferenceController[] references)
        {
            collection = references.ToList();
        }
    }
}
