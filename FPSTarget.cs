using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 
/// This script is attached to all the target GameObjects.
/// It is responsible for defining the target's behavior
/// before and after it is hit, as well as how many points
/// the target is worth.
/// 
public class FPSTarget : MonoBehaviour
{
    public int points;
    private Vector3 velocity = Vector3.zero;
    private Rigidbody rb;
    private Vector3 start;
    public bool isHit = false;

    private Dictionary<string, Action> targetActions; 

    private readonly Dictionary<string, int> targetPoints = new()
    {
            {"target_1", 1 },
            {"target_2", 3 },
            {"target_3", 5 },
            {"target_4", 10 },
            {"target_5", 20 }
    };

    /// 
    /// Defining some variables and setting the actions
    /// for each target, so we don't have to write a lot of
    /// if statements in the Update method.
    /// 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        points = targetPoints[tag];
        start = transform.position;
        // Define the target actions
        targetActions = new(){
            {"target_1", () => {}},
            {"target_2", () => {
                gameObject.transform.localScale = new Vector3(
                    Mathf.Abs(Mathf.Sin(Time.time)),
                    Mathf.Abs(Mathf.Sin(Time.time)),
                    Mathf.Abs(Mathf.Sin(Time.time))
                );
            } },
            {"target_3", () => {
                transform.SetPositionAndRotation(
                    start + new Vector3(0.75f * Mathf.Sin(Time.time), 0, 0),
                    Quaternion.Euler(45 * Mathf.Sin(Time.time), 45 * Mathf.Sin(Time.time), 45 * Mathf.Sin(Time.time))
                );
            } },
            {"target_4", () => {
                transform.SetPositionAndRotation(
                    start + new Vector3(0.5f * Mathf.Sin(Time.time), 0.5f * Mathf.Sin(Time.time), 0),
                    Quaternion.Euler(45 * Mathf.Sin(Time.time), 45 * Mathf.Sin(Time.time), 45 * Mathf.Sin(Time.time)));
            } },
            {"target_5", () => {
                StartCoroutine(TopLevelMovement(rb));
                gameObject.transform.localScale = new Vector3(
                1.0f + Mathf.Abs(Mathf.Sin(Time.time)),
                1.0f + Mathf.Abs(Mathf.Sin(Time.time)),
                1.0f + Mathf.Abs(Mathf.Sin(Time.time))
            );
            } }
        };
    }

    /// 
    /// All the target actions are defined in the Start method.
    /// Besides that, we only need to check if the target was hit.
    /// 
    void Update()
    {
        if (!isHit)
        {
            targetActions[tag]();
        }
    }

    /// 
    /// Just a coroutine that cleans up the target GameObject.
    /// 
    private IEnumerator Die()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    /// 
    /// This method is called by the RayShooter script 
    /// when the target is hit.
    /// Note that the target is not destroyed immediately.
    /// Instead, it plays a sound and starts the Die coroutine.
    /// 
    public void ReactToHit()
    {
        GetComponent<AudioSource>().Play();
        StartCoroutine(Die());
    }

    /// 
    /// This is a special coroutine that makes the
    /// top level target much more difficult to hit.
    /// 
    private IEnumerator TopLevelMovement(Rigidbody rb)
    {

        while (true)
        {
            if (rb.velocity.magnitude < 1) {
                rb.velocity = new Vector3(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(1, 2), 0);
            } 
            yield return new WaitForSeconds(1);
        }
    }

}
