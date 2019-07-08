using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public float speed = 0.001f;
    public float minSpeed = 0.8f;
    public float maxSpeed = 2.0f;

    Vector3 averageHeading;
    Vector3 averagePosition;

    float neighbourDistance = 3.0f;

    Vector3 newGoalPos;

    bool turning = false;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        GetComponent<Animator>().speed = speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!turning)
        {
            newGoalPos = transform.position - other.gameObject.transform.position;
        }
        turning = true;
    }

    void OnTriggerExit(Collider other)
    {
        turning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (turning)
        {
            HandleTurn();
        }
        else
        {
            if (Random.Range(0, 10) < 1)
                ApplyRules();
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void HandleTurn()
    {
        Vector3 direction = newGoalPos - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), TurnSpeed() * Time.deltaTime);
        speed = Random.Range(minSpeed, maxSpeed);
        GetComponent<Animator>().speed = speed;
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = GlobalFlock.allFish;

        Vector3 vcenter = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 goalPos = GlobalFlock.goalPos;

        float dist;
        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            if (go != gameObject)
            {
                dist = Vector3.Distance(go.transform.position, transform.position);
                if (dist <= neighbourDistance)
                {
                    vcenter += go.transform.position;
                    groupSize++;

                    if (dist < 2.0f)
                    {
                        vavoid = vavoid + (transform.position - go.transform.position);
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed += anotherFlock.speed;
                }
            }

            if (groupSize > 0)
            {
                vcenter = vcenter / groupSize + (goalPos - transform.position);
                speed = gSpeed / groupSize;

                Vector3 direction = (vcenter + vavoid) - transform.position;
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), TurnSpeed() * Time.deltaTime);
                }
            }
        }
    }

    float TurnSpeed()
    {
        return Random.Range(0.1f, 0.5f);
    }
}
