﻿using UnityEngine;
using System;
using System.Collections;

public class PlayerControl : MonoBehaviour {
    private Rigidbody2D body;

    public float maxSpeed = 3.0f;
    public float moveImpulse = 0.2f;
    public float jumpImpulse = 5.0f;

	void Start () {
        body = (Rigidbody2D)gameObject.GetComponent<Rigidbody2D>();
    }
	
	void Update () {
        DoMove();
        DoAction();
	}

    void DoMove() {
        if (Math.Abs(body.velocity.x) <= maxSpeed) {
            if (GameKernel.inputManager.GetKey(InputKey.Left)) {
                body.AddForce(new Vector2(-moveImpulse, 0.0f), ForceMode2D.Impulse);
            }
            if (GameKernel.inputManager.GetKey(InputKey.Right)) {
                body.AddForce(new Vector2(moveImpulse, 0.0f), ForceMode2D.Impulse);
            }
        }
    }

    void DoAction() {
        if (GameKernel.inputManager.GetKeyDown(InputKey.Jump) && body.velocity.y == 0) {
            body.AddForce(new Vector2(0.0f, jumpImpulse), ForceMode2D.Impulse);
        }
    }
}