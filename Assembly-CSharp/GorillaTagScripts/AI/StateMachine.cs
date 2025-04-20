using System;
using System.Collections.Generic;

namespace GorillaTagScripts.AI
{
	// Token: 0x02000A3F RID: 2623
	public class StateMachine
	{
		// Token: 0x060041D3 RID: 16851 RVA: 0x001739B0 File Offset: 0x00171BB0
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

		// Token: 0x060041D4 RID: 16852 RVA: 0x001739E4 File Offset: 0x00171BE4
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

		// Token: 0x060041D5 RID: 16853 RVA: 0x0005B0C4 File Offset: 0x000592C4
		public IState GetState()
		{
			return this._currentState;
		}

		// Token: 0x060041D6 RID: 16854 RVA: 0x00173A50 File Offset: 0x00171C50
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

		// Token: 0x060041D7 RID: 16855 RVA: 0x0005B0CC File Offset: 0x000592CC
		public void AddAnyTransition(IState state, Func<bool> predicate)
		{
			this._anyTransitions.Add(new StateMachine.Transition(state, predicate));
		}

		// Token: 0x060041D8 RID: 16856 RVA: 0x00173A98 File Offset: 0x00171C98
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

		// Token: 0x040042C6 RID: 17094
		private IState _currentState;

		// Token: 0x040042C7 RID: 17095
		private Dictionary<Type, List<StateMachine.Transition>> _transitions = new Dictionary<Type, List<StateMachine.Transition>>();

		// Token: 0x040042C8 RID: 17096
		private List<StateMachine.Transition> _currentTransitions = new List<StateMachine.Transition>();

		// Token: 0x040042C9 RID: 17097
		private List<StateMachine.Transition> _anyTransitions = new List<StateMachine.Transition>();

		// Token: 0x040042CA RID: 17098
		private static List<StateMachine.Transition> EmptyTransitions = new List<StateMachine.Transition>(0);

		// Token: 0x02000A40 RID: 2624
		private class Transition
		{
			// Token: 0x1700068B RID: 1675
			// (get) Token: 0x060041DB RID: 16859 RVA: 0x0005B116 File Offset: 0x00059316
			public Func<bool> Condition { get; }

			// Token: 0x1700068C RID: 1676
			// (get) Token: 0x060041DC RID: 16860 RVA: 0x0005B11E File Offset: 0x0005931E
			public IState To { get; }

			// Token: 0x060041DD RID: 16861 RVA: 0x0005B126 File Offset: 0x00059326
			public Transition(IState to, Func<bool> condition)
			{
				this.To = to;
				this.Condition = condition;
			}
		}
	}
}
