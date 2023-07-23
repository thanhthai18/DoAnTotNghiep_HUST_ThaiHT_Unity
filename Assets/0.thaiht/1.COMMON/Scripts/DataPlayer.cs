using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace thaiht20183826
{
    public static class DataPlayer
    {
        public const string KEY_DATA_0 = "keydata0";
        private static NewData newData;

        static DataPlayer()
        {
            newData = JsonUtility.FromJson<NewData>(PlayerPrefs.GetString(KEY_DATA_0));
            if(newData == null)
            {
                newData = new NewData()
                {
                    value0 = 0,
                    value1 = 1,
                };
                SaveData();
            }
        }

        static void SaveData()
        {
            PlayerPrefs.SetString(KEY_DATA_0, JsonUtility.ToJson(newData));
        }

        public static float GetValue0()
        {
            return newData.value0;
        }
    }

    public class NewData
    {
        public float value0;
        public float value1;
    }
}
