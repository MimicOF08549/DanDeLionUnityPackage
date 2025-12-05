using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



#if UNITY_EDITOR
using UnityEditor;
#endif

public class MaterialAnimationController : MonoBehaviour
{
    [SerializeField]
    private bool EnableToPlay = true;

    public Material AnimateMaterial;
    [Space(20f)]
    [Header("MATERIAL PART")]
    [Space(10f)]
    [Space(10f)]
    public MainStat initializeNode = new();

    [Space(10f)]
    public SetFloatNode setFloatNode = new();

    [Space(10f)]
    public SetColorNode setColorNode = new();

    [Space(10f)]
    public SetTextureNode setTextureNode = new();

    [Space(10f)]
    public DestroyerNode destroyerNode = new();

    [Space(20f)]
    [Header("TRANSFORM PART")]
    [Space(10f)]

    [Space(10f)]
    public GameObject AnimateTransform;


    [Space(10f)]
    public ThreeAxisAnimation PositionNode = new();

    [Space(10f)]
    public ThreeAxisAnimation RotationNode = new();

    [Space(10f)]
    public ThreeAxisAnimation ScaleNode = new();

    [Space(20f)]
    [Header("EVENT PART")]
    [Space(10f)]
    [Space(10f)]
    public EventCallbackNode eventCallbackNode;


    private float p_Age = 0;
    private bool p_IsPlay = false;
    private RectTransform rectAnimate;
    private Vector3 p_randomPos = Vector3.one;
    private Vector3 p_randomRot = Vector3.one;
    private Vector3 p_randomSca = Vector3.one;

    private QuitSetDefualt materialDefault = new();

    [HideInInspector]
    public static List<QuitSetDefualt> SetAllToDefault = new();

    [HideInInspector]
    public static bool MaterialChanging = false;

    void OnValidate()
    {
        setTextureNode.SetTextures.Sort((x1, x2) => x1.TimeStamp.CompareTo(x2.TimeStamp));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        AssignEndProgress();

        if (initializeNode.PlayOnAwake) Play();
    }

    private void OnEnable()
    {
        if (initializeNode.PlayOnEnable) { Play(); }
    }

    // Update is called once per frame
    void Update()
    {
        if (initializeNode.updateMethod == MainStat.UpdateMethod.EveryTick && p_IsPlay)
            UpdateAnimation(initializeNode.UnscaleTime ? Time.unscaledDeltaTime : Time.deltaTime);

    }

    void FixedUpdate()
    {
        if (initializeNode.updateMethod == MainStat.UpdateMethod.FixedTime && p_IsPlay)
            UpdateAnimation(initializeNode.UnscaleTime ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime);
    }

    public void Play()
    {
        if (!p_IsPlay && EnableToPlay)
        {
            if (eventCallbackNode.IsActiveThisNode) eventCallbackNode.BeginPlaying?.Invoke(AnimateMaterial);
            p_IsPlay = true;
            p_Age = 0;
            gameObject.SetActive(true);
            enabled = true;


            if (PositionNode.IsActiveThisNode && PositionNode.RandomMultiplier)
            {
                p_randomPos.x = UnityEngine.Random.Range(PositionNode.RandomMultiplierX.x, PositionNode.RandomMultiplierX.y);
                p_randomPos.y = UnityEngine.Random.Range(PositionNode.RandomMultiplierY.x, PositionNode.RandomMultiplierY.y);
                p_randomPos.z = UnityEngine.Random.Range(PositionNode.RandomMultiplierZ.x, PositionNode.RandomMultiplierZ.y);
            }
            else
            {
                p_randomPos = Vector3.one;
            }


            if (RotationNode.IsActiveThisNode && PositionNode.RandomMultiplier)
            {
                p_randomRot.x = UnityEngine.Random.Range(RotationNode.RandomMultiplierX.x, RotationNode.RandomMultiplierX.y);
                p_randomRot.y = UnityEngine.Random.Range(RotationNode.RandomMultiplierY.x, RotationNode.RandomMultiplierY.y);
                p_randomRot.z = UnityEngine.Random.Range(RotationNode.RandomMultiplierZ.x, RotationNode.RandomMultiplierZ.y);
            }
            else
            {
                p_randomRot = Vector3.one;
            }


            if (ScaleNode.IsActiveThisNode && PositionNode.RandomMultiplier)
            {
                p_randomSca.x = UnityEngine.Random.Range(ScaleNode.RandomMultiplierX.x, ScaleNode.RandomMultiplierX.y);
                p_randomSca.y = UnityEngine.Random.Range(ScaleNode.RandomMultiplierY.x, ScaleNode.RandomMultiplierY.y);
                p_randomSca.z = UnityEngine.Random.Range(ScaleNode.RandomMultiplierZ.x, ScaleNode.RandomMultiplierZ.y);
            }
            else
            {
                p_randomSca = Vector3.one;
            }
        }
    }

