using UnityEngine;
using System.Collections.Generic;

public static class GameTimeManager
{
    public static double gameTime = Time.time;
    public static double sceneTime = Time.timeSinceLevelLoadAsDouble;
    private static GameTimeManagerObj gameTimeManagerObj = null;

    public static List<TimeTag> timeTag { get; private set; }

    public static TimeTag AddTimeTag(string createTag)
    {

        if (!gameTimeManagerObj)
        {
            gameTimeManagerObj = new GameObject().AddComponent<GameTimeManagerObj>();
        }

        if (timeTag == null) timeTag = new List<TimeTag>();

        List<TimeTag> findRe = new();
        findRe = timeTag.FindAll(x => x.IDName.Equals(createTag));

        if (findRe.Count > 0)
        {
            Debug.LogWarning($"Can't Add a Tag name {createTag} to GameTimeManager");
            return null;
        }

        TimeTag createTimeTag = new TimeTag(createTag);
        GameTimeManager.timeTag.Add(createTimeTag);

        return createTimeTag;
    }

    public static TimeTag AddTimeTag(TimeTag createTag)
    {

        if (!gameTimeManagerObj)
        {
            gameTimeManagerObj = new GameObject().AddComponent<GameTimeManagerObj>();
        }

        if (timeTag == null) timeTag = new List<TimeTag>();

        List<TimeTag> findRe = new();
        findRe = timeTag.FindAll(x => x == createTag);

        if (findRe.Count > 0)
        {
            Debug.LogWarning($"Can't Add a Tag name {createTag.IDName} to GameTimeManager");
            return null;
        }

        TimeTag createTimeTag = createTag;
        timeTag.Add(createTimeTag);

        return createTimeTag;
    }

    public static TimeTag GetTimeTag(string TagID)
    {

        if (!gameTimeManagerObj)
        {
            gameTimeManagerObj = new GameObject().AddComponent<GameTimeManagerObj>();
        }

        if (timeTag == null) timeTag = new List<TimeTag>();

        List<TimeTag> findRe = new();
        findRe = timeTag.FindAll(x => x.IDName.Equals(TagID));

        TimeTag createTimeTag = new TimeTag(TagID);

        if (findRe.Count <= 0)
        {
            createTimeTag = AddTimeTag(TagID);
        }
        else
        {
            createTimeTag = findRe[0];
        }

        return createTimeTag;
    }

    public static TimeTag GetTimeTag(TimeTag TagRef)
    {

        if (!gameTimeManagerObj)
        {
            gameTimeManagerObj = new GameObject().AddComponent<GameTimeManagerObj>();
        }

        if (timeTag == null) timeTag = new List<TimeTag>();


        List<TimeTag> findRe = new();
        findRe = timeTag.FindAll(x => x == TagRef);


        TimeTag createTimeTag;

        if (findRe.Count <= 0)
        {
            createTimeTag = new TimeTag(TagRef);
            createTimeTag = AddTimeTag(TagRef);
        }
        else
        {
            createTimeTag = findRe[0];
        }

        return createTimeTag;
    }

    public static void ResetRecordTime(string TagID)
    {
        TimeTag tagTarget = GetTimeTag(TagID);

        tagTarget.tagRecordTime = 0;
    }

    public static void ResetRecordTime(TimeTag TagRef)
    {
        TimeTag tagTarget = GetTimeTag(TagRef);

        tagTarget.tagRecordTime = 0;
    }
    
    public static void ResetRecordTime()
    {

        if (!gameTimeManagerObj)
        {
            gameTimeManagerObj = new GameObject().AddComponent<GameTimeManagerObj>();
        }

        foreach (TimeTag tag in timeTag)
        {
            tag.tagRecordTime = 0;
        }
    }


    //--------------------------------------------------------//

    public class TimeTag
    {

        public string IDName { get; private set; }

        public float tagUnscaleDeltaTime = 0.016667f;
        public float tagTimeScale = 1f;
        public double tagTime = Time.time;
        public double tagRecordTime = Time.timeSinceLevelLoadAsDouble;
        public float tagUnityDeltaTime
        {
            get => tagTimeScale * Time.deltaTime;
        }

        private TimeTag()
        {

        }

        public TimeTag(string TagID)
        {
            IDName = TagID;
        }

        public TimeTag(TimeTag TagRef)
        {
            IDName = TagRef.IDName;
            tagUnscaleDeltaTime = TagRef.tagUnscaleDeltaTime;
            tagTimeScale = TagRef.tagTimeScale;
            tagTime = TagRef.tagTime;
            tagRecordTime = TagRef.tagRecordTime;
        }


    }
}



public class GameTimeManagerObj : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        foreach (GameTimeManager.TimeTag tags in GameTimeManager.timeTag)
        {
            tags.tagTime += tags.tagUnityDeltaTime;
            tags.tagRecordTime += tags.tagUnityDeltaTime;
        }
    }
}