﻿using System.Collections;
using UnityEngine;

/// <summary>
/// Attach this to any object that will create lighting that the AI or player should be aware of.
/// </summary>
public class FakeLighting : MonoBehaviour
{
    [SerializeField]
    [Range(0,1)]
    float startAlpha; //The alpha can be changed in the inspector after run time. But this will be set to the sprites alpha value at Start.
    [SerializeField]
    [Range(0, 1)]
    float flickerAlphaDifference; //The range of difference the alpha will flicker to.
    [SerializeField]
    [Range(0, 1)]
    float flickerSpeed; //The frequency the alpha flickers.
    [SerializeField]
    [Range(0, 1)]
    float flickerSpeedVariation; //The range of variation on the flicker speed.

    [SerializeField]
    Vector2 startScale; //The starting scale of the object. This can be changed after runtime in the inspector but is set at start to equal the localscale of the object.
    [SerializeField]
    [Range(0, 1)]
    float scaleSizeDifference; //The possible variance in scale when changed from the startScale.
    [SerializeField]
    [Range(0, 1)]
    float scaleSizeSpeed; //The time between changing sizes.
    [SerializeField]
    [Range(0, 1)]
    float sizeScaleSpeedVariation; //The variance ontop of scaleSizeSpeed to change between changing size.

    [SerializeField]
    bool independentXYGrow; //Allows the gameobject's X and Y to scale independantly creating oblong lighting.
    [SerializeField]
    bool sync; //Syncs the growing and flickering effect.

    SpriteRenderer sr;
    CapsuleCollider2D cc2d;

    private void Start()
    {
        startScale = transform.localScale;
        sr = GetComponent<SpriteRenderer>();
        cc2d = GetComponent<CapsuleCollider2D>();

        StartCoroutine(Flicker());
        StartCoroutine(ScaleLight());
    }

    /// <summary>
    /// Flickers the light based on the classes variables.
    /// </summary>
    /// <returns>Wait for flickertime</returns>
    IEnumerator Flicker()
    {  
        float flickerAmount = Mathf.Clamp(Random.Range(-flickerAlphaDifference, flickerAlphaDifference) + startAlpha, 0, 1);
        sr.color = new Color(sr.color[0], sr.color[1], sr.color[2], flickerAmount);

        Mathf.Clamp(Random.Range(-flickerSpeedVariation, flickerSpeedVariation) + flickerSpeed,0,5);
        float flickerTime = Random.Range(-flickerSpeedVariation, flickerSpeedVariation) + flickerSpeed;
        yield return new WaitForSeconds(flickerTime);
        StartCoroutine(Flicker());
        
        if (sync)
        {
            StartCoroutine(ScaleLight());
        }
    }
    /// <summary>
    /// Scales the light based on the classes variables.
    /// </summary>
    /// <returns>Wait for scaletime unless Synced</returns>
    IEnumerator ScaleLight()
    {
        float x = Mathf.Clamp(Random.Range(-scaleSizeDifference, scaleSizeDifference) + startScale.x, .1f, Mathf.Infinity);
        float y = Mathf.Clamp(Random.Range(-scaleSizeDifference, scaleSizeDifference) + startScale.y, .1f, Mathf.Infinity);

        if (x > y)
        {
            cc2d.direction = CapsuleDirection2D.Horizontal;
        }
        else
        {
            cc2d.direction = CapsuleDirection2D.Vertical;
        }
        
        if (independentXYGrow)
        {
            transform.localScale = new Vector2(x, y);
        }
        else
        {
            transform.localScale = new Vector2(x,x);
        }
 
        if (!sync)
        {
            float scaleTime = Mathf.Clamp(Random.Range(-sizeScaleSpeedVariation, sizeScaleSpeedVariation) + scaleSizeSpeed, .01f, Mathf.Infinity);
            yield return new WaitForSeconds(scaleTime);
            StartCoroutine(ScaleLight());
        }
    }

    /// <summary>
    /// if the collision object is in the trigger collider and if the colliding object has the fakelighting detection script, set it to visible.
    /// </summary>
    /// <param name="collision">The colliding object</param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<FakeLightingDetection>())
        {
            collision.GetComponent<FakeLightingDetection>().visible = true;
        }
    }
    
    /// <summary>
    /// On collision exit, if the colliding object has the fakelighting detection script, set visible to false.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<FakeLightingDetection>())
        {
            collision.GetComponent<FakeLightingDetection>().visible = false;
        }
    }
}