    public void Stop()
    {
        if (p_IsPlay)
        {
            if (eventCallbackNode.IsActiveThisNode) eventCallbackNode.DiedEvent?.Invoke(AnimateMaterial);
            p_IsPlay = false;
            p_Age = 0;

            if (initializeNode.Looping)
            {
                p_Age = 0;
                if (eventCallbackNode.IsActiveThisNode) eventCallbackNode.EveryEndOfEachLoop?.Invoke(AnimateMaterial);

                if (PositionNode.IsActiveThisNode && PositionNode.RandomMultiplier)
                {
                    p_randomPos.x = UnityEngine.Random.Range(PositionNode.RandomMultiplierX.x, PositionNode.RandomMultiplierX.y);
                    p_randomPos.y = UnityEngine.Random.Range(PositionNode.RandomMultiplierY.x, PositionNode.RandomMultiplierY.y);
                    p_randomPos.z = UnityEngine.Random.Range(PositionNode.RandomMultiplierZ.x, PositionNode.RandomMultiplierZ.y);
                }

                if (RotationNode.IsActiveThisNode && PositionNode.RandomMultiplier)
                {
                    p_randomRot.x = UnityEngine.Random.Range(RotationNode.RandomMultiplierX.x, RotationNode.RandomMultiplierX.y);
                    p_randomRot.y = UnityEngine.Random.Range(RotationNode.RandomMultiplierY.x, RotationNode.RandomMultiplierY.y);
                    p_randomRot.z = UnityEngine.Random.Range(RotationNode.RandomMultiplierZ.x, RotationNode.RandomMultiplierZ.y);
                }

                if (ScaleNode.IsActiveThisNode && PositionNode.RandomMultiplier)
                {
                    p_randomSca.x = UnityEngine.Random.Range(ScaleNode.RandomMultiplierX.x, ScaleNode.RandomMultiplierX.y);
                    p_randomSca.y = UnityEngine.Random.Range(ScaleNode.RandomMultiplierY.x, ScaleNode.RandomMultiplierY.y);
                    p_randomSca.z = UnityEngine.Random.Range(ScaleNode.RandomMultiplierZ.x, ScaleNode.RandomMultiplierZ.y);
                }

                return;
            }

            p_IsPlay = false;

            switch (destroyerNode.endingMethod)
            {
                case DestroyerNode.EndingMethodEnum.DisableThisComponent:
                    this.enabled = false;
                    return;
                case DestroyerNode.EndingMethodEnum.DisableThisComponentWithCallback:
                    this.enabled = false;
                    if (eventCallbackNode.IsActiveThisNode) eventCallbackNode.DiedEvent?.Invoke(AnimateMaterial);
                    return;
                case DestroyerNode.EndingMethodEnum.DestroyThisComponent:
                    Destroy(this);
                    return;
                case DestroyerNode.EndingMethodEnum.DestroyThisComponentWithCallback:
                    Destroy(this);
                    if (eventCallbackNode.IsActiveThisNode) eventCallbackNode.DiedEvent?.Invoke(AnimateMaterial);
                    return;
                case DestroyerNode.EndingMethodEnum.DisableThisObject:
                    gameObject.SetActive(false);
                    return;
                case DestroyerNode.EndingMethodEnum.DisableThisObjectWithCallback:
                    gameObject.SetActive(false);
                    if (eventCallbackNode.IsActiveThisNode) eventCallbackNode.DiedEvent?.Invoke(AnimateMaterial);
                    return;
                case DestroyerNode.EndingMethodEnum.DestroyThisObject:
                    Destroy(gameObject);
                    return;
                case DestroyerNode.EndingMethodEnum.DestroyThisObjectWithCallback:
                    Destroy(gameObject);
                    if (eventCallbackNode.IsActiveThisNode) eventCallbackNode.DiedEvent?.Invoke(AnimateMaterial);
                    return;
                case DestroyerNode.EndingMethodEnum.CallEventOnly:
                    if (eventCallbackNode.IsActiveThisNode) eventCallbackNode.DiedEvent?.Invoke(AnimateMaterial);
                    return;
            }
        }
    }

