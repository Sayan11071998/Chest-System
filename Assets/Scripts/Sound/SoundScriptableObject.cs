using UnityEngine;

namespace ChestSystem.Sound
{
    [CreateAssetMenu(fileName = "SoundScriptableObject", menuName = "Sound/SoundScriptableObject")]
    public class SoundScriptableObject : ScriptableObject
    {
        public Sounds[] audioList;
    }
}