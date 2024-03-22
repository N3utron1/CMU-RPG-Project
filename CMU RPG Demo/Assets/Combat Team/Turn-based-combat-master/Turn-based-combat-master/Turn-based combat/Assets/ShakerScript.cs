using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakerScript : MonoBehaviour
{
    public int shakeAmount;
    private bool shaking = false;
    public Vector3 originalPos;

    // Update is called once per frame
    void Update(){
        if(shaking){
            Vector3 newPos = originalPos + Random.insideUnitSphere * (Time.deltaTime * shakeAmount);
		    newPos.z = transform.position.z;
            transform.position = newPos;
        }
    }

    public void Shake(){
        StartCoroutine("Shaker");
    }

    IEnumerator Shaker(){
       originalPos = transform.position;

        if(!shaking){
            shaking = true;
        }
        
        yield return new WaitForSeconds(0.25f);
        shaking = false;
        transform.position = originalPos;
    }
}
