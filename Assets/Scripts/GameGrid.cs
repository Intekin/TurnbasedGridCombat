using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static GameGrid Instance { get; private set; }
    public event EventHandler OnAnyUnitMovedGridPosition;

    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private Vector2 cellSize;

    public bool showGridGizmo;

    private GridSystem<GridObject> gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more than one LevelGrid! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        gridSystem = new GridSystem<GridObject>(gridSize.x, gridSize.y, 1f,
            (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
    }

    public void AddUnitAtGridPosition(GridPosition gp, Unit unit)
    {
        gridSystem.GetGridObject(gp).AddUnit(unit);
    }

    public void RemoveUnitAtGridPosition(GridPosition gp, Unit unit)
    {
        gridSystem.GetGridObject(gp).RemoveUnit(unit);
    }
    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPos, GridPosition toGridPos)
    {
        RemoveUnitAtGridPosition(fromGridPos, unit);
        AddUnitAtGridPosition(toGridPos, unit);

        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);

    }

    public GridPosition GetGridPosition(Vector3 worldPos) => gridSystem.GetGridPosition(worldPos);
    public Vector3 GetWorldPosition(GridPosition worldPos) => gridSystem.GetWorldPos(worldPos);
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.isValidGridPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();
    public int GetHeight() => gridSystem.GetHeight();

    public bool HasExistingUnit(GridPosition gridPosition)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        if (gridObject == null) return false;
        return gridObject.HasUnit();
    }
    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

}
