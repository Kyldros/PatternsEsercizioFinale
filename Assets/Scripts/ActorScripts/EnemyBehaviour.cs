using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] Movement movement;
    private Vector2 lastDirection;
    private bool tried;



    private void Start()
    {
        movement = GetComponent<Movement>();
        tried = true;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!movement.moved)
        {
            if (tried)
            {
                lastDirection = CasualDirection();
                tried = false;
            }
            else
            {
                tried = !movement.Move(lastDirection);
            }

            
        }
    }

    private Vector2 CasualDirection()
    {
        int rand = Random.Range(1, 5);
        if (rand == 1)
            return Vector2.up;

        if (rand == 2)
            return Vector2.down;

        if (rand == 3)
            return Vector2.left;

        if (rand == 4)
            return Vector2.right;

        return Vector2.zero;
    }
}
