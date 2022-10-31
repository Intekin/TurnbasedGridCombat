using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static event EventHandler OnAnyUnitSpawnd;
    public static event EventHandler OnAnyUnitDead;

    [SerializeField] bool isEnemy;

    private GridPosition gridPosition;
    private HealthSystem healthSystem;

    public Vector2 m_GridPosition;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        gridPosition = GameGrid.Instance.GetGridPosition(transform.position);
        GameGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        healthSystem.OnDeath += HealthSystem_OnDeath;

        OnAnyUnitSpawnd?.Invoke(this, EventArgs.Empty);
    }

    void Update()
    {
        var gp = GameGrid.Instance.GetGridPosition(transform.position);

        m_GridPosition = new Vector2(gp.x, gp.y);

        var newGridPosition = GameGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPos = gridPosition;
            gridPosition = newGridPosition;
            GameGrid.Instance.UnitMovedGridPosition(this, oldGridPos, newGridPosition);
        }
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public bool IsEnemy() => isEnemy;

    public void Damage(int damageAmount)
    {
        healthSystem.TakeDamage(damageAmount);
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        GameGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);

        healthSystem.OnDeath -= HealthSystem_OnDeath;
        Destroy(gameObject);
    }
}
