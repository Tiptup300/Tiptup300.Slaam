namespace SlaamMono.Library
{
    public interface IRequest<TInputRequest, TOutputResponse>
    {
        TOutputResponse Execute(TInputRequest request);
    }

    public interface IRequest<TOutputResponse>
    {
        TOutputResponse Execute();
    }
}
