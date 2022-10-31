using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    [SerializeField]private List<Unit> unitList;
    [SerializeField] private List<Unit> frindlyList;
    [SerializeField] private List<Unit> enemyList;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("There is more than one " + ToString() + ". Please fix!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        unitList = new List<Unit>();
        frindlyList = new List<Unit>();
        enemyList = new List<Unit>();
    }

    void Start()
    {
        Unit.OnAnyUnitSpawnd += Unit_OnAnyUnitSpawnd;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    public List<Unit> GetEnemyUnitList()
    {
        return enemyList;
    }
    public List<Unit> GetFriendlyUnitList()
    {
        return frindlyList;
    }

    private void Unit_OnAnyUnitDead(object sender, System.EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Remove(unit);
        if (unit.IsEnemy())
            enemyList.Remove(unit);
        else
            frindlyList.Remove(unit);
    }

    private void Unit_OnAnyUnitSpawnd(object sender, System.EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Add(unit);
        if(unit.IsEnemy())
            enemyList.Add(unit);
        else
            frindlyList.Add(unit); 
    }
}
