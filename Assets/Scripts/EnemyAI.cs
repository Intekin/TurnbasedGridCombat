using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Waiting,
        TakingTurn,
    }
    private State state;
    private float timer;

    [SerializeField][Range(0,1)] private float moveAttackRatio = 0.5f;

    private void Awake()
    {
        state = State.Waiting;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnEnded += TurnSystem_OnTurnEnded;
    }

    void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (state)
        {
            case State.Waiting:
                break;
            case State.TakingTurn:
                EnemyTurn();
                break;
        }
    }

    private void EnemyTurn()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            EnemyAIAction();
            TurnSystem.Instance.NextTurn();
            state = State.Waiting;
        }
    }

    private void EnemyAIAction()
    {
        foreach (var enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            bool walk = Random.Range(0f,1f) < moveAttackRatio? true: false;

            if (walk)
            {
                enemyUnit.GetComponent<IMove>().Move();
            }
            else
            {
                enemyUnit.GetComponent<IAttack>().Attack();
            }
        }
    }

    #region Events
    private void TurnSystem_OnTurnEnded(object sender, System.EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 0.5f;
        }
    }
    #endregion

}
