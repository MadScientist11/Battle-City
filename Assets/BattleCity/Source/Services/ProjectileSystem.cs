using System.Collections.Generic;
using BattleCity.Source.PlayerLogic;
using UnityEngine;
using VContainer;

namespace BattleCity.Source.Services
{
    public interface IProjectileSystem : IService
    {
        void LaunchProjectile(Vector3 startingPoint, Quaternion rotation, LayerMask affectedLayers);
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

        public void LaunchProjectile(Vector3 startingPoint, Quaternion rotation, LayerMask affectedLayers)
        {
            Projectile projectile = _projectileFactory.GetOrCreateProjectile(startingPoint, rotation);
            projectile.AffectLayers = affectedLayers;
            _launchedProjectiles.Add(projectile);
        }

        public void FixedUpdate(float deltaTime)
        {
            foreach (Projectile projectile in _launchedProjectiles)
            {
                Transform projectileTransform = projectile.transform;
                if (CheckForHit(projectile, deltaTime, out RaycastHit2D hit))
                {
                    projectile.ReleaseToPool();
                }

                projectileTransform.position += projectileTransform.up * 5 * deltaTime;
            }
        }

        private bool CheckForHit(Projectile projectile, float deltaTime, out RaycastHit2D hit)
        {
            RaycastHit2D potentialHit =
                Physics2D.Raycast(projectile.transform.position, projectile.transform.up, 5 * deltaTime,
                    projectile.AffectLayers);
            if (potentialHit.transform != null)
            {
                hit = potentialHit;
                return true;
            }

            hit = default;
            return false;
        }
    }
}