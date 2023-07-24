using UnityEngine;

namespace BattleCity.Source.PlayerLogic
{
    [CreateAssetMenu(menuName = "BattleCity/PlayerConfiguration", fileName = "PlayerConfiguration")]
    public class PlayerConfiguration : ScriptableObject
    {
        public ContactFilter2D MovementFilter;
        public float CollisionOffset;
        public float MoveSpeed;
    }
}
