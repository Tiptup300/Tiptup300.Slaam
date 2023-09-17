namespace ZzziveGameEngine;

 public interface IResolver<TInputRequest, TOutputResponse>
 {
     TOutputResponse Resolve(TInputRequest request);
 }

 public interface IResolver<TOutputResponse>
 {
     TOutputResponse Resolve();
 }
