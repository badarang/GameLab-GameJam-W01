using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultPlatform : MonoBehaviour
{
    public float trapTimer = 1f;
    public float motorSpeed;
    public float motorForce;

    private HingeJoint2D hinge;
    private JointMotor2D motor;

    private float resetTrapTimer = .1f;
    private float timer;
    private bool timerStart;


    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
        motor = hinge.motor;
    }

    // Update is called once per frame
    void Update()
    {
        if(timerStart)
        {
            timer -= Time.deltaTime;
            if(timer <=0)
            {
                hinge.useMotor = false;
                motor.motorSpeed = motorSpeed;
                motor.maxMotorTorque = motorForce;
                hinge.motor = motor;
                hinge.useLimits = false;
                hinge.useMotor = true;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            timerStart = true;
        }
    }


}