    public void UpdateAnimation(float elapseTime)
    {
        elapseTime = Mathf.Max(0, elapseTime);

        if (setFloatNode.IsActiveThisNode) UpdateFloat();
        if (setColorNode.IsActiveThisNode) UpdateColor();
        if (setTextureNode.IsActiveThisNode) UpdateTexture();
        if (AnimateTransform) UpdateTransform();

        if (p_Age >= initializeNode.LifeTime && elapseTime > 0)
        {
            if (initializeNode.Looping)
            {
                p_Age = 0;
                if (eventCallbackNode.IsActiveThisNode) eventCallbackNode.EveryEndOfEachLoop?.Invoke(AnimateMaterial);

                if (PositionNode.IsActiveThisNode && PositionNode.RandomMultiplier)
                {
                    p_randomPos.x = UnityEngine.Random.Range(PositionNode.RandomMultiplierX.x, PositionNode.RandomMultiplierX.y);
                    p_randomPos.y = UnityEngine.Random.Range(PositionNode.RandomMultiplierY.x, PositionNode.RandomMultiplierY.y);
                    p_randomPos.z = UnityEngine.Random.Range(PositionNode.RandomMultiplierZ.x, PositionNode.RandomMultiplierZ.y);
                }

                if (RotationNode.IsActiveThisNode && PositionNode.RandomMultiplier)
                {
                    p_randomRot.x = UnityEngine.Random.Range(RotationNode.RandomMultiplierX.x, RotationNode.RandomMultiplierX.y);
                    p_randomRot.y = UnityEngine.Random.Range(RotationNode.RandomMultiplierY.x, RotationNode.RandomMultiplierY.y);
                    p_randomRot.z = UnityEngine.Random.Range(RotationNode.RandomMultiplierZ.x, RotationNode.RandomMultiplierZ.y);
                }

                if (ScaleNode.IsActiveThisNode && PositionNode.RandomMultiplier)
                {
                    p_randomSca.x = UnityEngine.Random.Range(ScaleNode.RandomMultiplierX.x, ScaleNode.RandomMultiplierX.y);
                    p_randomSca.y = UnityEngine.Random.Range(ScaleNode.RandomMultiplierY.x, ScaleNode.RandomMultiplierY.y);
                    p_randomSca.z = UnityEngine.Random.Range(ScaleNode.RandomMultiplierZ.x, ScaleNode.RandomMultiplierZ.y);
                }

                return;
            }

            p_IsPlay = false;

            switch (destroyerNode.endingMethod)
            {
                case DestroyerNode.EndingMethodEnum.DisableThisComponent:
                    this.enabled = false;
                    return;
                case DestroyerNode.EndingMethodEnum.DisableThisComponentWithCallback:
                    this.enabled = false;
                    if (eventCallbackNode.IsActiveThisNode) eventCallbackNode.DiedEvent?.Invoke(AnimateMaterial);
                    return;
                case DestroyerNode.EndingMethodEnum.DestroyThisComponent:
                    Destroy(this);
                    return;
                case DestroyerNode.EndingMethodEnum.DestroyThisComponentWithCallback:
                    Destroy(this);
                    if (eventCallbackNode.IsActiveThisNode) eventCallbackNode.DiedEvent?.Invoke(AnimateMaterial);
                    return;
                case DestroyerNode.EndingMethodEnum.DisableThisObject:
                    gameObject.SetActive(false);
                    return;
                case DestroyerNode.EndingMethodEnum.DisableThisObjectWithCallback:
                    gameObject.SetActive(false);
                    if (eventCallbackNode.IsActiveThisNode) eventCallbackNode.DiedEvent?.Invoke(AnimateMaterial);
                    return;
                case DestroyerNode.EndingMethodEnum.DestroyThisObject:
                    Destroy(gameObject);
                    return;
                case DestroyerNode.EndingMethodEnum.DestroyThisObjectWithCallback:
                    Destroy(gameObject);
                    if (eventCallbackNode.IsActiveThisNode) eventCallbackNode.DiedEvent?.Invoke(AnimateMaterial);
                    return;
                case DestroyerNode.EndingMethodEnum.CallEventOnly:
                    if (eventCallbackNode.IsActiveThisNode) eventCallbackNode.DiedEvent?.Invoke(AnimateMaterial);
                    return;
            }
        }

        p_Age = Mathf.MoveTowards(p_Age, initializeNode.LifeTime, elapseTime);
    }

