namespace SlaamMono.Library
{
    public interface IRequest<TRequest, TResponse>
    {
        TResponse Execute(TRequest request);
    }
}
