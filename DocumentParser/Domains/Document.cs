﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DocumentParser.Elements;
using DocumentParser.Elements.Implementations;
using DocumentParser.Visitors.implementations;

namespace DocumentParser.Domain
{
    public class Document : IEnumerable<IDocumentElement>
    {


        private List<IDocumentElement> _elements;

        public Document()
        {
            _elements = new List<IDocumentElement>();
        }

        public void Append(IDocumentElement element)
        {
            _elements.Add(element);
        }
        
        public string Convert(HtmlConverter htmlConverter)
        {
            
            StringBuilder sb = new StringBuilder();

            foreach (IDocumentElement element in this)
            {
                sb.Append(element.Accept(htmlConverter));
            }
            
            return sb.ToString();
        }

        public IEnumerator<IDocumentElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
