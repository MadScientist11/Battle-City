using UnityEngine;
using UnityEngine.InputSystem;

namespace BattleCity.Source.PlayerLogic
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private GameObject _projectile;
        [SerializeField] private Transform _projectileSpawnPoint;


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

            Instantiate(_projectile, _projectileSpawnPoint.position,
                Quaternion.LookRotation(Vector3.forward, transform.up));

        }
    }
}
