using BattleCity.Source.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace BattleCity.Source.PlayerLogic
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private GameObject _projectile;
        [SerializeField] private Transform _projectileSpawnPoint;
        private IProjectileSystem _projectileSystem;

        [Inject]
        public void Construct(IProjectileSystem projectileSystem)
        {
            _projectileSystem = projectileSystem;
        }


        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Debug.Log("Fire");
                FireProjectile();
            }
            
        }

        private void FireProjectile()
        {
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, transform.up);
            _projectileSystem.LaunchProjectile(_projectileSpawnPoint.position, rotation);
        }
    }
}
