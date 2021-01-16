using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntryPointController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#else
            Debug.unityLogger.logEnabled = false;
#endif

        //preloading of music and sound
        //common assets

        //setup singletons
        AssetProvider.Prewarm();
        InputManager.Activate();
        //network connections

        //create main controller
        var mainMenuVC = new MainMenuController();


        var dungeonVC = new DungeonController();
        //UNavigationController.SetRootViewController(dungeonVC);

        var particleTest = new ParticleTestController();
        UNavigationController.SetRootViewController(particleTest);
        //UNavigationController.SetRootViewController(mainMenuVC);
    }
}


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