    public void IsEnablePlay(bool setTo)
    {
        EnableToPlay = setTo;
    }

    public void AssignEndProgress()
    {
        if (AnimateMaterial)
        {
            MaterialChanging = true;

            materialDefault.desMaterial = AnimateMaterial;
            materialDefault.floatNode = setFloatNode;
            materialDefault.colorNode = setColorNode;
            materialDefault.textureNode = setTextureNode;

            materialDefault.SetDefaultValue();

            SetAllToDefault.Add(materialDefault);

            Application.quitting += OnApplicationEnd;
        }
    }

    public static void OnApplicationEnd()
    {
        if (!MaterialChanging) return;
        MaterialChanging = false;

        foreach (QuitSetDefualt quitSetDefualt in SetAllToDefault)
        {
            quitSetDefualt.SetEverythingToDefault();
        }
    }



    #region UpdatePart

    private void UpdateFloat()
    {

        foreach (SetFloatNode.FloatInput xInput in setFloatNode.SetFloats)
        {
            AnimateMaterial.SetFloat(xInput.Name, xInput.AnimateCurve.Evaluate(p_Age / initializeNode.LifeTime) * xInput.Multiplier);
        }
    }

    private void UpdateColor()
    {

        foreach (SetColorNode.ColorInput xInput in setColorNode.SetColors)
        {
            AnimateMaterial.SetColor(xInput.Name, xInput.ColorOverLifeTime.Evaluate(p_Age / initializeNode.LifeTime));
        }
    }

    private void UpdateTexture()
    {


        foreach (SetTextureNode.TextureTimeStamp xInput in setTextureNode.SetTextures)
        {

            if (xInput.TimeStamp <= p_Age / initializeNode.LifeTime)
            {
                foreach (SetTextureNode.TextureInput yInput in xInput.textureInputs)
                {
                    AnimateMaterial.SetTexture(yInput.Name, yInput.SetToTexture);
                }
            }
        }
    }

