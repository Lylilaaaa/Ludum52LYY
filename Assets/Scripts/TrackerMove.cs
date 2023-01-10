using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerMove : MonoBehaviour
{
    public Animator Anitor;
    int moveID = -1;
    private void Start()
    {
        moveID = Animator.StringToHash("canMove");
        StartCoroutine("moveAndHarvest");
    }
    IEnumerator moveAndHarvest()
    {
        Anitor.SetBool(moveID, true);
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
        }
    }
    
}
