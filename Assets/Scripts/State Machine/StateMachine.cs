using UnityEngine;

public class StateMachine<T>
{
	public State<T> CurrentState { get; private set; }
	public LayerMask CharactersLayer;

	public void ChangeState(State<T> newState)
	{
		CurrentState?.Exit();
		CurrentState = newState;
		CurrentState?.Enter();
	}

	public void Update()
	{
		CurrentState?.Update();
	}

}
