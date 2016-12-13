﻿using UnityEngine;
using UnityEngine.Assertions;
using System;

public class BoxPuzzle : Puzzle
{
    private Vector2 maxSpeed;
    private float moveImpulse = 0.2f;
    public Transform player;
    private Rigidbody2D body; 
    void Start()
    {
        Assert.IsNotNull(player);
        gameObject.GetComponent<Rigidbody2D>().mass = 100;
        maxSpeed = player.GetComponent<PlayerControl>().maxSpeed;
        moveImpulse = player.GetComponent<PlayerControl>().moveImpulse;
        body = gameObject.GetComponent<Rigidbody2D>();
        //gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public override void Update()
    {
        base.Update();
        if (GetTriggerDown())
        {
            //gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        }
        if (GetTrigger())
        {
            if (Math.Abs(body.velocity.x) <= maxSpeed.x)
            {
                if (GameKernel.inputManager.GetKey(InputKey.Left))
                {
                    body.AddForce(new Vector2(-100*moveImpulse, 0.0f), ForceMode2D.Impulse);
                }
                if (GameKernel.inputManager.GetKey(InputKey.Right))
                {
                    body.AddForce(new Vector2(100*moveImpulse, 0.0f), ForceMode2D.Impulse);
                }
            }   
        }
        if (GetTriggerUp())
        {
            //gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            transform.rotation = new Quaternion(0,0,0,0);//排除移动箱子以后不能左右跳跃的bug
        }
    }
}
