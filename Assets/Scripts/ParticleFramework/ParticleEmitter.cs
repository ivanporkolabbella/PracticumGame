using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleType
{
    Cube, Sphere, Billboard
}

//emit particles (create them, destroy them... pooling system)
//initialize particles with initial values
//update particles...
//Particle emitter to behave as a particle
//different emition modes - constatnt rate, burst... (rate over distance)
public class UParticleEmitter
{
    public float timeScale = 1f;
    public List<UParticle> particles = new List<UParticle>();

    //Initialization
    public int initialNumber = 10;
    public int maximumNumber = 1000;


    //Render/Output
    public ParticleType particleType = ParticleType.Cube;


    public UParticleEmitter(ParticleType type, int initialNumberOfParticlesInstatiated, int maximumNumberOfParticles)
    {
        this.particleType = type;
        this.initialNumber = initialNumberOfParticlesInstatiated;
        this.maximumNumber = maximumNumberOfParticles;

        //Prewarm particle pool

    }

    public virtual void ActivateEmitter()
    {
        //grab all need particles from the pool (from Assets providers)
    }

    public virtual void Update(float deltaTime)
    {
        var scaledDeltaTime = deltaTime * timeScale;
        //TODO: test if foreach matches with for... 
        foreach (var particle in particles)
        {
            //Apply some forces, atractors, turbulance, noise...

            //Update particle
            particle.Update(scaledDeltaTime);
        }
    }
}

//game object(basically a view)
//color
//current age....
//applying forces
//two modes - rigid body based; transform based
public class UParticle
{
    public GameObject gameObject;
    public Transform transform;
    public Rigidbody rigidbody;
    public PoolableObject poolableObject;

    public Vector3 initialPosition;
    public Quaternion initialRotation;
    public Vector3 initialScale;

    public Vector3 initialForce;

    public float lifespan;
    private float currentAge = 0f;

    private Action<UParticle> OnDestroyed;


    public UParticle(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = gameObject.transform;
        var rigidbody = gameObject.GetComponent<Rigidbody>();

        if (rigidbody != null)
        {
            this.rigidbody = rigidbody;
        }

        this.poolableObject = gameObject.GetComponent<PoolableObject>();
    }

    public virtual void Activate()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        transform.localScale = initialScale;

        currentAge = 0f;
        ApplyForce(initialForce);
    }

    public virtual void ApplyForce(Vector3 force)
    {
        rigidbody.AddForce(force);
    }

    public virtual void Update(float deltaTime)
    {
        currentAge += deltaTime;

        if (currentAge > lifespan)
        {
            //kill particle
            OnDestroyed?.Invoke(this);
            poolableObject.ReturnToPool();
        }
    }
}

//hold a list of emitters
//update emitters
//handle time scale
public class UParticleSystem
{
    public List<UParticleEmitter> emitters = new List<UParticleEmitter>();
    public float timeScale = 1f;

    public UParticleSystem()
    {
        GameTicker.SharedInstance.Update += Update;
    }

    public virtual void Update()
    {
        var scaledDeltaTime = GameTicker.DeltaTime * timeScale;

        foreach (var emitter in emitters)
        {
            emitter.Update(scaledDeltaTime);
        }
    }
}