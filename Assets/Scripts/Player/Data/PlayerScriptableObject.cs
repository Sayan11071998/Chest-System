using UnityEngine;

namespace ChestSystem.Player.Data
{
    [CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "Player/PlayerScriptableObject")]
    public class PlayerScriptableObject : ScriptableObject
    {
        public int coinCount;
        public int gemsCount;
    }
}