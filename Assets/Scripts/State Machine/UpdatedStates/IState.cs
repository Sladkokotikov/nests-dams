namespace StateMachine.UpdatedStates
{
    public interface IState
    {
        public IState NextState(byte b);
    }
}