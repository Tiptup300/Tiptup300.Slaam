using Microsoft.Xna.Framework.Content.Pipeline;

using TInput = System.Collections.Generic.List<string>;
using TOutput = System.Collections.Generic.List<string>;

namespace TXTProcesser
{
    [ContentProcessor(DisplayName = "TextProcessor")]
    class Processor1 : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            return input;
        }
    }
}
