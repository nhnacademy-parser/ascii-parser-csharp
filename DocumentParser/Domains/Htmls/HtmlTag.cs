using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace DocumentParser.Domains.Htmls
{
    public class HtmlTag
    {
        private const char LineBreak = '\n';
        private const char Indent = '\t';
        private const char DoubleQuotes = '\"';

        public static string TagBlock(string tagName, string value, List<TagAttribute> attributes)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<");
            sb.Append(tagName);
            foreach (TagAttribute attribute in attributes)
            {
                sb.Append(" ");
                sb.Append(attribute.Name).Append("=")
                    .Append(DoubleQuotes);

                sb.Append(attribute.ValueString);

                sb.Append(DoubleQuotes);
            }

            sb.Append(">");
            sb.Append(value);

            sb.Append(Indent).Append("</").Append(tagName).Append(">");
            sb.Append(LineBreak);

            return sb.ToString();
        }

        public static string TagBlock(string tagName, string value)
        {
            return TagBlock(tagName, value, list => { });
        }

        public static string TagBlock(string tagName, string value, string attributeName, string attributeValue)
        {
            return TagBlock(tagName, value, new TagAttribute(attributeName, attributeValue));
        }

        public static string TagBlock(string tagName, string value, TagAttribute attribute)
        {
            return TagBlock(tagName, value, list => { list.Add(attribute); });
        }

        public static string TagBlock(string tagName, string value, Action<List<TagAttribute>> action)
        {
            List<TagAttribute> attributes = new List<TagAttribute>();
            action(attributes);
            
            return TagBlock(tagName, value, attributes);
        }    

    }

    public class TagAttribute
    {
        public string Name { get; }
        public string[] Values { get; }
        public string ValueString => string.Join(" ",Values);

        public TagAttribute(string name, params string[] values)
        {
            Name = name;
            Values = values;
        }
    }
}