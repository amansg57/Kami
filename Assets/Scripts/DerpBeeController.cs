using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DerpBeeController : AirUnitController
{
    private float initX;
    private float initY;
    private float deltaX = 0;

    [SerializeField]
    private float xMult, yMult, sinSpeedMult;

    private void Start() 
    {
        initX = transform.position.x;
        initY = transform.position.y;
    }

    private void FixedUpdate()
    {
        deltaX += Time.deltaTime;
        rb2d.MovePosition(new Vector2(initX - deltaX * xMult, initY + Mathf.Sin(deltaX * sinSpeedMult) * yMult));
    }
}
