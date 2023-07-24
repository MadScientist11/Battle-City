using BattleCity.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public interface IInputService : IService
    {
        public Vector2 Movement { get; }
        public MoveDirection MoveDirection { get; }
    }

    public interface IService
    {
    }

    public enum MoveDirection
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
    }

    public class InputService : IInputService, GameInput.IGameplayActions
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
                return;
            }

            if (Mathf.Abs(unprocessedMovement.x) > Mathf.Abs(unprocessedMovement.y))
            {
                Movement = new Vector2(unprocessedMovement.x, 0);
                MoveDirection = unprocessedMovement.x > 0 ? MoveDirection.Right : MoveDirection.Left;
            }
            else
            {
                Movement = new Vector2(0, unprocessedMovement.y);
                MoveDirection = unprocessedMovement.y > 0 ? MoveDirection.Up : MoveDirection.Down;
            }
        }

        public Vector2 Movement { get; private set; }
        public MoveDirection MoveDirection { get; private set; }
    }
}