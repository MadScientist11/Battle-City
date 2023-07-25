using BattleCity.Source.PlayerLogic;
using UnityEngine;
using VContainer;

namespace BattleCity.Source.Services
{
    public interface IProjectileFactory : IService
    {
        Projectile GetOrCreateProjectile(Vector3 spawnPoint, Quaternion rotation);
    }
    
    public class ProjectileFactory : IProjectileFactory, IInitializableService
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
            return GameObject.Instantiate(_projectilePrefab, spawnPoint, rotation);
        }
    }
}