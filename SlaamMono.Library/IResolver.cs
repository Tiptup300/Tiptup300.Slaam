namespace SlaamMono.Library
{
    public interface IResolver<TInputRequest, TOutputResponse> where TInputRequest : IRequest
    {
        TOutputResponse Resolve(TInputRequest request);
    }

    public interface IResolver<TOutputResponse>
    {
        TOutputResponse Resolve();
    }
}
