using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Class that manages boss health and AI
public class Boss : MonoBehaviour
{
    protected float bossHP;//Design question: should this be an integer?
    public void ChangeBossHP(int amt) {//Function to be called by others when increasing/decreasing hp
        bossHP += amt;
    }

    BossAI AttackAI;
}
