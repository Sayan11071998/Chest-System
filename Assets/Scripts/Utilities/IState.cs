namespace ChestSystem.Utilities
{
    public interface IState
    {
        void OnStateEnter();
        void Update();
        void OnStateExit();
    }
}