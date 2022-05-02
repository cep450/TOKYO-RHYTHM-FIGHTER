using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_StartGameplay : Stage
{

    //Start the music.
    //Give the player a few beats to let the song start.
    //Then, kick off gameplay.

    [SerializeField] SongData songToStart;

    public float introBeats = 16;

    bool checkNextStage = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(checkNextStage && BeatController.GetBeat() >= introBeats) {
            Global.TutorialManager.NextStage();
            checkNextStage = false;
        }
    }

    public override void OnStageStart() {
        
        //boss enabled.
        Global.Boss.GetComponent<SpriteRenderer>().enabled = true;

        //boss lerps in.
        //TODO

        introBeats = introBeats + Mathf.Ceil(BeatController.GetBeat());
        Debug.Log("current beat: " + BeatController.GetBeat() + " attacks will start on: " + introBeats);

        //switch the music.
        BeatController.StartSong(songToStart);

        checkNextStage = true;

    }

    public override void OnStageEnd() {

    }
}
