using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour {

    public GameObject container;
    public Rigidbody particle;
    private Box box;
    public uint n = 1000;
    public float size = 1;
    public float speed = 10;
    private Rigidbody[] particles;

    private static System.Random r = new System.Random();

    public int maxParticles(Vector3 dims)
    {
        int result = 1;
        for(int i = 0; i < 3; i++)
        {
            if (dims[i] < size) { return 0; }
            else
            {
                result *= (int)((dims[i] - (size / 2.0f)) / size);
            }
        }
        return result;
    }

    private Vector3 GetOrigin(Vector3 dims)
    {
        Vector3 result = new Vector3();
        for(int i = 0; i < 3; i++)
        {
            result[i] = -1 * (dims[i] - size) / 2;
        }
        return result;
    }

    public void packBox(uint n, Vector3 dims, Vector3 origin)
    {
        int x, y, z;
        for (int i = 0; i < n; i++)
        {
            x = i % (int)(dims.x - size);
            y = (i / (int)(dims.x - size)) % (int)(dims.y - size);
            z = (i / (int)((dims.x - size) * (dims.y - size)) % (int)(dims.z - size));
            Rigidbody p = Instantiate(particle, origin + new Vector3(x, y, z), Quaternion.identity);
            p.transform.localScale *= size;
            p.transform.SetParent(this.transform);
            p.velocity = randomVelocity() * speed;

            particles[i] = p;
        }

    }

    private int randomSign()
    {
        return r.Next(2) == 0 ? 1 : -1;
    }

    private Vector3 randomVelocity()
    {
        Vector3 result = new Vector3();
        for(int i = 0; i < 3; i++)
        {
            result[i] = randomSign() * (float)r.NextDouble();
        }
        return result;
    }

	// Use this for initialization
	void Start () {
        particles = new Rigidbody[n];
        box = container.GetComponent<Box>();
        Vector3 dims = box.dimensions;
        Vector3 origin = GetOrigin(dims);
        if (n > maxParticles(dims)) { throw new System.ArgumentException(n+">"+maxParticles(dims)+"\tToo many particles"); return; }
        else
        {
            packBox(n, dims, origin);
            //print((randomVelocity()*speed).ToString());
            //print(GetOrigin(dims).ToString());
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
