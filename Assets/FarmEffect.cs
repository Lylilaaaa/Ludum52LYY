using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmEffect : MonoBehaviour
{
    public float rain_duration;
    public GameObject effect_rain;

    public float thunder_duration;
    public GameObject effect_thunder;

    public float dry_duration;
    public GameObject effect_dry;


    public void Rain()
    {
        StartCoroutine(RainCoroutine());
    }

    IEnumerator RainCoroutine()
    {
        GameObject new_effect = Instantiate(effect_rain, transform);
        yield return new WaitForSeconds(rain_duration);
        Destroy(new_effect);
    }

    public void Thunder()
    {
        StartCoroutine(ThunderCoroutine());
    }

    IEnumerator ThunderCoroutine()
    {
        GameObject new_effect = Instantiate(effect_thunder, transform);
        yield return new WaitForSeconds(thunder_duration);
        Destroy(new_effect);
    }

    public void Dry()
    {
        StartCoroutine(DryCoroutine());
    }

    IEnumerator DryCoroutine()
    {
        GameObject new_effect = Instantiate(effect_dry, transform);
        yield return new WaitForSeconds(dry_duration);
        Destroy(new_effect);
    }

}
