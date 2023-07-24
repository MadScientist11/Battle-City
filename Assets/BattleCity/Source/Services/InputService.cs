using BattleCity.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BattleCity.Source.Services
{
    public interface IInputService : IService
    {
        public Vector2 Movement { get; }
        public bool MovementInputDetected { get; }
    }


    public class InputService : IInputService, IInitializableService, GameInput.IGameplayActions
    {
        private GameInput _input;

        public void Initialize()
        {
            _input = new GameInput();
            _input.Gameplay.SetCallbacks(this);
            _input.Gameplay.Enable();
        }

        public void DisablePlayerInput()
        {
            _input.Gameplay.Disable();
        }


        public void OnMovement(InputAction.CallbackContext context)
        {
            Vector2 unprocessedMovement = context.ReadValue<Vector2>();

            if (unprocessedMovement.sqrMagnitude < 0.001f)
            {
                Movement = Vector2.zero;
                MovementInputDetected = false;
                return;
            }

            MovementInputDetected = true;

            if (Mathf.Abs(unprocessedMovement.x) > Mathf.Abs(unprocessedMovement.y))
            {
                Movement = new Vector2(unprocessedMovement.x, 0);
            }
            else
            {
                Movement = new Vector2(0, unprocessedMovement.y);
            }
        }

        public Vector2 Movement { get; private set; }
        public bool MovementInputDetected { get; private set; }
    }
}