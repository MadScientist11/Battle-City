using System.Collections.Generic;
using BattleCity.Source.Services;
using UnityEngine;
using VContainer;

namespace BattleCity.Source.PlayerLogic
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class PlayerMovement : MonoBehaviour
    {
        private List<RaycastHit2D> castCollisions = new();

        private Rigidbody2D _rigidbody;
        
        private IInputService _inputService;
        private PlayerConfiguration _playerConfig;

        [Inject]
        public void Construct(IInputService inputService, PlayerConfiguration playerConfiguration)
        {
            _inputService = inputService;
            _playerConfig = playerConfiguration;
        }
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            SetUpRigidbody(_rigidbody);
        }

        private void FixedUpdate()
        {
            if(_inputService.MovementInputDetected is false)
                return;
            
            Vector2 moveInput = _inputService.Movement;
            HandleRotation(moveInput);
            TryMovePlayer(moveInput);
        }

        private void HandleRotation(Vector2 moveInput)
        {
            Vector2 movementDirection = moveInput.normalized;
            float rotation = Vector2.SignedAngle(Vector2.up, movementDirection);
            _rigidbody.SetRotation(rotation);
        }

        private void TryMovePlayer(Vector2 moveInput)
        {
            bool success = MovePlayer(moveInput);

            if (!success)
            {
                success = MovePlayer(new Vector2(moveInput.x, 0));

                if (!success)
                {
                    success = MovePlayer(new Vector2(0, moveInput.y));
                }
            }
        }

        private bool MovePlayer(Vector2 moveDirection)
        {
            int count = _rigidbody.Cast(moveDirection,
                _playerConfig.MovementFilter,
                castCollisions,
                _playerConfig.MoveSpeed * Time.fixedDeltaTime + _playerConfig.CollisionOffset);

            if (count == 0)
            {
                Vector2 moveVector = moveDirection * _playerConfig.MoveSpeed * Time.fixedDeltaTime;
                _rigidbody.MovePosition(_rigidbody.position + moveVector);
                return true;
            }

            return false;
        }

        private void SetUpRigidbody(Rigidbody2D rigidbody)
        {
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            rigidbody.useFullKinematicContacts = true;
        }

        private float GetRotationAngle(Vector2 dir2)
        {
            float rotationAngle = Vector2.SignedAngle(Vector2.up, dir2);
            int clampedAngle = Mathf.RoundToInt(rotationAngle / 90f);

            return clampedAngle;
        }

        private float ClampToNearestAngle(Vector2 direction)
        {
            direction = direction.normalized;
            float angle = (float)Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return Mathf.Round(angle / 90) * 90;
        }
    }
}