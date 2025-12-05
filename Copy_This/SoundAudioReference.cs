using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

#if UNITY_EDITOR

using UnityEditor;

#endif


[CreateAssetMenu(menuName = "Sound/SoundReference", fileName = "SoundReference")]
public class SoundAudioReference : ScriptableObject
{
    public SoundMode soundMode = SoundMode.SFX;
    public AudioResource AudioResource;
    public AudioMixerGroup AudioMixerGroup;
    public bool Mute;
    public bool Bypass_Effect;
    public bool Bypass_Listener_Effect;
    public bool Bypass_Reverb_Zone;
    public bool PlayOnAwake = true;
    public bool Loop;
    public int Priority = 128;
    public float Volume = 1;
    public float Pitch = 1;
    public float StereoPan;
    public float SpatialBlend;
    public float ReverbZoneMix = 1;


    private void Initialize()
    {
        if (SoundAudioManager.soundReference == null)
        {
            SoundAudioManager.soundReference = new();
        }

        if (!SoundAudioManager.soundReference.Contains(this))
        {
            SoundAudioManager.soundReference.Add(this);
            SoundAudioManager.instance.TryReInitialize(this);
        }
    }

    public void PlayAudio()
    {
        this.Initialize();
        if (SoundAudioManager.instance)
        {
            if (SoundAudioManager.keySound.TryGetValue(this, out var source))
            {

                switch (soundMode)
                {
                    case SoundMode.SFX:
                        if (source.clip)
                            source.PlayOneShot(source.clip);
                        else
                            source.Play();
                        break;
                    case SoundMode.Music:
                        source.Play();
                        break;
                }
            }
        }
    }

    public void PauseAudio()
    {
        Initialize();
        if (SoundAudioManager.instance)
        {
            if (SoundAudioManager.keySound.TryGetValue(this, out var source))
            {
                source.Pause();
            }
        }
    }

    public void StopAudio()
    {
        Initialize();
        if (SoundAudioManager.instance)
        {
            if (SoundAudioManager.keySound.TryGetValue(this, out var source))
            {
                source.Stop();
            }
        }
    }

    public void ResumeAudio()
    {
        Initialize();
        if (SoundAudioManager.instance)
        {
            SoundAudioManager.keySound.TryGetValue(this, out var source);
            source.UnPause();
        }
    }

    public static void PlayAll(int specific_mode)
    {
        if (SoundAudioManager.instance)
        {
            List<SoundAudioReference> musics = SoundAudioManager.soundReference.FindAll(x => x.soundMode == (SoundMode)specific_mode);
            foreach (SoundAudioReference reference in musics)
            {
                reference.PlayAudio();
            }
        }
    }

    public static void PauseAll(int specific_mode)
    {
        if (SoundAudioManager.instance)
        {
            List<SoundAudioReference> musics = SoundAudioManager.soundReference.FindAll(x => x.soundMode == (SoundMode)specific_mode);
            foreach (SoundAudioReference reference in musics)
            {
                reference.PauseAudio();
            }
        }
    }

    public static void ResumeAll(int specific_mode)
    {
        if (SoundAudioManager.instance)
        {
            List<SoundAudioReference> musics = SoundAudioManager.soundReference.FindAll(x => x.soundMode == (SoundMode)specific_mode);
            foreach (SoundAudioReference reference in musics)
            {
                reference.ResumeAudio();
            }
        }
    }

    public static void StopAll(int specific_mode)
    {
        if (SoundAudioManager.instance)
        {
            List<SoundAudioReference> musics = SoundAudioManager.soundReference.FindAll(x => x.soundMode == (SoundMode)specific_mode);
            foreach (SoundAudioReference reference in musics)
            {
                reference.StopAudio();
            }
        }
    }

    public static void PlayAll()
    {
        if (SoundAudioManager.instance)
            SoundAudioManager.PlayAll();
    }

    public static void PauseAll()
    {
        if (SoundAudioManager.instance)
            SoundAudioManager.PauseAll();
    }

    public static void ResumeAll()
    {
        if (SoundAudioManager.instance)
            SoundAudioManager.ResumeAll();
    }

    public static void StopAll()
    {
        if (SoundAudioManager.instance)
            SoundAudioManager.StopAll();
    }

    private void OnDestroy()
    {
        if (SoundAudioManager.soundReference == null)
        {
            SoundAudioManager.soundReference = new();
        }

        if (SoundAudioManager.soundReference.Contains(this))
        {
            SoundAudioManager.soundReference.Remove(this);
        }
    }

    public enum SoundMode
    {
        Music = 1,
        SFX = 2
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(SoundAudioReference))]
    public class SoundAudioReferenceEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SoundAudioReference mainTarget = (SoundAudioReference)target;

            if (GUILayout.Button("Try Set Name"))
            {


                if (!mainTarget.AudioResource)
                {
                    Debug.LogError($"There is Unassigned AudioResource in {mainTarget.name}. Abort is Fuction");
                    return;
                }

                string assetsPath = AssetDatabase.GetAssetPath(mainTarget);
                UnityEditor.AssetDatabase.RenameAsset(assetsPath, $"{mainTarget.AudioResource.name} Reference");
                UnityEditor.EditorUtility.SetDirty(mainTarget);
            }
        }
    }

    public static class SoundAudioContextMenu
    {
        [MenuItem("Assets/Create/Sound/SoundReferenceAssigned")]
        private static void CreateObjectFromAudioResource(MenuCommand menuCommand)
        {
            AudioResource resource = Selection.activeObject as AudioResource;

            if (resource == null)
            {
                Debug.LogWarning("No Resource Selected.");
                return;
            }

            SoundAudioReference createRef = ScriptableObject.CreateInstance<SoundAudioReference>();
            createRef.AudioResource = resource;

            string path = AssetDatabase.GetAssetPath(resource);

            path = path.Replace(".mp3", ".asset");
            path = AssetDatabase.GenerateUniqueAssetPath(path);

            AssetDatabase.CreateAsset(createRef, path);
            AssetDatabase.RenameAsset(path, $"{createRef.AudioResource.name} Reference");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = createRef;

        }
    }

#endif

}
