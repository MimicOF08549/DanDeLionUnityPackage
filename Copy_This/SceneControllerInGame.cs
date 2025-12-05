using UnityEngine;
using UnityEngine.Events;

public class SceneControllerInGame : MonoBehaviour
{
    public UnityEvent OnGoingLoadScene = new();
    public UnityEvent OnEndLoadScene = new();

    public UnityEvent OnGoingUnLoadScene = new();
    public UnityEvent OnEndUnLoadScene = new();


    [HideInCallstack]
    public void StartLoadSceneNow(SceneReferenceController sceneRef, SceneControllerOutGame spawnLoader)
    {
        var loadAnimation = Instantiate(spawnLoader);
        loadAnimation.OnGoingUnLoadScene = OnGoingUnLoadScene;
        loadAnimation.OnCompleteUnLoadScene = OnEndUnLoadScene;
        loadAnimation.StartLoadingAnimation(sceneRef);
    }

    [HideInCallstack]
    public static void StartLoadSceneByForce(SceneReferenceController sceneRef, SceneControllerOutGame spawnLoader)
    {
        var loadAnimation = Instantiate(spawnLoader);
        loadAnimation.StartLoadingAnimation(sceneRef);
    }
}
