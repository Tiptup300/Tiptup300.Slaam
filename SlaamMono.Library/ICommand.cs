namespace SlaamMono.Library
{
    public interface ICommand<TInputRequest>
    {
        void Execute(TInputRequest request);
    }
}
