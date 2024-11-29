using GF.Fussion.Web.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.ComponentModel;
using System.Text.Encodings.Web;

namespace GF.Fussion.Web.TagHelpers;

[HtmlTargetElement("button-action")]
public sealed class ButtonActionTagHelper : TagHelper
{
    #region Private Fields
    private const string DefaultIcon = "circle-question";
    #endregion

    #region Public Properties
    public BootstrapColor Color { get; set; } = BootstrapColor.Secondary;
    public IconPosition IconPosition { get; set; } = IconPosition.End;
    public string TagName { get; set; } = "button";
    public string? Icon { get; set; }
    public bool OnlyIcon { get; set; }
    #endregion

    private static TagBuilder IconBuilder (string iconName, bool isCollapsible = true)
    {
        TagBuilder builder = new("i");

        builder.AddCssClass("fa-solid");
        builder.AddCssClass($"fa-{iconName}");
        builder.AddCssClass("pe-none");

        if (isCollapsible)
        {
            builder.AddCssClass("d-none");
            builder.AddCssClass("d-md-inline");
        }

        return builder;
    }
    private static void AppendIcon (TagHelperContent content, string iconName, IconPosition iconPosition)
    {
        TagBuilder iconBuilder = IconBuilder(iconName);

        switch (iconPosition)
        {
            case IconPosition.Start:
                iconBuilder.AddCssClass("me-2");
                break;

            case IconPosition.End:
                iconBuilder.AddCssClass("ms-2");
                break;
        }

        content.AppendHtml(iconBuilder);
    }

    public sealed override void Process (TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = TagName;
        output.TagMode = TagMode.StartTagAndEndTag;

        switch (TagName)
        {
            case "a":
                output.Attributes.SetAttribute("role", "button");
                break;

            case "button":
                if (output.Attributes.TryGetAttribute("type", out TagHelperAttribute attrType))
                {
                    if (attrType.Value.ToString() == "submit")
                    {
                        Color = BootstrapColor.Primary;
                    }
                }
                else
                {
                    output.Attributes.SetAttribute("type", "button");
                }
                break;
        }

        SetTagClasses();

        if (OnlyIcon)
        {
            AppendIcon(output.Content, Icon ?? DefaultIcon, IconPosition.Undefined);
        }
        else
        {
            SetContentTitle();

            if (!string.IsNullOrWhiteSpace(Icon))
            {
                TagHelperContent content = IconPosition switch
                {
                    IconPosition.Start => output.PreElement,
                    IconPosition.End => output.PostContent,
                    _ => throw new InvalidEnumArgumentException()
                };

                AppendIcon(content, Icon, IconPosition);
            }
        }

        void SetTagClasses ()
        {
            output.AddClass("btn", HtmlEncoder.Default);
            output.AddClass($"btn-{Color.ToString().ToLower()}", HtmlEncoder.Default);
            output.AddClass("text-decoration-none", HtmlEncoder.Default);
            output.AddClass("px-3", HtmlEncoder.Default);

            if (output.Attributes.ContainsName("shadow"))
            {
                output.AddClass("shadow-sm", HtmlEncoder.Default);
                output.Attributes.RemoveAll("shadow");
            }
        }

        void SetContentTitle ()
        {
            if (output.Attributes.TryGetAttribute("title", out TagHelperAttribute titleAttr))
            {
                output.Content.SetContent(titleAttr.Value.ToString());
            }
        }
    }
}