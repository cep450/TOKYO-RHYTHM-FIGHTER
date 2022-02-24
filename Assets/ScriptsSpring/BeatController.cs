using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour
{

    public struct Accuracy {
        public Accuracy(float threshBefore, float threshAfter) {
            this.thresholdBeforeBeat = threshBefore;
            this.thresholdAfterBeat = threshAfter;
        }
        public float thresholdBeforeBeat { get; }
        public float thresholdAfterBeat { get; }
    }

    //// Beat accuracies! 
    //You might get these from BeatController functions.
    //You can check them against each other
    //ex. if(accuracyPassedIn.Equals(BeatController.PERFECT)) { do something cause it's perfect }

    //TODO: should there be different thresholds for different fractions of beats? 
    public static readonly Accuracy MINIMUM = new Accuracy(0.30f, 0.25f);
    public static readonly Accuracy GREAT = new Accuracy(0.20f, 0.15f);
    public static readonly Accuracy PERFECT = new Accuracy(0.15f, 0.10f);
    public static readonly Accuracy OFFBEAT = new Accuracy(float.NaN, float.NaN);

    
    //BPM 
    //easy to know and set. human-readable, will be used to do some conversion 
    static float BPM = 100; 


    public AudioSource audioSource;

    //will be calculated from BPM. in seconds. 
    static float secPerBeat;

    //time the song started, in unity audio time 
    static float songStartTime;

    //song position, in seconds 
    static float songPos = 0;

    //what beat the song is on ex. 1, 2, 4, 5.5, 6.75 
    //we will use a threshold with this for reaction 
    public static float beat { get; private set; }

    //how near or far we are from a beat. 
    //1 when exactly on beat (ex. 5.0) 0 when exactly between beats (ex. 5.5)
    //would look like a triangle wave when graphed 
    public static float beatOffset { get; private set; }

    bool beatEnded1, beatEnded05, beatEnded025;


    // Start is called before the first frame update
    void Start()
    {
        //convert BPM to functional value 
        secPerBeat = 60f / BPM;

        //TODO might have the song started by a button or soemthing idk. 
        //for now just starts at startup 
        StartSong();

    }

    //call when we start the song. 
    //records the time ect 
    void StartSong() {
        //kick off tracker with current time 
        songStartTime = (float)AudioSettings.dspTime;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //update all our tracker variables. 
        songPos = (float)(AudioSettings.dspTime - songStartTime);
        beat = songPos / secPerBeat;
        beatOffset = GetAbsDistanceFromBeat();
        //accuracy = GetAccuracy();


        //If we've hit major beats (1, 0.5, 0.25) send out events. 
        if(!beatEnded1 && GetDistanceFromBeat(1) > MINIMUM.thresholdAfterBeat) {
            beatEnded1 = true;
            Global.Boss.EndOfBeat1();
            Global.Player.EndOfBeat1();
        } else if(GetDistanceFromBeat(1) > 1 - MINIMUM.thresholdBeforeBeat) {
            //if we've moved on to the next beat, open this flag 
            beatEnded1 = false;
        }

        if(!beatEnded05 && GetDistanceFromBeat(0.5f) > MINIMUM.thresholdAfterBeat) {
            beatEnded05 = true;
            Global.Boss.EndOfBeat05();
            Global.Player.EndOfBeat05();
        } else if(GetDistanceFromBeat(0.5f) > 0.5 - MINIMUM.thresholdBeforeBeat) {
            //if we've moved on to the next beat, open this flag 
            beatEnded05 = false;
        }

        if(!beatEnded025 && GetDistanceFromBeat(0.25f) > MINIMUM.thresholdAfterBeat) {
            beatEnded025 = true;
            Global.Boss.EndOfBeat025(); 
            Global.Player.EndOfBeat025();
        } else if(GetDistanceFromBeat(0.25f) > 0.25 - MINIMUM.thresholdBeforeBeat) {
            //if we've moved on to the next beat, open this flag 
            beatEnded025 = false;
        }

    }


    //5.1 returns 0.1, 5.5 returns 0.5, 5.9 returns 0.9
    public static float GetDistanceFromBeat() {
        return beat % 1f;
    }

    public static float GetDistanceFromBeat(float fraction) {
        return beat % fraction;
    }

    //5.1 returns 0.1, 5.5 returns 0.5, 5.9 returns 0.1
    public static float GetAbsDistanceFromBeat() {
        float b = GetDistanceFromBeat() - 0.5f;
        b = -Mathf.Abs(b);
        return b + 0.5f;
    }
    
    public static float GetAbsDistanceFromBeat(float fraction) {
        float b = GetDistanceFromBeat(fraction) - (fraction / 2);
        b = Mathf.Abs(b);
        return b + (fraction / 2);
    }

    //for use by player actions. 
    //are we on beat, within the threshold, according to a certain fraction?
    public static bool IsOnBeat(float fraction) {
        
        float distFromBeat = GetDistanceFromBeat(fraction);

        if(distFromBeat < fraction / 2) {
            //if this is after 
            if(distFromBeat <= MINIMUM.thresholdAfterBeat) {
                return true;
            } else {
                return false;
            }
        } else {
            //if this is before 
            if(GetAbsDistanceFromBeat(fraction) <= MINIMUM.thresholdBeforeBeat) {
                return true;
            } else {
                return false;
            }
        }
    }

    //Like WaitForSeconds, but in sync with the music. 
    //USE THIS INSTEAD OF WAITFORSECONDS
    public static IEnumerator WaitForBeat(float fraction) {
        float lastDistFromBeat = GetDistanceFromBeat(fraction);
        while(lastDistFromBeat <= GetDistanceFromBeat(fraction)) {
            lastDistFromBeat = GetDistanceFromBeat(fraction);
            yield return null;
        }
    }

    //Wait for X number of a type of beat 
    //Ex. wait for 2 0.5s to go by 
    public static IEnumerator WaitForBeatsMulti(int numBeats, float fraction) {
        int counter = 0;
        float lastDistFromBeat = GetDistanceFromBeat(fraction);
        while(counter < numBeats) {
            while(lastDistFromBeat <= GetDistanceFromBeat(fraction)) {
                lastDistFromBeat = GetDistanceFromBeat(fraction);
                yield return null;
            }
            counter++;
        }
    }

    //Wait for the end of the threshold in which you can make an attack. 
    //For use with updating variables after everything has happened on a beat. 
    public static IEnumerator WaitForEndOfThreshold(float fraction) {

        //if we're not on beat, first wait until we're on beat. (we've entered the threshold.)
        if(!IsOnBeat(fraction)) {
            while(!IsOnBeat(fraction)) {
                yield return null;
            }
        }

        //if we're on beat, wait for the end of the threshold.
        while(IsOnBeat(fraction)) {
            yield return null;
        }
    }


    //get the current accuracy. returns OK, GOOD, GREAT, PERFECT according to thresholds. 
    /*
    public static Accuracy GetAccuracy() {

        float dist = getAbsDistanceFromBeat();
        if(dist < thresh_PERFECT) {
            return Accuracy.PERFECT;
        } else if(dist < thresh_GREAT) {
            return Accuracy.GREAT;
        } else if(dist < thresh_GOOD) {
            return Accuracy.GOOD;
        } else if(dist < thresh_OK) {
            return Accuracy.OK;
        } else {
            return Accuracy.OFFBEAT;
        }
        
    } */ 
    


}
