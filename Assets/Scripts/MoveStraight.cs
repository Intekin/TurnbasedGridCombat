using UnityEngine;

public class MoveStraight : MonoBehaviour, IMove
{
    [SerializeField] private float moveSpeed = 4;

    private Unit unit;
    private Vector3 targetPosition;
    private float stoppingDistance = 0.01f;

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
            var moveVector = Mathf.FloorToInt(Random.Range(0, 4));
            nextGridPos = unit.GetGridPosition();

            switch (moveVector)
            {
                case 0:
                    nextGridPos.x += 1;
                    break;
                case 1:
                    nextGridPos.x -= 1;
                    break;
                case 2:
                    nextGridPos.y += 1;
                    break;
                case 3:
                    nextGridPos.y -= 1;
                    break;
            }
        } while (!(GameGrid.Instance.IsValidGridPosition(nextGridPos) && !GameGrid.Instance.HasExistingUnit(nextGridPos)));

            targetPosition = new Vector3(nextGridPos.x, nextGridPos.y, 0);
    }
}
