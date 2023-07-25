using System.Collections.Generic;
using BattleCity.Source.PlayerLogic;
using UnityEngine;
using VContainer;

namespace BattleCity.Source.Services
{
    public interface IProjectileSystem : IService
    {
        void LaunchProjectile(Vector3 startingPoint, Quaternion rotation);
    }

    public class ProjectileSystem : IProjectileSystem, IInitializableService, IFixedUpdatable
    {
        private readonly HashSet<Projectile> _launchedProjectiles = new();

        private IProjectileFactory _projectileFactory;
        private IFixedUpdateService _fixedUpdate;

        [Inject]
        public void Construct(IProjectileFactory projectileFactory, IFixedUpdateService fixedUpdate)
        {
            _fixedUpdate = fixedUpdate;
            _projectileFactory = projectileFactory;
        }

        public void Initialize()
        {
            _fixedUpdate.AddListener(this);
        }

        public void LaunchProjectile(Vector3 startingPoint, Quaternion rotation)
        {
            Projectile projectile = _projectileFactory.GetOrCreateProjectile(startingPoint, rotation);
            _launchedProjectiles.Add(projectile);
        }

        public void FixedUpdate(float deltaTime)
        {
            foreach (Projectile projectile in _launchedProjectiles)
            {
                projectile.transform.position += projectile.transform.up * 5 *deltaTime;
            }
        }
    }

   
}
