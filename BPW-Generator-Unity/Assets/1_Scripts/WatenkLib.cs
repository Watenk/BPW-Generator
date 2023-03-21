using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatenkLib
{
    public float PercentageToDecimal(float percentage)
    {
        return percentage /= 100;
    }

    public float DecimalToPercentage(float percentage)
    {
        return percentage *= 100;
    }

    public void ConvertMouseToInts(Vector2 mouseInput, out int mouseXInt, out int mouseYInt)
    {
        mouseXInt = (int)Mathf.Round(mouseInput.x);
        mouseYInt = (int)Mathf.Round(mouseInput.y);
    }

    public int GetRandomInt(int min, int max)
    {
        return Random.Range(min, max);
    }
}

public class FSM
{
    public Dictionary<System.Type, BaseState> States = new Dictionary<System.Type, BaseState>(); 
    public BaseState currentState;

    public FSM(BaseState[] states)
    {
        foreach (BaseState state in states)
        {
            state.SetOwner(this);
            States.Add(state.GetType(), state);
        }
    }
    public void SwitchState(System.Type newState)
    {
        currentState?.OnExit();
        currentState = States[newState];
        currentState?.OnAwake();
        currentState?.OnStart();
    }

    public void OnUpdate()
    {
        currentState?.OnUpdate();
    }

    public void OnExit()
    {
        currentState?.OnExit();
    }
}

public abstract class BaseState : MonoBehaviour
{
    protected FSM owner;
    public void SetOwner(FSM owner) { this.owner = owner; }
    public virtual void OnAwake() { }
    public virtual void OnStart() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }
}