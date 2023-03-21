﻿using System.Collections;
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
            public int Lane;
            public GameObject prefabToSpawn;
        }
    
        [System.Serializable]
        public class Entry
        {            
            public Timestamp[] SpawnEvent = new Timestamp[0];
        }

        public Entry[] DatabaseEntries = new Entry[0];

        Dictionary<AudioClip, Entry> m_AudioToEntryMap;

        public void BuildMap()
        {
            m_AudioToEntryMap = new Dictionary<AudioClip, Entry>();
            for (int i = 0; i < DatabaseEntries.Length; ++i)
            {
              //  m_AudioToEntryMap.Add(DatabaseEntries[i].clip, DatabaseEntries[i]);
            }
        }

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