using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EventUnityInvoker : MonoBehaviour
{
    public UnityEvent eventEnable = new();

    public UnityEvent eventAwake = new();
    public UnityEvent eventStart = new();
    public UnityEvent eventLoaded = new();
    public UnityEvent eventLoadedFixed = new();
    public UnityEvent eventDisable = new();

    public float waitTime = 3f;
    public UnityEvent eventWaitTime = new();

    // Start is called before the first frame update
    void Start()
    {
        eventStart?.Invoke();

        StartCoroutine(OnLoadedScene());
        StartCoroutine(OnLoadedFixedScene());
        StartCoroutine(OnWaitTimeScene());

    }

    void OnEnable()
    {
        eventEnable?.Invoke();
    }

    void OnDisable()
    {
        eventDisable?.Invoke();
    }

    void Awake()
    {
        eventAwake?.Invoke();
    }

    IEnumerator OnLoadedScene()
    {
        yield return new WaitForEndOfFrame();
        eventLoaded?.Invoke();
    }

    IEnumerator OnLoadedFixedScene()
    {
        yield return new WaitForFixedUpdate();
        eventLoadedFixed?.Invoke();
    }

    IEnumerator OnWaitTimeScene()
    {
        yield return new WaitForSeconds(waitTime);
        eventWaitTime?.Invoke();
    }

    private void Update()
    {

    }
}

