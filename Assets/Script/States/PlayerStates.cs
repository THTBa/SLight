using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerIdleState : State {
    Animator animator;
    AudioSource audio;

    public PlayerIdleState() {
        name = "Idle";
    }

    public override void OnEnter() {
        // Debug.Log("Idle OnEnter");

        animator = stateMachine.gameObject.GetComponent<Animator>();
        audio = stateMachine.gameObject.GetComponent<AudioSource>();
        Assert.IsNotNull(animator);
        Assert.IsNotNull(audio);

        animator.SetBool("IsIdling", true);

        AudioClip clip = Resources.Load("SoundEffect/脚步/脚步-硬地") as AudioClip;
        Assert.IsNotNull(clip);
        audio.PlayOneShot(clip);
    }

    public override void Update() {
        // Jump
        if (GameKernel.inputManager.GetKey(InputKey.Jump)) {
            stateMachine.ChangeState("Jump");
            return;
        }
        // Walk
        if (GameKernel.inputManager.GetKey(InputKey.Left) || GameKernel.inputManager.GetKey(InputKey.Right)) {
            stateMachine.ChangeState("Walk");
            return;
        }
    }

    public override void OnExit() {
        // Debug.Log("Idle OnExit");
        animator.SetBool("IsIdling", false);
    }
}

public class PlayerWalkState : State {
    Animator animator;
    Collider2D collider;
    Rigidbody2D body;
    AudioSource audio;
    PlayerControl script;

    float soundDelta = 0.4f;
    float deltaTime = 0.0f;

    public PlayerWalkState() {
        name = "Walk";
    }

    public override void OnEnter() {
        // Debug.Log("Walk OnEnter");
        animator = stateMachine.gameObject.GetComponent<Animator>();
        collider = stateMachine.gameObject.GetComponent<Collider2D>();
        body = stateMachine.gameObject.GetComponent<Rigidbody2D>();
        audio = stateMachine.gameObject.GetComponent<AudioSource>();
        script = stateMachine.gameObject.GetComponent<PlayerControl>();
        Assert.IsNotNull(animator);
        Assert.IsNotNull(collider);
        Assert.IsNotNull(body);
        Assert.IsNotNull(audio);
        Assert.IsNotNull(script);
        animator.SetBool("IsWalking", true);
    }

    public override void Update() {
        // Jump
        if (GameKernel.inputManager.GetKey(InputKey.Jump)) {
            stateMachine.ChangeState("Jump");
            return;
        }
        // Idle
        if (!GameKernel.inputManager.GetKey(InputKey.Left) && 
            !GameKernel.inputManager.GetKey(InputKey.Right)) {
            stateMachine.ChangeState("Idle");
            return;
        }
        // still Walk
        if (GameKernel.inputManager.GetKey(InputKey.Left)) {
            if (body.velocity.x >= -script.maxSpeed.x) {
                body.AddForce(new Vector2(-script.moveImpulse, 0.0f), ForceMode2D.Impulse);
            }
        }
        if (GameKernel.inputManager.GetKey(InputKey.Right)) {
            if (body.velocity.x <= script.maxSpeed.x) {
                body.AddForce(new Vector2(script.moveImpulse, 0.0f), ForceMode2D.Impulse);
            }
        }
        // sound effect
        deltaTime += Time.deltaTime;
        if (deltaTime >= soundDelta) {
            AudioClip clip = Resources.Load("SoundEffect/脚步/脚步-硬地") as AudioClip;
            Assert.IsNotNull(clip);
            audio.PlayOneShot(clip);
            deltaTime = 0.0f;
        }
    }

    public override void OnExit() {
        // Debug.Log("Walk OnExit");
        animator.SetBool("IsWalking", false);
    }
}

public class PlayerJumpState : State {
    Animator animator;
    Collider2D collider;
    Rigidbody2D body;
    PlayerControl script;

    /**
     *  when switch to PlayerJumpState, 
     *  this state will give Player an impulse to make Player jump,
     *  but the first time give the impulse, 
     *  Player still standing on the Ground, 
     *  cause impulse doesn't take effect that fast, 
     *  so we give a 'isJumpTakingEffect' flag, 
     *  to mark that whether Player really start jumping or not
     */
    static bool isJumpTakingEffect = false;

    public PlayerJumpState() {
        name = "Jump";
    }

    public override void OnEnter() {
        // Debug.Log("Jump OnEnter");
        animator = stateMachine.gameObject.GetComponent<Animator>();
        Assert.IsNotNull(animator);
        collider = stateMachine.gameObject.GetComponent<Collider2D>();
        Assert.IsNotNull(collider);
        body = stateMachine.gameObject.GetComponent<Rigidbody2D>();
        Assert.IsNotNull(body);
        script = stateMachine.gameObject.GetComponent<PlayerControl>();
        Assert.IsNotNull(script);
        animator.SetBool("IsJumping", true);
        body.velocity = new Vector3(0, 0, 0);
        if (!isJumpTakingEffect) {
            if (body.velocity.y <= -0.01f && !collider.IsTouchingLayers(Physics2D.AllLayers)) {
                // if falling, jump in air
                body.velocity = new Vector2(body.velocity.x, 0.0f);
                body.AddForce(new Vector2(0.0f, script.jumpInAirImpulse), ForceMode2D.Impulse);
            } else if (body.velocity.y <= script.maxSpeed.y) {
                // normal jump
                body.AddForce(new Vector2(0.0f, script.jumpImpulse), ForceMode2D.Impulse);
            }
            isJumpTakingEffect = true;
        }
    }

    public override void Update() {
        if (isJumpTakingEffect) {
            if (!collider.IsTouchingLayers(script.groundMask.value)) {
                isJumpTakingEffect = false;
            }
            return;
        }
        // Idle or Walk
        if (collider.IsTouchingLayers(script.groundMask.value)) {
            if (Math.Abs(body.velocity.x) <= 0.0f) {
                stateMachine.ChangeState("Idle");
            } else {
                stateMachine.ChangeState("Walk");
            }
            return;
        }
        // if (script.isPlayerOnGround) {
        //     if (Math.Abs(body.velocity.x) <= 0.0f) {
        //         stateMachine.ChangeState("Idle");
        //     } else {
        //         stateMachine.ChangeState("Walk");
        //     }
        //     return;
        // }
        // still Jump
        if (GameKernel.inputManager.GetKey(InputKey.Left)) {
            if (body.velocity.x >= -script.maxSpeed.x) {
                body.AddForce(new Vector2(-script.moveImpulse, 0.0f), ForceMode2D.Impulse);
            }
        }
        if (GameKernel.inputManager.GetKey(InputKey.Right)) {
            if (body.velocity.x <= script.maxSpeed.x) {
                body.AddForce(new Vector2(script.moveImpulse, 0.0f), ForceMode2D.Impulse);
            }
        }
    }

    public override void OnExit() {
        // Debug.Log("Jump OnExit");
        animator.SetBool("IsJumping", false);
    }
}
