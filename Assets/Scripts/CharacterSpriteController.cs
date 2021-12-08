using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteController : MonoBehaviour
{

    public Sprite spr_Idle;
    public Sprite spr_High;
    public Sprite spr_HighForward;
    public Sprite spr_Low;
    public Sprite spr_LowForward;
    public Sprite spr_MessUp;


    public IEnumerator coroutine;
    
    
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void idle() {
        spriteRenderer.sprite = spr_Idle;
    }

    public void high() {
        spriteRenderer.sprite = spr_High;
    }

    public void low() {
        spriteRenderer.sprite = spr_Low;
    }

    public void highForward() {
        spriteRenderer.sprite = spr_HighForward;
    }

    public void lowForward() {
        spriteRenderer.sprite = spr_LowForward;
    }

    public void messUp() {
        spriteRenderer.sprite = spr_MessUp;
    }

    public void flashColor(bool isP1) {
        if(isP1) {
            coroutine = flashColorRoutine(Color.red);
            StartCoroutine(coroutine);
            
        } else {
            coroutine = flashColorRoutine(Color.blue);
            StartCoroutine(coroutine);
        }
        
    }

    IEnumerator flashColorRoutine(Color colorFlash) {
        spriteRenderer.color = colorFlash;
        float ttracker = 0;
        while(spriteRenderer.color != Color.white) {
            spriteRenderer.color = Color.Lerp(colorFlash, Color.white, ttracker);
            ttracker += (Time.deltaTime * 1.33f);
            yield return null;
        }
        
    }


}
