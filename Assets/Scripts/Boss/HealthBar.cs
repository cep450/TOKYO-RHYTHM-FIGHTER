using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image healthBarLeft;
    [SerializeField] Image healthBarRight;


    [SerializeField] Image healthBarLeftYell;
    [SerializeField] Image healthBarRightYell;
    //Between 0,1 and this decides the rate at which health is visually decreased from the health bar (the higher the decimal, the faster it changes)
    [SerializeField] float interpolationSpeed;
    ShakeRect shaker;
    private float shakeMagnitude = 6f;
    private float shakeTimeMultiplier = -0.25f;



    // Start is called before the first frame update
    void Start()
    {
        shaker = GetComponent<ShakeRect>();
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        ////Testing function
        if (Input.GetKey(KeyCode.Space))
        {
           ChangeHealth(5);
        }
        */
    }

    public void ChangeHealth(float healthDifference)
    {
        // /*
        StopCoroutine(HealthBarChange());
        StartCoroutine(HealthBarChange());
       // */

        StopCoroutine(HealthBarChangeYellow(healthDifference));
        StartCoroutine(HealthBarChangeYellow(healthDifference));
        shaker.ShakeIt(healthDifference * shakeMagnitude, healthDifference * shakeTimeMultiplier);
    }


    public IEnumerator HealthBarChange()
    {
        
        for (float t = 0f; t <= 2f; t += Time.deltaTime)
        {
            
            healthBarLeft.fillAmount = Mathf.Lerp(healthBarLeft.fillAmount, (((float)Global.Boss.bossHP) / ((float)Global.Boss.currentStageStartingHP)), t );
            healthBarRight.fillAmount = Mathf.Lerp(healthBarRight.fillAmount, (((float)Global.Boss.bossHP) / ((float)Global.Boss.currentStageStartingHP)), t );


            yield return null;
        }
    }

    public IEnumerator HealthBarChangeYellow(float amount)
    {
        for (float t = 0f; t <= 3f; t += Time.deltaTime)
        {

            healthBarLeftYell.fillAmount = Mathf.Lerp(healthBarLeftYell.fillAmount, healthBarLeft.fillAmount, t * interpolationSpeed);
            healthBarRightYell.fillAmount = Mathf.Lerp(healthBarRightYell.fillAmount, healthBarRight.fillAmount, t * interpolationSpeed);


            yield return null;
        }
    }
}
