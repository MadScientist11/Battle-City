using BattleCity.Source.PlayerLogic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BattleCity.Source.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerConfiguration _playerConfiguration;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_playerConfiguration);
        }
    }
}
