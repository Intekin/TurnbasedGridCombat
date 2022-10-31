using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveJump : MonoBehaviour, IMove
{
    [SerializeField] private float moveSpeed = 8;
    [SerializeField] private int moveDistance = 2;

    private Unit unit;
    private Vector3 targetPosition;
    private float stoppingDistance = 0.02f;

    void Start()
    {
        unit = GetComponent<Unit>();
        targetPosition = unit.transform.position;
    }

    void Update()
    {
        Vector3 nextPosition = targetPosition;
        Vector3 moveDirection = (nextPosition - transform.position).normalized;

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }

    public void Move()
    {
        var nextGridPos = unit.GetGridPosition();

        do
        {
            var moveVector = Mathf.FloorToInt(Random.Range(0, 8));
            nextGridPos = unit.GetGridPosition();

            switch (moveVector)
            {
                case 0:
                    nextGridPos.x += moveDistance;
                    nextGridPos.y += moveDistance;
                    break;
                case 1:
                    nextGridPos.x -= moveDistance;
                    nextGridPos.y -= moveDistance;
                    break;
                case 2:
                    nextGridPos.x -= moveDistance;
                    nextGridPos.y += moveDistance;
                    break;
                case 3:
                    nextGridPos.x += moveDistance;
                    nextGridPos.y -= moveDistance;
                    break;
                case 4:
                    nextGridPos.x += moveDistance;
                    break;
                case 5:
                    nextGridPos.x -= moveDistance;
                    break;
                case 6:
                    nextGridPos.y += moveDistance;
                    break;
                case 7:
                    nextGridPos.y -= moveDistance;
                    break;
            }
        } while (!(GameGrid.Instance.IsValidGridPosition(nextGridPos) && !GameGrid.Instance.HasExistingUnit(nextGridPos)));

        targetPosition = new Vector3(nextGridPos.x, nextGridPos.y, 0);
    }
}
