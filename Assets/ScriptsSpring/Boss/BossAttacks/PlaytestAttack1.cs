using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaytestAttack1 : BossAttack
{
    public SpriteRenderer mySprite;
    public override IEnumerator Attack() {
        mySprite.color = Color.red;
        yield return BeatController.Instance.StartCoroutine(BeatController.WaitForBeat(1));
        mySprite.color = Color.white;
        yield return null;
        mySprite.color = Color.black;
    }   

    public override void Interrupt() {

    }

    public override IEnumerator Cancel()
    {
        yield return null;
    }
}