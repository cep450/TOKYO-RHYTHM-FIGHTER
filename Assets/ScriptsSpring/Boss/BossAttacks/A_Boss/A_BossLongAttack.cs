using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//A longer attack the boss performs over the course of 4 beats
public class A_BossLongAttack : BossAttack
{
    [SerializeField] private SpriteRenderer mySprite;
    [SerializeField] private float damageToDeal;

    public override IEnumerator Attack() {
        mySprite.color = Color.red;
        yield return StartCoroutine(BeatController.WaitForBeatsMulti(4, 1));
        //Checks to see if they can hit the player - if they do, the player gets hit
        Global.Boss.makeAttackThisBeat = true;
        yield return null;
        mySprite.color = Color.black;
    }   

    public override void Interrupt(PlayerAction action) {
        //This attack is cancelled if hit twice before 
    }

//When cancelled, this waits for the next beat
    public override IEnumerator Cancel()
    {
        yield return StartCoroutine(BeatController.WaitForBeat(1));
    }
    
    //Waits until the end of threshold, then checks to see if the attack is successful
    public override void CheckAttackSuccess()
    {

        if(Global.Player.CurrentAction == null || Global.Player.CurrentAction == Global.Player.messUpAction) {
            Global.Player.ChangeHP(-damageToDeal);
        }
        //CheckPlayerCurrentAction;
        //If player action is on beat, this attack is blocked and does nothing

        //else { Global.PlayerInstance.ChangeHP(-damage);}
    }
}
