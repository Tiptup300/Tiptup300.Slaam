namespace Tiptup300.Slaam.Library.Widgets;

public class TimerWidget
{
   public bool Active = false;
   public bool MakeUpTime = false;

   /// <summary>
   /// The amount of time the input has been held
   /// </summary>
   private TimeSpan HoldCount;
   public TimeSpan TimeLeft
   {
      get
      {
         return threshold - HoldCount;
      }
   }

   /// <summary>
   /// The maximum amount of time the input can be held before it is again 
   /// considered "Active"
   /// </summary>
   public TimeSpan Threshold
   {
      get
      {
         return threshold;
      }
      set
      {
         threshold = value;
         Reset();
      }
   }

   /// <summary>
   /// The maximum amount of time the input can be held before it is again 
   /// considered "Active"
   /// </summary>
   private TimeSpan threshold;

   public TimerWidget(TimeSpan? threshold = null)
   {
      this.threshold = threshold is null ? TimeSpan.Zero : threshold.Value;
   }

   public void Update(TimeSpan TimeElapsed)
   {
      HoldCount += TimeElapsed;

      if (MakeUpTime)
      {
         if (HoldCount.TotalMilliseconds > Threshold.TotalMilliseconds * 3)
         {
            HoldCount = Threshold;
         }
      }

      if (HoldCount >= threshold)
      {
         Active = true;
         HoldCount -= threshold;
      }
      else
      {
         Active = false;
      }

   }

   public void Reset()
   {
      HoldCount = TimeSpan.Zero;
      Active = false;
   }
}