    private void UpdateTransform()
    {
        if (PositionNode.IsActiveThisNode)
        {

            var xP = (PositionNode.XAxis.Evaluate(p_Age / initializeNode.LifeTime) * p_randomPos.x * PositionNode.ConstantMultiplier.x) + PositionNode.StarterValue.x;
            var yP = (PositionNode.YAxis.Evaluate(p_Age / initializeNode.LifeTime) * p_randomPos.y * PositionNode.ConstantMultiplier.y) + PositionNode.StarterValue.y;
            var zP = (PositionNode.ZAxis.Evaluate(p_Age / initializeNode.LifeTime) * p_randomPos.z * PositionNode.ConstantMultiplier.z) + PositionNode.StarterValue.z;

            if (!rectAnimate && (PositionNode.CalculateSpace == ThreeAxisAnimation.AnimateSpace.RectLocal || PositionNode.CalculateSpace == ThreeAxisAnimation.AnimateSpace.RectWorld
                || PositionNode.CalculateSpace == ThreeAxisAnimation.AnimateSpace.RectPureAnchor || PositionNode.CalculateSpace == ThreeAxisAnimation.AnimateSpace.RectCenterAnchor
                || PositionNode.CalculateSpace == ThreeAxisAnimation.AnimateSpace.LocalAnchorBL || PositionNode.CalculateSpace == ThreeAxisAnimation.AnimateSpace.WorldAnchorBL))
            {
                rectAnimate = AnimateTransform.GetComponent<RectTransform>();
                if (!rectAnimate) PositionNode.CalculateSpace = ThreeAxisAnimation.AnimateSpace.Local;
            }

            switch (PositionNode.CalculateSpace)
            {
                case ThreeAxisAnimation.AnimateSpace.Local:
                    AnimateTransform.transform.localPosition = new Vector3(xP, yP, zP);
                    break;
                case ThreeAxisAnimation.AnimateSpace.World:
                    AnimateTransform.transform.position = new Vector3(xP, yP, zP);
                    break;
                case ThreeAxisAnimation.AnimateSpace.RectLocal:
                    rectAnimate.localPosition = new Vector3(xP, yP, zP);
                    break;
                case ThreeAxisAnimation.AnimateSpace.RectWorld:
                    rectAnimate.position = new Vector3(xP, yP, zP);
                    break;
                case ThreeAxisAnimation.AnimateSpace.RectPureAnchor:
                    rectAnimate.anchorMin = new Vector2(xP, yP);
                    rectAnimate.anchorMax = new Vector2(xP, yP);
                    break;
                case ThreeAxisAnimation.AnimateSpace.RectCenterAnchor:
                    rectAnimate.anchorMin = new Vector2(xP, yP) * (Vector2.one * 0.5f) + new Vector2(0.5f, 0.5f);
                    rectAnimate.anchorMax = new Vector2(xP, yP) * (Vector2.one * 0.5f) + new Vector2(0.5f, 0.5f);
                    break;
                case ThreeAxisAnimation.AnimateSpace.LocalAnchorBL:
                    rectAnimate.localPosition = new Vector3(xP * Screen.width, yP * Screen.height, rectAnimate.position.z);
                    break;
                case ThreeAxisAnimation.AnimateSpace.WorldAnchorBL:
                    rectAnimate.position = new Vector3(xP * Screen.width, yP * Screen.height, rectAnimate.position.z);
                    break;
            }
        }

        if (RotationNode.IsActiveThisNode)
        {
            var xP = (RotationNode.XAxis.Evaluate(p_Age / initializeNode.LifeTime) * p_randomRot.x * RotationNode.ConstantMultiplier.x) + RotationNode.StarterValue.x;
            var yP = (RotationNode.YAxis.Evaluate(p_Age / initializeNode.LifeTime) * p_randomRot.y * RotationNode.ConstantMultiplier.y) + RotationNode.StarterValue.y;
            var zP = (RotationNode.ZAxis.Evaluate(p_Age / initializeNode.LifeTime) * p_randomRot.z * RotationNode.ConstantMultiplier.z) + RotationNode.StarterValue.z;

            switch (RotationNode.CalculateSpace)
            {
                case ThreeAxisAnimation.AnimateSpace.Local:
                    AnimateTransform.transform.localRotation = Quaternion.Euler(xP, yP, zP);
                    break;
                case ThreeAxisAnimation.AnimateSpace.World:
                    AnimateTransform.transform.rotation = Quaternion.Euler(xP, yP, zP);
                    break;
                case ThreeAxisAnimation.AnimateSpace.RectLocal:
                    rectAnimate.localRotation = Quaternion.Euler(xP, yP, zP);
                    break;
                case ThreeAxisAnimation.AnimateSpace.RectWorld:
                    rectAnimate.rotation = Quaternion.Euler(xP, yP, zP);
                    break;
                default:
                    rectAnimate.localRotation = Quaternion.Euler(xP, yP, zP);
                    break;
            }
        }

        if (ScaleNode.IsActiveThisNode)
        {
            var xP = (ScaleNode.XAxis.Evaluate(p_Age / initializeNode.LifeTime) * p_randomSca.x * ScaleNode.ConstantMultiplier.x) + ScaleNode.StarterValue.x;
            var yP = (ScaleNode.YAxis.Evaluate(p_Age / initializeNode.LifeTime) * p_randomSca.y * ScaleNode.ConstantMultiplier.y) + ScaleNode.StarterValue.y;
            var zP = (ScaleNode.ZAxis.Evaluate(p_Age / initializeNode.LifeTime) * p_randomSca.z * ScaleNode.ConstantMultiplier.z) + ScaleNode.StarterValue.z;

            switch (ScaleNode.CalculateSpace)
            {
                case ThreeAxisAnimation.AnimateSpace.Local:
                    AnimateTransform.transform.localScale = new Vector3(xP, yP, zP);
                    break;
                case ThreeAxisAnimation.AnimateSpace.World:
                    Transform parentGame = AnimateTransform.transform.parent;
                    if (parentGame)
                    {
                        AnimateTransform.transform.parent = null;
                        AnimateTransform.transform.localScale = new Vector3(xP, yP, zP);
                        AnimateTransform.transform.parent = parentGame;
                    }
                    else
                    {
                        AnimateTransform.transform.localScale = new Vector3(xP, yP, zP);
                    }
                    break;
                case ThreeAxisAnimation.AnimateSpace.RectLocal:
                    rectAnimate.localScale = new Vector3(xP, yP, zP);
                    break;
                case ThreeAxisAnimation.AnimateSpace.RectWorld:
                    Transform parentRect = rectAnimate.parent;
                    if (parentRect)
                    {
                        rectAnimate.parent = null;
                        rectAnimate.localScale = new Vector3(xP, yP, zP);
                        rectAnimate.parent = parentRect;
                    }
                    else
                    {
                        rectAnimate.localScale = new Vector3(xP, yP, zP);
                    }
                    break;
                default:
                    rectAnimate.localScale = new Vector3(xP, yP, zP);
                    break;
            }
        }
    }

