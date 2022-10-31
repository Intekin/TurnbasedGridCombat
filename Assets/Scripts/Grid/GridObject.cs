using System.Collections.Generic;

public class GridObject
{
    private GridPosition gridPosition;
    private GridSystem<GridObject> gridSystem;
    private List<Unit> unitList;

    public GridObject(GridSystem<GridObject> gs, GridPosition gp)
    {
        gridPosition = gp;
        gridSystem = gs;
        unitList = new List<Unit>();
    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public bool HasUnit()
    {
        return unitList.Count > 0;
    }
    public Unit GetUnit(int unitIndex = 0)
    {
        if (HasUnit())
        {
            return unitList[unitIndex];
        }
        return null;
    }
}
