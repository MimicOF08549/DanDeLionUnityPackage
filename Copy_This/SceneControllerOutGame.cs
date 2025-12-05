using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SceneControllerOutGame : MonoBehaviour
{

    //[SerializeField] SceneManageController sceneController;

    [SerializeField] MaterialAnimationController animationStart_Prefab;
    [SerializeField] MaterialAnimationController animationEnd_Prefab;

    public UnityEvent OnGoingUnLoadScene = new();
    public UnityEvent OnCompleteUnLoadScene = new();

    public UnityEvent<string> OnGoingLoadingSceneText = new();
    public UnityEvent<int> OnGoingLoadingSceneProgress = new();

    public bool PauseEveryThingsWhileLoad = true;
    [SerializeField][Range(0f, 5f)] private float FakeBuffer = 1f;

    private SceneReferenceController DesScene;
    private bool p_endAnimation = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        //DontDestroyOnLoad(animationStart_Prefab.gameObject);
        //DontDestroyOnLoad(animationEnd_Prefab.gameObject);
        animationStart_Prefab.eventCallbackNode.BeginPlaying.AddListener(OnStartChangeToOtherScene);
        animationStart_Prefab.eventCallbackNode.DiedEvent.AddListener(OnEndChangeToOtherScene);

        animationStart_Prefab.destroyerNode.IsActiveThisNode = true;
        animationStart_Prefab.destroyerNode.endingMethod = DestroyerNode.EndingMethodEnum.CallEventOnly;
        animationStart_Prefab.eventCallbackNode.IsActiveThisNode = true;

        animationEnd_Prefab.destroyerNode.IsActiveThisNode = true;
        animationEnd_Prefab.destroyerNode.endingMethod = DestroyerNode.EndingMethodEnum.CallEventOnly;
        animationEnd_Prefab.eventCallbackNode.IsActiveThisNode = true;

        p_endAnimation = false;
    }

    public void StartLoadingAnimation(SceneReferenceController referenceController)
    {
        DesScene = referenceController;
        StartCoroutine(UnLoadScene());
    }

    void OnStartChangeToOtherScene(Material material)
    {
        OnGoingUnLoadScene?.Invoke();
    }

    void OnEndChangeToOtherScene(Material material)
    {
        OnCompleteUnLoadScene?.Invoke();
        p_endAnimation = true;
    }

    void OnStartOtherScene(Material material)
    {
        var loadObj = GameObject.FindAnyObjectByType<SceneControllerInGame>();

        if (loadObj)
        {
            loadObj.OnGoingLoadScene?.Invoke();
        }
    }

    void OnEndOtherScene(Material material)
    {
        var loadObj = GameObject.FindAnyObjectByType<SceneControllerInGame>();

        if (loadObj)
        {
            loadObj.OnEndLoadScene?.Invoke();
        }
        p_endAnimation = true;
    }

    IEnumerator UnLoadScene()
    {

        OnGoingLoadingSceneText?.Invoke(DesScene.sceneRefName);

        animationStart_Prefab.Play();

        if (PauseEveryThingsWhileLoad) Time.timeScale = 0.0f;

        while (!p_endAnimation)
        {
            yield return new WaitForEndOfFrame();
        }
        p_endAnimation = false;


        AsyncOperation[] unloadOp = SceneManageController.UnLoadSceneForScript();

        if (unloadOp != null)
            foreach (AsyncOperation op in unloadOp)
            {
                while (!op.isDone)
                {
                    OnGoingLoadingSceneProgress?.Invoke(Mathf.Clamp(0, 100, Mathf.FloorToInt(op.progress * 100f / 90f)));
                    yield return new WaitForEndOfFrame();
                }
            }

        yield return new WaitForEndOfFrame();
        yield return new WaitForSecondsRealtime(FakeBuffer);


        StartCoroutine(LoadScene());

    }

    IEnumerator LoadScene()
    {

        animationEnd_Prefab.eventCallbackNode.BeginPlaying.AddListener(OnStartOtherScene);
        animationEnd_Prefab.eventCallbackNode.DiedEvent.AddListener(OnEndOtherScene);

        AsyncOperation loadSceneOp = SceneManageController.LoadSceneForScript(DesScene);


        if (loadSceneOp != null)
            while (!loadSceneOp.isDone)
            {
                yield return new WaitForEndOfFrame();
            }

        yield return new WaitForEndOfFrame();

        SceneManageController.SetActiveScene(DesScene);

        animationEnd_Prefab.Play();

        while (!p_endAnimation)
        {
            yield return new WaitForEndOfFrame();
        }

        p_endAnimation = false;

        if (PauseEveryThingsWhileLoad) Time.timeScale = 1.0f;

        Destroy(this.gameObject);
    }

    private void OnApplicationQuit()
    {
        Time.timeScale = 1.0f;
    }
}
