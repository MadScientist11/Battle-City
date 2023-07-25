using System;
using BattleCity.Source.PlayerLogic;
using UnityEngine;
using VContainer;

namespace BattleCity.Source.Services
{
    public interface IProjectileFactory : IService
    {
        Projectile GetOrCreateProjectile(Vector3 spawnPoint, Quaternion rotation);
    }
    
    public class ProjectileFactory : PooledFactory<Projectile>, IProjectileFactory, IInitializableService
    {
        private Projectile _projectilePrefab;
        
        private IAssetProvider _assetProvider;

        [Inject]
        public void Construct(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        
        public void Initialize()
        {
            _projectilePrefab = _assetProvider.LoadAsset<Projectile>(GameConstants.Assets.ProjectilePath);
        }

        public Projectile GetOrCreateProjectile(Vector3 spawnPoint, Quaternion rotation)
        {
            Projectile projectile = Get(null);
            projectile.transform.position = spawnPoint;
            projectile.transform.rotation = rotation;
            return projectile;
        }

        protected override Projectile Create()
        {
            return GameObject.Instantiate(_projectilePrefab);
        }

        protected override void Release(Projectile obj)
        {
            base.Release(obj);
            obj.Hide();
        }

        protected override Projectile Get(Func<Projectile, bool> predicate)
        {
            Projectile projectile = base.Get(predicate);
            projectile.Show();
            return projectile;
        }
    }
}