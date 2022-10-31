using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour, IAttack
{
    [SerializeField] private GameObject damageAreaPrefab;
    [SerializeField] private int damage;
    [SerializeField] private int range = 4;
    [SerializeField] private bool attackStraight = false;
    [SerializeField] private bool attackDiagonal = false;

    private Unit unit;
    private List<GridPosition> attackPattern = new List<GridPosition>();

    public void Attack()
    {
        Debug.Log("Ranged attack!!");
        var unitGridPosition = unit.GetGridPosition();

        foreach (GridPosition offsetGridPosition in attackPattern)
        {
            GridPosition testGridPos = unitGridPosition + offsetGridPosition;

            if (unitGridPosition == testGridPos) continue; //Check self position   
            if (!GameGrid.Instance.IsValidGridPosition(testGridPos)) continue; //Is position within grid

            Instantiate(damageAreaPrefab, unit.transform.position + new Vector3(offsetGridPosition.x, offsetGridPosition.y, 0), Quaternion.identity);

            var targetUnit = GameGrid.Instance.GetUnitAtGridPosition(testGridPos);
            if (targetUnit != null)
            {
                if (targetUnit.IsEnemy() == unit.IsEnemy()) continue;

                Debug.Log("Hit " + targetUnit.name);
                targetUnit.Damage(damage);
            }
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        unit = GetComponent<Unit>();
        AddAttackPattern();
    }

    private void AddAttackPattern()
    {
        for (int i = -range; i <= range; i++)
        {
            if (attackStraight)
            {
                attackPattern.Add(new GridPosition(i, 0)); //horisontal
                attackPattern.Add(new GridPosition(0, i)); //vertical
            }
            if (attackDiagonal)
            {
                attackPattern.Add(new GridPosition(i, -i)); //diagonal
                attackPattern.Add(new GridPosition(-i, -i)); //diagonal2
            }

        }
    }
}
