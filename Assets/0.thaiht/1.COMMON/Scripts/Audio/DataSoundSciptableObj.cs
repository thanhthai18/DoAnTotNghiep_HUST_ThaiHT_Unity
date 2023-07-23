using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace thaiht20183826
{
    [CreateAssetMenu(fileName = "DataSound", menuName = "Data/DataSound")]
    public class DataSoundSciptableObj : ScriptableObject
    {
        public DataSoundInfo[] list;

        public AudioClip FindClip(AudioClipEnum type)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].type == type)
                {
                    return list[i].audioClip;
                }
            }

            Debug.LogError("Not Found Clip=" + type);
            return list[0].audioClip;
        }

#if UNITY_EDITOR
        [Button("Add Enum")]
        private void AutoAddEnum()
        {
            string path = Application.dataPath + "/0.thaiht/1.COMMON/Scripts/Audio/AudioClipEnum.cs";
            var txt = File.ReadAllText(path).Replace("}", "");
            var txtContent = File.ReadAllText(path).Replace("}", "").Split('\n');
            for (int i = 0; i < list.Length; i++)
            {
                if (!string.IsNullOrEmpty(list[i].nameClip))
                {
                    var newEnum = name + "_" + list[i].nameClip + ",";
                    if (!txtContent.Contains(newEnum))
                        txt += newEnum + "\n";
                }
            }

            txt += "}";
            // Debug.LogError(txt);
            File.WriteAllText(path, txt);
            AssetDatabase.Refresh();
        }


        [Button("ReLoad")]
        public void OnCompileScripts()
        {
            for (int i = 0; i < list.Length; i++)
            {
                list[i].type = (AudioClipEnum)Enum.Parse(typeof(AudioClipEnum), name + "_" + list[i].nameClip);
            }
        }
#endif
    }

    [System.Serializable]
    public class DataSoundInfo
    {
#if UNITY_EDITOR
        public string nameClip;
#endif

        public AudioClip audioClip;
        public AudioClipEnum type;
    }
}
