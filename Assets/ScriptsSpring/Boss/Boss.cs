using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Class that manages boss health and AI
public class Boss : MonoBehaviour
{
    public static Boss CurrentBoss;//Declares whichever boss is the current boss, for reference with player input & such
    protected float bossHP;

    public void ChangeBossHP(int amt) {//Function to be called by others when increasing/decreasing hp
        bossHP += amt;
    }

    [SerializeField] public BossAI AttackAI;

    void Start() {
        //Going to remove this later:
        CurrentBoss = this;
    }
}
