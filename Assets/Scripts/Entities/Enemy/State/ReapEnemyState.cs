
public class ReapEnemyState : State
{
   
    public ReapEnemyState(Enemy enemy,EnemyStateMachine stateMachine) : base(enemy,stateMachine)
    {
    }
    
    // Enemy runs away from Player
    public override void Action()
    {
        
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        enemy.RaiseReapEvent();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
    
    public override void Decision()
    {
        base.Decision();
    }
}
