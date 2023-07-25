using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity.Source.Services
{
    public interface IFixedUpdatable
    {
        void FixedUpdate(float deltaTime);
    }
    public interface IFixedUpdateService
    {
        void AddListener(IFixedUpdatable listener);
        void RemoveListener(IFixedUpdatable listener);
    }
   
    public class FixedUpdateServiceService : MonoBehaviour, IInitializableService, IFixedUpdateService
    {
        private readonly List<IFixedUpdatable> _updateListeners = new();

        public void Initialize()
        {
            DontDestroyOnLoad(this);
        }

        private void FixedUpdate()
        {
            foreach (var updateListener in _updateListeners)
            {
                updateListener.FixedUpdate(Time.fixedDeltaTime);
            }
        }

        public void AddListener(IFixedUpdatable listener)
        {
            _updateListeners.Add(listener);
        }

        public void RemoveListener(IFixedUpdatable listener)
        {
            _updateListeners.Remove(listener);
        }
    }
}