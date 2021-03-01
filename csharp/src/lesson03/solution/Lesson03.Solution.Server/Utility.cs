
using System;
using System.Collections.Generic;
using OpenTracing.Propagation;
using OpenTracing.Tag;

namespace OpenTracing.Tutorial.Lesson03.Solution.Server
{
    public class Utility
    {
        public static IScope StartServerSpan(ITracer tracer, IDictionary<string, string> headers, string operationName)
        {
            ISpanBuilder spanBuilder;
            try
            {
                ISpanContext parentSpanCtx = tracer.Extract(BuiltinFormats.HttpHeaders, new TextMapExtractAdapter(headers));

                spanBuilder = tracer.BuildSpan(operationName);
                if (parentSpanCtx != null)
                {
                    spanBuilder = spanBuilder.AsChildOf(parentSpanCtx);
                }
            }
            catch (Exception)
            {
                spanBuilder = tracer.BuildSpan(operationName);
            }

            // TODO could add more tags like http.url
            return spanBuilder.WithTag(Tags.SpanKind, Tags.SpanKindServer).StartActive(true);
        }
    }
}