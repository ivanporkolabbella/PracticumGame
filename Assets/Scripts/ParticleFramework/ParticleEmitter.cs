using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//emit particles (create them, destroy them... pooling system)
//initialize particles with initial values
//update particles...
//Particle emitter to behave as a particle
//different emition modes - constatnt rate, burst... (rate over distance)
public class UParticleEmitter
{
    public float timeScale = 1f;
    public List<UParticle> particles = new List<UParticle>();

    private bool isPrewarmed;

    public GameObject gameObject;

    //Initialization
    public int initialNumber = 10;
    public int maximumNumber = 1000;
    public Vector3 spawnBlock = new Vector3(5, 5, 5);

    //rate over time
    public float particlesPerSecond = 0f;
    private int milisecondsBetweenParticlesEmitted;

    //
    private DelayedExecutionTicket ticket;

    //Render/Output
    public ParticleType particleType = ParticleType.Cube;


    public UParticleEmitter()
    {
        gameObject = new GameObject("Particle Emitter");
    }

    public UParticleEmitter(ParticleType type, int initialNumberOfParticlesInstatiated, int maximumNumberOfParticles = -1)
    {
        gameObject = new GameObject("Particle Emitter");

        this.particleType = type;
        this.initialNumber = initialNumberOfParticlesInstatiated;
        this.maximumNumber = maximumNumberOfParticles > 0 ? maximumNumberOfParticles : initialNumberOfParticlesInstatiated;
    }

    public void Prewarm()
    {
        isPrewarmed = true;
        ParticleAssetProvider.Prewarm(particleType, initialNumber);
    }

    public virtual void Activate()
    {
        if (!isPrewarmed)
        {
            isPrewarmed = true;
            ParticleAssetProvider.Prewarm(particleType, 1);
        }

        //grab all need particles from the pool (from Assets providers)
        if (particlesPerSecond > 0 && milisecondsBetweenParticlesEmitted > 0)
        {
            DelayedExecutionManager.ExecuteActionAfterDelay(milisecondsBetweenParticlesEmitted, () => { Spawn(); });
        }
    }

    public void Spawn()
    {
        var particleObject = ParticleAssetProvider.GetParticle(particleType);
        var particle = new UParticle(particleObject);

        particles.Add(particle);

        particle.OnDestroyed = (destroyedParticle) => { particles.Remove(destroyedParticle); };

        particle.transform.SetParent(gameObject.transform);
        particle.initialPosition = gameObject.transform.position + GetPositionOffset();

        SetupParticle(particle);

        particle.Activate();

        ticket = DelayedExecutionManager.ExecuteActionAfterDelay(milisecondsBetweenParticlesEmitted,() => { Spawn(); });
    }

    private Vector3 GetPositionOffset()
    {
        return new Vector3(spawnBlock.x * (float)HelperFunctions.randomizer.NextDouble(),
            spawnBlock.y * (float)HelperFunctions.randomizer.NextDouble(),
            spawnBlock.z * (float)HelperFunctions.randomizer.NextDouble());
    }

    public virtual void SetupParticle(UParticle particle)
    {

    }

    public virtual void Update(float deltaTime)
    {
        var scaledDeltaTime = deltaTime * timeScale;

        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Update(scaledDeltaTime);
        }
    }

    public void SetParticlesPerSecond(float particlesPerSecond)
    {
        this.particlesPerSecond = particlesPerSecond;
        if (particlesPerSecond > 0)
        {
            milisecondsBetweenParticlesEmitted = (int)(1000f / particlesPerSecond);
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

    public Vector3 initialPosition = Vector3.zero;
    public Quaternion initialRotation = Quaternion.identity;
    public Vector3 initialScale = Vector3.one;

    public Vector3 initialForce = Vector3.zero;

    public float lifespan = 0f;
    private float currentAge = 0f;

    public Action<UParticle> OnDestroyed;


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
        rigidbody.velocity = Vector3.zero;

        ApplyForce(initialForce);
    }

    public virtual void ApplyForce(Vector3 force)
    {
        rigidbody.AddForce(force, ForceMode.Impulse);
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

    public virtual void Update()
    {
        var scaledDeltaTime = GameTicker.DeltaTime * timeScale;

        foreach (var emitter in emitters)
        {
            emitter.Update(scaledDeltaTime);
        }
    }

    public void Activate()
    {
        foreach (var emitter in emitters)
        {
            emitter.Activate();
        }

        GameTicker.SharedInstance.Update += Update;
    }

    public void AddEmitter(UParticleEmitter emitter)
    {
        emitters.Add(emitter);
    }
}