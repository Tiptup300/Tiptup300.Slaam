namespace SlaamMono.Library
{
    public interface IResolver<TInputRequest, TOutputResponse>
    {
        TOutputResponse Execute(TInputRequest request);
    }

    public interface IResolver<TOutputResponse>
    {
        TOutputResponse Execute();
    }
}
