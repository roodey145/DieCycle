using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest1Controller : MonoBehaviour
{
    private int _moneyRequestedAmount = 300;

    // Update is called once per frame
    void Update()
    {
        if(PlayerInfo.money >= _moneyRequestedAmount)
        {
            Tutorial.isQuestFinished = true;
            gameObject.SetActive(false);
        }
    }
}
