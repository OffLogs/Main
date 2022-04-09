using System;
using Markdig;
using Markdown.ColorCode;

namespace OffLogs.Web.Core.Helpers
{
    public static class MarkdownHelper
    {
        public static string ToHtml(string markdownString)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseColorCode()
                .Build();

            return Markdig.Markdown.ToHtml(markdownString, pipeline);
        }
    }
}
