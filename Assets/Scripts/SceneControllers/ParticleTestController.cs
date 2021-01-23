public class ParticleTestController : USceneController
{
    public ParticleTestController() : base(SceneNames.ParticleTest)
    {

    }

    public override void SceneDidLoad()
    {
        var particleSystem = new MyFirstParticleSystem();
        particleSystem.Activate();
    }
}

public class MyFirstParticleSystem : UParticleSystem
{
    public MyFirstParticleSystem()
    {
        var emitter = new MyFirstParticleEmitter();
        AddEmitter(emitter);
    }
}

public class MyFirstParticleEmitter : UParticleEmitter
{
    public MyFirstParticleEmitter()
    {
        initialNumber = 300;
        particleType = ParticleType.Cube;
        SetParticlesPerSecond(100);
        Prewarm();
    }

    public override void SetupParticle(UParticle particle)
    {
        particle.lifespan = 3f;
        particle.initialForce = HelperFunctions.RandomVector(5, 15, 5);
    }
}