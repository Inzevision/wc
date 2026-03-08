namespace Core
{
    using UnityEngine;
    
    public class GameBootstrap : MonoBehaviour
    {
        // Ссылка на нашу машину состояний
        private GameStateMachine _stateMachine;
    
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    
        private void Start()
        {
            InitGame();
        }
    
        private void InitGame()
        {
            // Создаем машину состояний
            _stateMachine = new GameStateMachine();
            
            // Говорим ей: "Запусти самое первое состояние загрузки!"
            _stateMachine.Enter<BootstrapState>();
        }
    }
}

