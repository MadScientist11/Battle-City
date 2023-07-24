using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BattleCity.PlayerLogic
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        public float collisionOffset = 0.05f;
        public ContactFilter2D movementFilter;
        private List<RaycastHit2D> castCollisions = new();
        private float _currentAngle;


        private Dictionary<MoveDirection, float> keyAngleMapping = new()
        {
            { MoveDirection.Up, 0 },
            { MoveDirection.Left, 90f },
            { MoveDirection.Down, 180f },
            { MoveDirection.Right, 270f },
        };

        private InputService _inputService;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            SetUpRigidbody(_rigidbody);

            _inputService = new InputService();
            _inputService.Initialize();
        }

        private void Update()
        {
            HandleRotation(keyAngleMapping[_inputService.MoveDirection]);
Debug.Log(_inputService.MoveDirection);
           
        }

        private void FixedUpdate()
        {
            Vector2 moveInput =
                _inputService.Movement;

            TryMovePlayer(moveInput);
        }

        private void HandleRotation(float angle)
        {
            _rigidbody.SetRotation(angle);
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
                movementFilter,
                castCollisions,
                5 * Time.fixedDeltaTime + collisionOffset);

            if (count == 0)
            {
                Vector2 moveVector = moveDirection * 5 * Time.fixedDeltaTime;
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