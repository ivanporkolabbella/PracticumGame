public class EnemyStateMachine : StateMachine
{
    public float distanceFromTarget;
    public float health;
    public EnemyController3 controller;

    public EnemyStateMachine()
    {
        var idleState = new EnemyIdleState();
        AddState(idleState);

        var chaseState = new EnemyChaseState();
        AddState(chaseState);

        var attackState = new EnemyAttackState();
        AddState(attackState);

        var deadState = new EnemyDeadState();
        AddState(deadState);

        var enrageState = new EnemyEnrageState();
        AddState(enrageState);

        var anyState = new EnemyAnyState();
        AddAnyState(anyState);

        InitializeMachine(typeof(EnemyIdleState));
    }
}
