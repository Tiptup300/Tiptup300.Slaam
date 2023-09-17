using System;

namespace SlaamMono.Subclasses;

 public class Transition
 {
     public TimeSpan Length { get; private set; }
     public TimeSpan Elapsed { get; private set; }

     public Transition(TimeSpan length)
     {
         Length = length;
     }

     public float Position => Math.Min((float)Elapsed.TotalMilliseconds / (float)this.Length.TotalMilliseconds, 1f);
     public bool IsFinished => Elapsed >= Length;

     public void AddProgress(TimeSpan elapsed)
     {
         Elapsed += elapsed;
     }

     public void Reset()
     {
         Elapsed = TimeSpan.Zero;
     }

     public void Reset(TimeSpan newLength)
     {
         Length = newLength;
         Reset();
     }
 }