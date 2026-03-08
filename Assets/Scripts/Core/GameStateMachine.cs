namespace Core
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class GameStateMachine
    {
        // Словарь, где мы храним все наши состояния
        private Dictionary<Type, IGameState> _states;
    
        // Текущее активное состояние
        private IGameState _currentState;

        // Конструктор: здесь мы создаем список всех состояний игры
        public GameStateMachine()
        {
            _states = new Dictionary<Type, IGameState>
            {
                // Пока у нас есть только одно тестовое состояние, позже добавим карту и локации
                [typeof(BootstrapState)] = new BootstrapState(this)
            };
        }

        // Главный метод для переключения состояний
        public void Enter<TState>() where TState : IGameState
        {
            // 1. Если мы уже в каком-то состоянии, выходим из него
            _currentState?.Exit();

            // 2. Находим новое состояние в нашем словаре
            IGameState state = _states[typeof(TState)];
            _currentState = state;

            // 3. Запускаем новое состояние
            _currentState.Enter();
        }
    }

// --- ТЕСТОВОЕ СОСТОЯНИЕ (Прямо в этом же файле для удобства) ---
    public class BootstrapState : IGameState
    {
        private GameStateMachine _stateMachine;

        public BootstrapState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            Debug.Log(">>> [State] Мы вошли в состояние BOOTSTRAP...");
        }

        public void Exit()
        {
            Debug.Log("<<< [State] Мы вышли из состояния BOOTSTRAP.");
        }
    }
}