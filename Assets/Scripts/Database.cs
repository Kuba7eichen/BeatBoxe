using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCSystem
{
    /// <summary>
    /// Closed Caption Database is an asset that map given AudioClip to a list of lines & time used by the CCManager
    /// </summary>
    [CreateAssetMenu(fileName = "Database", menuName = "ObjectSpawnEvent/Database")]
    public class Database : ScriptableObject
    {
        [System.Serializable]
        public class Timestamp
        {
            public float SpawnSecond;
            [Range(1, 3)] public int Lane;
            public GameObject prefabToSpawn;
        }

        [System.Serializable]
        public class Entry
        {
            public Timestamp[] SpawnEvents = new Timestamp[0];
        }

        public Timestamp[] DatabaseEntries = new Timestamp[0];

        // private Dictionary<AudioClip, Entry> spawnEventsTimeline;

        //public void BuildMap()
        //{
        //    spawnEventsTimeline = new Dictionary<AudioClip, Entry>();
        //    for (int i = 0; i < DatabaseEntries.Length; ++i)
        //    {
        //        spawnEventsTimeline.Add(DatabaseEntries[i].clip, DatabaseEntries[i]);
        //    }
        //}

        //public string GetTextEntry(AudioClip clip, float time)
        //{
        //    Entry entry;
        //    if (m_AudioToEntryMap.TryGetValue(clip, out entry))
        //    {
        //        int count = entry.Lines.Length;
        //        for (int i = 0; i < count; ++i)
        //        {
        //            if (i == count - 1 || (entry.Lines[i].SpawnSecond < time && entry.Lines[i + 1].SpawnSecond > time))
        //            {
        //                return entry.Lines[i].Lane;
        //            }
        //        }
        //    }

        //    return "CLOSED_CAPTION_MISSING";
        //}
    }
}