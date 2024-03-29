﻿using HtmlAgilityPack;

namespace Bootstrap.Infrastructure.Emailing.EmailDelivery;

public static class HtmlUtilities
{
    public static string ConvertToPlainText(string html)
    {
        HtmlDocument doc = new();
        doc.LoadHtml(html);

        using (StringWriter sw = new())
        {
            ConvertTo(doc.DocumentNode, sw);
            sw.Flush();
            return RemoveDuplicatedSpacesAndNewLines(sw.ToString());
        }
    }

    private static string RemoveDuplicatedSpacesAndNewLines(string value) =>
        string.Join(
            ' ',
            value.Split(new[] { " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToArray()
        );

    private static void ConvertContentTo(HtmlNode node, TextWriter outText)
    {
        foreach (var subnode in node.ChildNodes)
        {
            ConvertTo(subnode, outText);
        }
    }

    private static void ConvertTo(HtmlNode node, TextWriter outText)
    {
        string html;
        switch (node.NodeType)
        {
            case HtmlNodeType.Comment:
                // don't output comments
                break;

            case HtmlNodeType.Document:
                ConvertContentTo(node, outText);
                break;

            case HtmlNodeType.Text:
                // script and style must not be output
                var parentName = node.ParentNode.Name;
                if (parentName == "script" || parentName == "style")
                {
                    break;
                }

                // get text
                html = ((HtmlTextNode)node).Text;

                // is it in fact a special closing node output as text?
                if (HtmlNode.IsOverlappedClosingElement(html))
                {
                    break;
                }

                // check the text is meaningful and not a bunch of whitespaces
                if (html.Trim().Length > 0)
                {
                    outText.Write(HtmlEntity.DeEntitize(html));
                }

                break;

            case HtmlNodeType.Element:
                switch (node.Name)
                {
                    case "p":
                        // treat paragraphs as crlf
                        outText.Write("\r\n");
                        break;
                    case "br":
                        outText.Write("\r\n");
                        break;
                }

                if (node.HasChildNodes)
                {
                    ConvertContentTo(node, outText);
                }

                break;
        }
    }
}
