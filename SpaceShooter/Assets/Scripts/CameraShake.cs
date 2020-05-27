using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;
    
    public IEnumerator Shake(float duration , float magnitude)
    {
        Debug.Log("Shake is called");
        originalPosition = transform.position;
       
        float elapsedTime = 0;

        while(elapsedTime < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(x, y, -10f);
            elapsedTime += Time.deltaTime;
            yield return 0;
        }

        transform.position = originalPosition;
        yield return null;
    }
}

