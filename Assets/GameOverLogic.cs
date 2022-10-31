using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverLogic : MonoBehaviour
{

    [SerializeField] private TMPro.TMP_Text text;
    [SerializeField] private GameObject panel;

    void Start()
    {
        panel.SetActive(false);

        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitDead(object sender, System.EventArgs e)
    {
        if (UnitManager.Instance.GetFriendlyUnitList().Count == 0)
        {
            text.text = "You have Lost! :(";
            panel.SetActive(true);
        }
        if (UnitManager.Instance.GetEnemyUnitList().Count == 0)
        {
            text.text = "You have Won!";
            panel.SetActive(true);

        }
    }

    public void Restart()
    {
        panel.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