    #endregion

}

#region Editor

#if UNITY_EDITOR

[CustomEditor(typeof(MaterialAnimationController)), CanEditMultipleObjects]
public class MaterialAnimationControllerEditor : Editor
{

    MaterialAnimationController animator;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        animator = (MaterialAnimationController)target;

        if (GUILayout.Button("Play") && Application.isPlaying)
        {
            ((MaterialAnimationController)target).Play();
        }
    }

}

#endif

#endregion


#region Node

[Serializable]
public class NodeBase
{
    public bool IsActiveThisNode = false;
}

[Serializable]
public class MainStat
{
    [Range(0.01f, 30f)]
    public float LifeTime = 1f;
    public bool PlayOnAwake = true;
    public bool PlayOnEnable = false;
    public bool Looping = true;
    public UpdateMethod updateMethod = UpdateMethod.EveryTick;
    public bool UnscaleTime = false;


    public enum UpdateMethod
    {
        EveryTick,
        FixedTime,
        None
    }
}

[Serializable]
public class SetFloatNode : NodeBase
{
    public List<FloatInput> SetFloats = new();
    [HideInInspector] public List<DefualtValue> DefualtValues = new();

    [Serializable]
    public class FloatInput
    {
        public string Name;
        public AnimationCurve AnimateCurve = AnimationCurve.Constant(0, 1, 0);
        public float Multiplier = 1f;
    }

    public class DefualtValue
    {
        public string Name;
        public float OldValue;
    }
}

[Serializable]
public class SetColorNode : NodeBase
{
    public List<ColorInput> SetColors = new();
    [HideInInspector] public List<DefualtValue> DefualtValues = new();

    [Serializable]
    public class ColorInput
    {
        public string Name;
        [GradientUsage(true)]
        public Gradient ColorOverLifeTime;
    }

    public class DefualtValue
    {
        public string Name;
        [ColorUsage(true)]
        public Color ColorBase;
    }
}

[Serializable]
public class SetTextureNode : NodeBase
{
    public List<TextureTimeStamp> SetTextures = new();
    [HideInInspector] public List<DefualtValue> DefualtValues = new();

    [Serializable]
    public class TextureTimeStamp
    {
        [Range(0f, 1f)]
        public float TimeStamp;
        public List<TextureInput> textureInputs = new();
    }

    [Serializable]
    public class TextureInput
    {
        public string Name;
        public Texture SetToTexture;

    }

    public class DefualtValue
    {
        public string Name;
        public Texture SetToTexture;
    }
}

