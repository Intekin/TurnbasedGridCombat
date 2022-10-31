using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float stoppingDistance = 0.01f;
    public float moveSpeed = 4f;

    private Unit unit;
    private Vector2 moveVector;
    private Vector3 targetPosition;
    private bool inAttackMode = false;

    [SerializeField] GameObject damageAreaPrefab = null;

    [SerializeField] int swordAttackRange = 1;
    [SerializeField] int swordAttackDamage = 1;
    [SerializeField] int rangedAttackRange = 4;
    [SerializeField] int rangedAttackDamage = 4;

    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponent<Unit>();
        targetPosition = unit.transform.position;

        InputManager.Controls.Player.Move.performed += ctx => Move_performed(ctx);
        InputManager.Controls.Player.Move.canceled += Move_canceld;
        InputManager.Controls.Player.Attack.performed += Attack_performed;
        InputManager.Controls.Player.Attack.canceled += Attack_Executed;
    }

    private void Move_canceld(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        moveVector = Vector2.zero;
    }

    private void Attack_Executed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!TurnSystem.Instance.IsPlayerTurn()) return;

        if (moveVector.magnitude > 0.5f) //RangeAttack
        {
            Debug.Log("Ranged Attack " + moveVector.magnitude + " : " + moveVector);

            if (moveVector.x < 0)
            {
                AttackArea(unit.GetGridPosition(), -rangedAttackRange,0,0,0,rangedAttackDamage);
            }
            else if (moveVector.x > 0)
            {
                AttackArea(unit.GetGridPosition(), 0, rangedAttackRange, 0, 0, rangedAttackDamage);
            }
            else if (moveVector.y < 0)
            {
                AttackArea(unit.GetGridPosition(), 0, 0, -rangedAttackRange, 0, rangedAttackDamage);
            }
            else if (moveVector.y > 0)
            {
                AttackArea(unit.GetGridPosition(), 0, 0, 0, rangedAttackRange, rangedAttackDamage);
            }

        }
        else //SwordAttack
        {
            Debug.Log("Sword Attack");

            AttackArea(unit.GetGridPosition(), -swordAttackRange, swordAttackRange, -swordAttackRange, swordAttackRange, swordAttackDamage);
        }

        inAttackMode = false;
        TurnSystem.Instance.NextTurn();
    }

    private void AttackArea(GridPosition unitGridPosition, int minX, int maxX, int minY, int maxY, int damage)
    {
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, y);
                GridPosition testGridPos = unitGridPosition + offsetGridPosition;

                if (unitGridPosition == testGridPos) continue; //Check self position   
                if (!GameGrid.Instance.IsValidGridPosition(testGridPos)) continue; //Is position within grid

                Instantiate(damageAreaPrefab, unit.transform.position + new Vector3(x, y, 0), Quaternion.identity);

                var targetUnit = GameGrid.Instance.GetUnitAtGridPosition(testGridPos);
                if (targetUnit != null)
                {
                    Debug.Log("Hit " + targetUnit.name);
                    targetUnit.Damage(damage);
                }
            }
        }
    }

    private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!TurnSystem.Instance.IsPlayerTurn()) return;
        inAttackMode = true;
    }

    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        if (!TurnSystem.Instance.IsPlayerTurn()) return;

        moveVector = ctx.ReadValue<Vector2>();

        if (inAttackMode) return;

        var nextGridPos = unit.GetGridPosition();
        if (moveVector.x > 0) nextGridPos.x += 1;
        else if (moveVector.x < 0) nextGridPos.x -= 1;
        else if (moveVector.y > 0) nextGridPos.y += 1;
        else if (moveVector.y < 0) nextGridPos.y -= 1;

        if (GameGrid.Instance.IsValidGridPosition(nextGridPos) && !GameGrid.Instance.HasExistingUnit(nextGridPos))
        {
            targetPosition = new Vector3(nextGridPos.x, nextGridPos.y, 0);
            TurnSystem.Instance.NextTurn();
        }
    }

    private void Update()
    {
        Vector3 nextPosition = targetPosition;
        Vector3 moveDirection = (nextPosition - transform.position).normalized;

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }
}
