using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController {
    [Serializable]
    class PlayerJump
    {
        [Range(1, 10)]
        [Tooltip("Specifies the velocity of the caracter when jumping")]
        public float jumpVelocity = 1f;

        [Range(2, 10)]
        [Tooltip("Gravity scale when the character is falling from a long jump")]
        public float fallMultiplier = 2.5f;

        [Range(2, 10)]
        [Tooltip("Gravity scale when the character is falling from a short jump")]
        public float lowJumpMultiplier = 2.5f;

        internal bool jumpRequest;
        internal bool grounded;

        [Range(0, 2)]
        [Tooltip("Gravity scale when the character is falling from a short jump")]
        public float groundedSkin = 0.5f;

        [Tooltip("Layer or Layers to check for ground collisions")]
        public LayerMask mask;

        public Transform boxCenter;   
        internal Vector2 boxSize;
        internal Vector2 playerSize;

        internal Rigidbody2D _rigidBody;

        /// <param name="rigidbody">The rigid body of the current object (player).</param>
        public PlayerJump(Rigidbody2D rigidbody)
        {
            _rigidBody = rigidbody;
        }

        internal void Jump()
        {
            if (jumpRequest)
            {
                _rigidBody.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
                jumpRequest = false;
                grounded = false;
            }
            else
            {
                grounded = Physics2D.OverlapBox((Vector2)(boxCenter.position), boxSize, 0f, mask) != null;
            }
        }

        internal void EmulatedJump()
        {
            if (_rigidBody.velocity.y < 0)
            {
                _rigidBody.gravityScale = fallMultiplier;
            }
            else if (!Input.GetButton("Jump") && _rigidBody.velocity.y > 0)
            {
                _rigidBody.gravityScale = lowJumpMultiplier;
            }
            else
            {
                _rigidBody.gravityScale = 1f;
            }
        }
    }

    public class Player_Locomotion : MonoBehaviour
    {
        private Rigidbody2D _rigidBody;

        public float speed;

        [SerializeField]
        private PlayerJump playerJump;

        //[Range(1, 10)]
        //public float jumpVelocity = 1f;

        //public float fallMultiplier = 2.5f;
        //public float lowJumpMultiplier = 2.0f;

        private Vector2 moveVelocity;

        //private Boolean jumpRequest;

        //bool grounded;
        //public float groundedSkin;
        //public LayerMask mask;
        //Vector2 boxSize;
        //public Transform boxCenter;

        //public Vector2 playerSize;

        void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            playerJump._rigidBody = _rigidBody;
            playerJump.playerSize = GetComponent<BoxCollider2D>().size;
            playerJump.boxSize = new Vector2(playerJump.playerSize.x / 2, playerJump.groundedSkin);
            //playerJump.boxCenter = (Vector2)transform.position + (Vector2.down * (playerJump.playerSize.y + playerJump.boxSize.y) * 0.5f);
        }

        void Update()
        {
            playerJump.jumpRequest = Input.GetButtonDown("Jump") && playerJump.grounded;
            /* Normalize vector to obtain general direction and multiply by the speed*/
        }

        void FixedUpdate()
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), transform.position.y);
            moveVelocity = moveInput.normalized * speed;

            playerJump.Jump();
            playerJump.EmulatedJump();
            MovePlayer();
        }

        //void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.blue;
        //    //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        //        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        //    Gizmos.DrawWireCube(playerJump.boxCenter, playerJump.boxSize);
        //}

        //void Jump()
        //{
        //    if (jumpRequest)
        //    {
        //        _rigidBody.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
        //        jumpRequest = false;
        //        grounded = false;
        //    }
        //    else
        //    {
        //        grounded = (Physics2D.OverlapBox((Vector2)(boxCenter.position), boxSize, 0f, mask) != null);
        //    }
        //}

        //private void EmulatedJump()
        //{
        //    if (_rigidBody.velocity.y < 0)
        //    {
        //        _rigidBody.gravityScale = fallMultiplier;
        //    }
        //    else if (!Input.GetButton("Jump") && _rigidBody.velocity.y > 0)
        //    {
        //        _rigidBody.gravityScale = lowJumpMultiplier;
        //    }
        //    else
        //    {
        //        _rigidBody.gravityScale = 1f;
        //    }
        //}

        private void MovePlayer()
        {
            if (playerJump.grounded)
            {
                _rigidBody.velocity = new Vector2(moveVelocity.x, _rigidBody.velocity.y);
            }
            else
            {
                _rigidBody.velocity = new Vector2(moveVelocity.x / 2, _rigidBody.velocity.y);
            }
        }
    }

    public class Player_Controller: MonoBehaviour
    {
        public Player_Stats playerInfo; 

        private void Start() {
            playerInfo.playerPosition = transform;
        }
    }
}