[Serializable]
public class DestroyerNode : NodeBase
{
    public enum EndingMethodEnum
    {
        None,
        DisableThisComponent,
        DisableThisComponentWithCallback,
        DestroyThisComponent,
        DestroyThisComponentWithCallback,
        DisableThisObject,
        DisableThisObjectWithCallback,
        DestroyThisObject,
        DestroyThisObjectWithCallback,
        CallEventOnly
    }

    [Tooltip("Will Active Only Looping is false")]
    public EndingMethodEnum endingMethod = EndingMethodEnum.None;
}

[Serializable]
public class EventCallbackNode : NodeBase
{
    public UnityEvent<Material> BeginPlaying = new();

    [Tooltip("Work Only When Looping is true")]
    public UnityEvent<Material> EveryEndOfEachLoop = new();

    [Tooltip("Work Only When Looping is false, and Destroy Method has CallEvent")]
    public UnityEvent<Material> DiedEvent = new();
}

[Serializable]
public class ThreeAxisAnimation : NodeBase
{

    public AnimateSpace CalculateSpace = AnimateSpace.Local;
    public bool RandomMultiplier = false;

    public Vector3 StarterValue = Vector3.zero;
    public Vector3 ConstantMultiplier = Vector3.one;

    public AnimationCurve XAxis = AnimationCurve.Constant(0, 1, 0);
    public Vector2 RandomMultiplierX = Vector2.one;
    public AnimationCurve YAxis = AnimationCurve.Constant(0, 1, 0);
    public Vector2 RandomMultiplierY = Vector2.one;
    public AnimationCurve ZAxis = AnimationCurve.Constant(0, 1, 0);
    public Vector2 RandomMultiplierZ = Vector2.one;


    public enum AnimateSpace
    {
        Local,
        World,
        RectLocal,
        RectWorld,
        RectPureAnchor,
        RectCenterAnchor,
        LocalAnchorBL,
        WorldAnchorBL
    }
}


public class QuitSetDefualt
{
    public Material desMaterial;
    public SetFloatNode floatNode;
    public SetColorNode colorNode;
    public SetTextureNode textureNode;

    public void SetEverythingToDefault()
    {

        if (desMaterial == null) { return; }

        foreach (SetFloatNode.DefualtValue defualtValue in floatNode.DefualtValues)
        {
            desMaterial.SetFloat(defualtValue.Name, defualtValue.OldValue);
        }

        foreach (SetColorNode.DefualtValue defualtValue in colorNode.DefualtValues)
        {
            desMaterial.SetColor(defualtValue.Name, defualtValue.ColorBase);
        }

        foreach (SetTextureNode.DefualtValue defualtValue in textureNode.DefualtValues)
        {
            desMaterial.SetTexture(defualtValue.Name, defualtValue.SetToTexture);
        }
    }

    public void SetDefaultValue()
    {

        if (desMaterial == null) { return; }

        floatNode.DefualtValues.Clear();
        foreach (SetFloatNode.FloatInput floatInput in floatNode.SetFloats)
        {
            SetFloatNode.DefualtValue defaultFloat = new();
            defaultFloat.Name = floatInput.Name;
            defaultFloat.OldValue = desMaterial.GetFloat(floatInput.Name);
            floatNode.DefualtValues.Add(defaultFloat);
        }

        colorNode.DefualtValues.Clear();
        foreach (SetColorNode.ColorInput colorInput in colorNode.SetColors)
        {
            SetColorNode.DefualtValue defaultColor = new();
            defaultColor.Name = colorInput.Name;
            defaultColor.ColorBase = desMaterial.GetColor(colorInput.Name);
            colorNode.DefualtValues.Add(defaultColor);
        }

        textureNode.DefualtValues.Clear();
        foreach (SetTextureNode.TextureTimeStamp textureInputTime in textureNode.SetTextures)
        {

            foreach (SetTextureNode.TextureInput textureInput in textureInputTime.textureInputs)
            {
                SetFloatNode.DefualtValue defaultTex = new();
                defaultTex.Name = textureInput.Name;
                defaultTex.OldValue = desMaterial.GetFloat(textureInput.Name);
                floatNode.DefualtValues.Add(defaultTex);
            }

        }
    }
}


#endregion
