using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundAudioManager : MonoBehaviour
{

    public static SoundAudioManager instance
    {
        get
        {
            List<Object> result = FindObjectsByType(typeof(SoundAudioManager), FindObjectsInactive.Include, FindObjectsSortMode.InstanceID).ToList();
            if (result.Count > 1)
            {
                while (result.Count > 1)
                {
                    var obj = result[0];
                    result.Remove(obj);
                    DestroyImmediate(obj);
                }


                return result[0] as SoundAudioManager;
            }
            else if (result.Count <= 0)
            {
                var obj = new GameObject("SoundAudioManager");
                var component = obj.AddComponent<SoundAudioManager>();
                return component;
            }
            else
            {
                return result[0] as SoundAudioManager;
            }
        }
    }

    public static List<SoundAudioReference> soundReference = new();
    public static List<AudioSource> childrenSource = new List<AudioSource>();

    public static Dictionary<SoundAudioReference, AudioSource> keySound = new Dictionary<SoundAudioReference, AudioSource>();


    private void Awake()
    {
        if (!initialized) Initialize();

    }

    private bool initialized = false;
    public bool is_initialized
    {
        get => initialized;
    }
    private void Initialize()
    {
        DontDestroyOnLoad(this.gameObject);

        while (transform.childCount > 0)
        {
            var child = transform.GetChild(0);
            Destroy(child);
        }

        childrenSource.Clear();
        keySound.Clear();


        int i = 1;

        foreach (var child in soundReference)
        {
            var spawnObj = new GameObject($"Sound {i}. {child.name} ");
            spawnObj.transform.parent = transform;
            var component = spawnObj.AddComponent<AudioSource>();
            childrenSource.Add(component);

            keySound.Add(child, component);

            if (child.AudioResource) component.resource = child.AudioResource;
            if (child.AudioMixerGroup) component.outputAudioMixerGroup = child.AudioMixerGroup;
            component.mute = child.Mute;
            component.bypassEffects = child.Bypass_Effect;
            component.bypassListenerEffects = child.Bypass_Listener_Effect;
            component.bypassReverbZones = child.Bypass_Reverb_Zone;
            component.playOnAwake = child.PlayOnAwake;
            component.loop = child.Loop;
            component.priority = child.Priority;
            component.volume = child.Volume;
            component.pitch = child.Pitch;
            component.panStereo = child.StereoPan;
            component.spatialBlend = child.SpatialBlend;
            component.reverbZoneMix = child.ReverbZoneMix;
        }

        initialized = true;
    }

    public void TryReInitialize(SoundAudioReference soundRef)
    {
        if (keySound.TryGetValue(soundRef, out var audioSource) || keySound.ContainsKey(soundRef))
        {
            return;
        }

        int i = 1;

        var child = soundRef;

        var spawnObj = new GameObject($"Sound {i}. {child.name} ");
        spawnObj.transform.parent = transform;
        var component = spawnObj.AddComponent<AudioSource>();
        childrenSource.Add(component);

        keySound.Add(child, component);

        if (child.AudioResource) component.resource = child.AudioResource;
        if (child.AudioMixerGroup) component.outputAudioMixerGroup = child.AudioMixerGroup;
        component.mute = child.Mute;
        component.bypassEffects = child.Bypass_Effect;
        component.bypassListenerEffects = child.Bypass_Listener_Effect;
        component.bypassReverbZones = child.Bypass_Reverb_Zone;
        component.playOnAwake = child.PlayOnAwake;
        component.loop = child.Loop;
        component.priority = child.Priority;
        component.volume = child.Volume;
        component.pitch = child.Pitch;
        component.panStereo = child.StereoPan;
        component.spatialBlend = child.SpatialBlend;
        component.reverbZoneMix = child.ReverbZoneMix;
    }

    public static void Play(SoundAudioReference reference)
    {
        if (SoundAudioManager.instance)
            reference.PlayAudio();
    }

    public static void Pause(SoundAudioReference reference)
    {
        if (SoundAudioManager.instance)
            reference.PauseAudio();
    }

    public static void Resume(SoundAudioReference reference)
    {
        if (SoundAudioManager.instance)
            reference.ResumeAudio();
    }

    public static void Stop(SoundAudioReference reference)
    {
        if (SoundAudioManager.instance)
            reference.StopAudio();
    }


    public static void PlayAll()
    {
        if (SoundAudioManager.instance)
            foreach (var child in childrenSource)
            {
                child.Play();
            }
    }

    public static void PauseAll()
    {
        if (SoundAudioManager.instance)
            foreach (var child in childrenSource)
            {
                child.Pause();
            }
    }

    public static void ResumeAll()
    {
        if (SoundAudioManager.instance)
            foreach (var child in childrenSource)
            {
                child.UnPause();
            }
    }

    public static void StopAll()
    {
        if (SoundAudioManager.instance)
            foreach (var child in childrenSource)
            {
                child.Stop();
            }
    }


}
