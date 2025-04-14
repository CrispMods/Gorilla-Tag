using System;
using System.Collections.Generic;

namespace GorillaTagScripts.AI
{
	// Token: 0x02000A12 RID: 2578
	public class StateMachine
	{
		// Token: 0x0600408E RID: 16526 RVA: 0x00133230 File Offset: 0x00131430
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

		// Token: 0x0600408F RID: 16527 RVA: 0x00133264 File Offset: 0x00131464
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

		// Token: 0x06004090 RID: 16528 RVA: 0x001332CE File Offset: 0x001314CE
		public IState GetState()
		{
			return this._currentState;
		}

		// Token: 0x06004091 RID: 16529 RVA: 0x001332D8 File Offset: 0x001314D8
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

		// Token: 0x06004092 RID: 16530 RVA: 0x0013331F File Offset: 0x0013151F
		public void AddAnyTransition(IState state, Func<bool> predicate)
		{
			this._anyTransitions.Add(new StateMachine.Transition(state, predicate));
		}

		// Token: 0x06004093 RID: 16531 RVA: 0x00133334 File Offset: 0x00131534
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

		// Token: 0x040041CC RID: 16844
		private IState _currentState;

		// Token: 0x040041CD RID: 16845
		private Dictionary<Type, List<StateMachine.Transition>> _transitions = new Dictionary<Type, List<StateMachine.Transition>>();

		// Token: 0x040041CE RID: 16846
		private List<StateMachine.Transition> _currentTransitions = new List<StateMachine.Transition>();

		// Token: 0x040041CF RID: 16847
		private List<StateMachine.Transition> _anyTransitions = new List<StateMachine.Transition>();

		// Token: 0x040041D0 RID: 16848
		private static List<StateMachine.Transition> EmptyTransitions = new List<StateMachine.Transition>(0);

		// Token: 0x02000A13 RID: 2579
		private class Transition
		{
			// Token: 0x1700066F RID: 1647
			// (get) Token: 0x06004096 RID: 16534 RVA: 0x00133416 File Offset: 0x00131616
			public Func<bool> Condition { get; }

			// Token: 0x17000670 RID: 1648
			// (get) Token: 0x06004097 RID: 16535 RVA: 0x0013341E File Offset: 0x0013161E
			public IState To { get; }

			// Token: 0x06004098 RID: 16536 RVA: 0x00133426 File Offset: 0x00131626
			public Transition(IState to, Func<bool> condition)
			{
				this.To = to;
				this.Condition = condition;
			}
		}
	}
}
