using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    bool gameEndsPointThreshold = true;


    public Character otherPlayer;

    public Controls controls;

    public PlayerSoundEffectController sfxController;
    public CharacterSpriteController spriteController;
    public SpriteSplash spriteSplashController;


    static int pointsToGame = 3;
    public int points = 0;

    public int hitsScored = 0;



    //// POINT VALUES ////
    int pval_block = 1;
    int pval_hit = 1;

    float pmult_OK = 0.5f;
    float pmult_GOOD = 0.75f;
    float pmult_GREAT = 1f;
    float pmult_PERFECT = 1.25f;


    bool iHit = false;
    public bool wasBlocked = false;
    public bool messedUp = false;

    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //when the player tries to take an action off the beat. 
    public void messUp() {

        messedUp = true;

        //TODO splash
    }

    //we can hit or block an attack if the other player is 2 units in front of us. 
    public bool otherInProximity() {
        return (controls.position == (-otherPlayer.controls.position + (otherPlayer.controls.posMax - 2)));
    }

    //whenever we do high/low, check if we've blocked someone. 
    public void checkBlock() {

        //we're in the right place to block an attack if the other player is 2 units in front of us
        if(otherInProximity()) {
            
            //and we've blocked if our high/low matches their high/low-forward
            if(controls.colliderHigh && otherPlayer.controls.colliderHighForward) {
                block(true);
            } else if(controls.colliderLow && otherPlayer.controls.colliderLowForward) {
                block(false);
            }
            //TODO else- whiff- play a sound? test if we need this 

        }

    }

    //whenever we do high/low-forward, check if we've hit someone. 
    public void checkHit() {

        //we're in the right place to hit if the other player is 2 units in front of us 
        if(otherInProximity()) {

            //and we've hit if our high/low-forward isn't met with another 
            if(controls.colliderHighForward && !otherPlayer.controls.colliderHigh) {
                hit(true);
            } else if(controls.colliderLowForward && !otherPlayer.controls.colliderLow) {
                hit(false);
            }
            //but if we ARE met, WE'VE been blocked 
            else if(controls.colliderHighForward && otherPlayer.controls.colliderHigh){
                otherPlayer.block(true);
            } else if(controls.colliderLowForward && otherPlayer.controls.colliderLow) {
                otherPlayer.block(false);
            }

        }
    }

    //we blocked the other player's attack. swords clashed. 
    //initiates a riposte. TODO 
    public void block(bool wasHigh) {

        otherPlayer.wasBlocked = true; 

        //TODO rack up points depending on accuracy 
        BeatController.Accuracy accuracy = BeatController.GetAccuracy();

        //display a splash depending on accuracy 
        spriteSplashController.showSplash(wasHigh, accuracy);

        riposte();

    }

    //this player gets a riposte. 
    //this could be called either by this player if they block, or by the other player if they were blocked by this player 
    //this player can attack on the offbeat. 
    //TODO
    public void riposte() {

    }

    //high/low forward collides with body. 
    //this could get overwritten by a block, so it won't score until the end of the beat threshold. 
    public void hit(bool wasHigh) {

        iHit = true;

        otherPlayer.spriteController.flashRed();

        //TODO rack up points depending on accuracy 
        BeatController.Accuracy accuracy = BeatController.GetAccuracy();

        //display a splash depending on accuracy 
        spriteSplashController.showSplash(wasHigh, accuracy);
    }

    //called every time the beat threshold is over. 
    //adds hits scored, resets variables. 
    public void endOfBeat() {

        //tally up any hits that weren't blocked this beat. 
        if(iHit && !wasBlocked) {
            //i hit and wasn't blocked. score a point 
            hitsScored++;
            if(gameEndsPointThreshold && hitsScored == pointsToGame) {
                //WE HAVE A WINNER 
                wonTheGame();
            }
        }

        //reset variables and flags. 
        iHit = false;
        wasBlocked = false;
        controls.actedThisBeat = false;

        //cont- if we were forward, reset to blocking 
        if(controls.colliderHighForward) {
            controls.colliderHighForward = false;
        }
        if(controls.colliderLowForward) {
            controls.colliderLowForward = false;
        }

        //cont- return to baseline if messed up 
        if(messedUp) {
            messedUp = false;
            spriteController.idle();
        }


    }


    void wonTheGame() {
        //TODO 
    }




}
