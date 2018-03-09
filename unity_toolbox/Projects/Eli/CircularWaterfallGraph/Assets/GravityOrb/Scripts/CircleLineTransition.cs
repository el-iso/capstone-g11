using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLineTransition : MonoBehaviour
{

    public bool isCircle = true;

    public int numberTestParticles = 100;
    public int testRadius = 5;

    public float transitionLength = 1;


    private Vector3 targetLineStart;
    private Vector3 targetLineEnd;

    private bool moving = false;
    private List<GameObject> points = new List<GameObject>();
    private List<Transition> transitions = new List<Transition>();
    // Use this for initialization
    void Start()
    {
        targetLineStart = new Vector3(-1 * testRadius * Mathf.PI, 0, 0);
        targetLineEnd = new Vector3(testRadius * Mathf.PI, 0, 0);


        for (int i = 0; i < numberTestParticles; i++)
        {
            
            if (isCircle)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                sphere.transform.position = new Vector3(0, testRadius, 0);
                sphere.transform.RotateAround(Vector3.zero, new Vector3(0, 0, 1), i * 360 / numberTestParticles);
                points.Add(sphere);
            }
            else
            {
                GameObject sphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                sphere2.transform.position = (((float)i / numberTestParticles) * (targetLineEnd - targetLineStart)) + targetLineStart;
                points.Add(sphere2);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && !moving)
        {
            print("Moving");
            moving = true;
            float moveStartTime = Time.time;
            float moveEndTime = moveStartTime + transitionLength;


            foreach(GameObject point in points)
            {
                if (isCircle)
                {
                    Vector3 origin = point.transform.position;
                    Vector3 dest = TransitionToLine(
                        point,
                        Vector3.zero,
                        new Vector3(0, 0, 1),
                        (float)testRadius,
                        targetLineStart,
                        targetLineEnd,
                        points[0]
                    );
                    transitions.Add(new Transition(point, origin, dest, moveStartTime, moveEndTime));
                    print(origin + " -> " + dest);
                    GameObject copy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    copy.transform.position = point.transform.position;
                    copy.transform.localScale = point.transform.localScale;
                }
                else
                {
                    Vector3 origin = point.transform.position;
                    Vector3 dest = TransitionToCircle(
                        point,
                        Vector3.zero,
                        new Vector3(0, 0, 1),
                        (float)testRadius,
                        targetLineStart,
                        targetLineEnd,
                        points[0]
                    );
                    transitions.Add(new Transition(point, origin, dest, moveStartTime, moveEndTime));
                    print(origin + " -> " + dest);
                }
                
            }
            print("Queued " + transitions.Count + " transitions.");

        }
        if (moving)
        {

            foreach(Transition transition in transitions)
            {
                if(Time.time > transition.startTime && Time.time < transition.endTime)
                {
                    float progress = (Time.time - transition.startTime) / (transition.startTime - transition.endTime);
                    Vector3 transitionLine = (transition.origin - transition.destination);
                    transition.point.transform.position = (progress * transitionLine) + transition.origin;
                }
                else if(Time.time > transition.endTime)
                {
                    transition.point.transform.position = transition.destination;
                    transition.done = true;
                }
            }
            transitions.RemoveAll(item => item.done == true);
            if(transitions.Count == 0)
            {
                moving = false;
                isCircle = !isCircle;
            }
        }
    }

    Vector3 TransitionToLine(GameObject g, Vector3 origin, Vector3 normal, float radius, Vector3 lineStart, Vector3 lineStop, GameObject start)
    {
        Vector3 pos = g.transform.position;
        float angle = Vector3.SignedAngle(origin - start.transform.position, origin - pos, normal);
        angle = Mathf.Abs(angle) == 180.0f ? -1 * Mathf.Abs(angle) : angle;

        //angle = angle + 180.0f;
        angle /= -180.0f;
        print(angle);
        return (angle * (lineStop - lineStart));
    }

    Vector3 TransitionToCircle(GameObject g, Vector3 origin, Vector3 normal, float radius, Vector3 lineStart, Vector3 lineStop, GameObject start)
    {
        Vector3 p = g.transform.position;
        Vector3 lineCenter = (0.5f * (lineStop - lineStart)) + lineStart;
        float distanceFromCenter = (p - lineCenter).magnitude;
        if (Vector3.Angle(p - lineCenter, lineStart - lineCenter) < 90.0f)
        {
            distanceFromCenter *= -1;
        }
        float radian = distanceFromCenter / (Mathf.PI * radius);
        float angle = radian * 180;
        angle -= 180;
        print(angle);
        print(start.transform.position);
        GameObject temp = new GameObject();
        temp.transform.position = Vector3.up * radius;
        temp.transform.RotateAround(origin, normal, angle);
        return temp.transform.position;
    }
}

public class Transition
{
    public GameObject point;
    public Vector3 origin;
    public Vector3 destination;
    public float startTime;
    public float endTime;
    public bool done = false;

    public Transition(GameObject p, Vector3 o, Vector3 d, float start, float end)
    {
        this.point = p;
        this.origin = o;
        this.destination = d;
        this.startTime = start;
        this.endTime = end;
    }
}

