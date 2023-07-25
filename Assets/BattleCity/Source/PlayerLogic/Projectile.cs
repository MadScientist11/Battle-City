using System;
using UnityEngine;

namespace BattleCity.Source.PlayerLogic
{
    public class Projectile : MonoBehaviour, IPoolable<Projectile>
    {
        public LayerMask AffectLayers { get; set; }
        public Action<Projectile> Release { get; set; }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ReleaseToPool()
        {
            Release?.Invoke(this);
        }
    }
}