using System;
using System.Collections;
using Scripts.Interfaces;
using Scripts.Tools;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
	public class PlayerMovement : ExtendedMonoBehaviour
	{
		//Components
		private Rigidbody2D rb;
		private PlayerManager player;
		private PlayerInput input;

		//Input actions
		private InputAction moveAction;
		private InputAction dashAction;

		//Parameters
		[SerializeField] private float basicSpeed = 5f;
		[SerializeField] private float dashSpeed = 30f;
		[SerializeField] private float dashTime = 0.1f;

		//Internal variables
		private Vector2 direction;
		private float speed;

		private void Start()
		{
			rb = GetComponent<Rigidbody2D>();
			player = GetComponentInParent<PlayerManager>();
			input = GetComponentInParent<PlayerInput>();

			moveAction = input.actions["Move"];
			dashAction = input.actions["Dash"];

			dashAction.performed += PerformDash;

			speed = basicSpeed;
		}

		private void OnDestroy()
		{
			dashAction.performed -= PerformDash;
		}

		private void Update()
		{
			if (player.State == PlayerManager.PlayerState.Dash) return;

			direction = moveAction.ReadValue<Vector2>();
			rb.velocity = direction.normalized * basicSpeed;
			player.MoveDirection = rb.velocity;
		}


		private void PerformDash(InputAction.CallbackContext context)
		{
			Dash(direction, dashSpeed, dashTime, null);
			player.AnimationController.Dash();
		}
		public void Dash(Vector2 direction, float speed, float time, Action func)
		{
			if (player.State == PlayerManager.PlayerState.Dash) return;

			player.State = PlayerManager.PlayerState.Dash;
			rb.velocity = direction.normalized * speed;
			StartCoroutine(Dashing(time, func));
		}
		private IEnumerator Dashing(float time, Action after)
		{
			yield return new WaitForSeconds(time);
			speed = basicSpeed;
			player.State = PlayerManager.PlayerState.Walk;
			after?.Invoke();
		}
	}
}
