using System;
using System.Collections.Generic;

namespace GorillaTagScripts.AI
{
	// Token: 0x02000A15 RID: 2581
	public class StateMachine
	{
		// Token: 0x0600409A RID: 16538 RVA: 0x0016CB2C File Offset: 0x0016AD2C
		public void Tick()
		{
			StateMachine.Transition transition = this.GetTransition();
			if (transition != null)
			{
				this.SetState(transition.To);
			}
			IState currentState = this._currentState;
			if (currentState == null)
			{
				return;
			}
			currentState.Tick();
		}

		// Token: 0x0600409B RID: 16539 RVA: 0x0016CB60 File Offset: 0x0016AD60
		public void SetState(IState state)
		{
			if (state == this._currentState)
			{
				return;
			}
			IState currentState = this._currentState;
			if (currentState != null)
			{
				currentState.OnExit();
			}
			this._currentState = state;
			this._transitions.TryGetValue(this._currentState.GetType(), out this._currentTransitions);
			if (this._currentTransitions == null)
			{
				this._currentTransitions = StateMachine.EmptyTransitions;
			}
			this._currentState.OnEnter();
		}

		// Token: 0x0600409C RID: 16540 RVA: 0x000596C2 File Offset: 0x000578C2
		public IState GetState()
		{
			return this._currentState;
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x0016CBCC File Offset: 0x0016ADCC
		public void AddTransition(IState from, IState to, Func<bool> predicate)
		{
			List<StateMachine.Transition> list;
			if (!this._transitions.TryGetValue(from.GetType(), out list))
			{
				list = new List<StateMachine.Transition>();
				this._transitions[from.GetType()] = list;
			}
			list.Add(new StateMachine.Transition(to, predicate));
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x000596CA File Offset: 0x000578CA
		public void AddAnyTransition(IState state, Func<bool> predicate)
		{
			this._anyTransitions.Add(new StateMachine.Transition(state, predicate));
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x0016CC14 File Offset: 0x0016AE14
		private StateMachine.Transition GetTransition()
		{
			foreach (StateMachine.Transition transition in this._anyTransitions)
			{
				if (transition.Condition())
				{
					return transition;
				}
			}
			foreach (StateMachine.Transition transition2 in this._currentTransitions)
			{
				if (transition2.Condition())
				{
					return transition2;
				}
			}
			return null;
		}

		// Token: 0x040041DE RID: 16862
		private IState _currentState;

		// Token: 0x040041DF RID: 16863
		private Dictionary<Type, List<StateMachine.Transition>> _transitions = new Dictionary<Type, List<StateMachine.Transition>>();

		// Token: 0x040041E0 RID: 16864
		private List<StateMachine.Transition> _currentTransitions = new List<StateMachine.Transition>();

		// Token: 0x040041E1 RID: 16865
		private List<StateMachine.Transition> _anyTransitions = new List<StateMachine.Transition>();

		// Token: 0x040041E2 RID: 16866
		private static List<StateMachine.Transition> EmptyTransitions = new List<StateMachine.Transition>(0);

		// Token: 0x02000A16 RID: 2582
		private class Transition
		{
			// Token: 0x17000670 RID: 1648
			// (get) Token: 0x060040A2 RID: 16546 RVA: 0x00059714 File Offset: 0x00057914
			public Func<bool> Condition { get; }

			// Token: 0x17000671 RID: 1649
			// (get) Token: 0x060040A3 RID: 16547 RVA: 0x0005971C File Offset: 0x0005791C
			public IState To { get; }

			// Token: 0x060040A4 RID: 16548 RVA: 0x00059724 File Offset: 0x00057924
			public Transition(IState to, Func<bool> condition)
			{
				this.To = to;
				this.Condition = condition;
			}
		}
	}
}
