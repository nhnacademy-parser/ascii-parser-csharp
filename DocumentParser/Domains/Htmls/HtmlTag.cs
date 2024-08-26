using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DocumentParser.Domains.Htmls
{
    public class HtmlTag
    {
        private const char LineBreak = '\n';
        private const char Indent = '\t';
        private const char DoubleQuotes = '\"';

        private HtmlTag()
        {
        }

        public static string TagBlock(string tagName, string value, params KeyValuePair<string, string[]>[] attributes)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<");
            sb.Append(tagName);
            foreach (KeyValuePair<string, string[]> attribute in attributes)
            {
                sb.Append(" ");
                sb.Append(attribute.Key).Append("=")
                    .Append(DoubleQuotes);

                foreach (string v in attribute.Value)
                {
                    sb.Append(v);
                    sb.Append(" ");
                }

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
            return TagBlock(tagName, value, new KeyValuePair<string, string[]>[] { });
        }

        public static string TagBlock(string tagName, string value, params KeyValuePair<string, string>[] attributes)
        {
            KeyValuePair<string, string[]>[] attributesArray = new KeyValuePair<string, string[]>[attributes.Length];

            int i = 0;
            foreach (KeyValuePair<string, string> attribute in attributes)
            {
                attributesArray.SetValue(
                    new KeyValuePair<string, string[]>(attribute.Key, new string[] { attribute.Value }), i);
                i++;
            }

            return TagBlock(tagName, value, attributesArray);
        }

        public static string TagBlock(string tagName, string value, string attributeName, string attributeValue)
        {
            return TagBlock(tagName, value, new KeyValuePair<string, string>(attributeName, attributeValue));
        }
    }
}