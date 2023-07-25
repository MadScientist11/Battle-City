using BattleCity.Source.Services;
using VContainer;
using VContainer.Unity;

namespace BattleCity.Source.Scopes
{
    public class AppLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterServices(builder);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<SceneLoader>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ProjectileSystem>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ProjectileFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AssetProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponentOnNewGameObject<FixedUpdateServiceService>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
