using UnityEngine;

public abstract class Ability : ScriptableObject
{
    protected IAbilityExecutionHandler executionHandler;
    protected LaughTaleCharacterController source;
    protected LaughTaleCharacterController target;

    public void ExecuteAbility(LaughTaleCharacterController source, LaughTaleCharacterController target, IAbilityExecutionHandler executionHandler)
    {
        this.executionHandler = executionHandler;
        this.source = source;
        this.target = target;
        Execute();
    }

    public abstract void Execute();

    public virtual void StartAbilityExecution()
    {
        executionHandler.OnBeforeAbilityExecuted(this);
    }

    public virtual void EndAbilityExecution()
    {
        executionHandler.OnAfterAbilityExecuted(this);
    }
}
