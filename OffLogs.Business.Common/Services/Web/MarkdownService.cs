using System;
using ColorCode;
using ColorCode.Styling;
using Markdig;
using Markdown.ColorCode;

namespace OffLogs.Business.Common.Services.Web;

public class MarkdownService: IMarkdownService
{
    private static MarkdownPipeline _pipeline;
    
    private static MarkdownPipeline Pipeline
    {
        get
        {
            if (_pipeline == null)
            {
                _pipeline = new MarkdownPipelineBuilder()
                    .UseAdvancedExtensions()
                    .UseColorCode(StyleDictionary.DefaultDark)
                    .Build();
            }

            return _pipeline;
        }
    }

    public string ToHtml(string markdown)
    {
        return Markdig.Markdown.ToHtml($"{markdown}", Pipeline);
    }
}
