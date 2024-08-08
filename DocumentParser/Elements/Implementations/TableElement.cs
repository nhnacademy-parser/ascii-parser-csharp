﻿using System;
using System.Collections.Generic;
using DocumentParser.Visitors;

namespace DocumentParser.Elements.Implementations
{
    public class TableElement : DocsElement
    {
        public TableElement() : base()
        {
            ColumnHeading = new List<string>();
            Rows = new List<string[]>();
        }

        public List<string> ColumnHeading { get; private set; }
        public List<string[]> Rows { get; private set; }
        public string Title { get { return Value.ToString(); } }

        public override object Accept(IDocumentVisitor visitor)
        {
            return visitor.Visit(this);
        }

        //public void AddChild (string column)
        //{

        //    if (Rows.Count <= 0)
        //    {
        //        if(column == "\n")
        //        {
        //            Rows.Add(new string[ColumnHeading.Count]);
        //        }
        //    }

        //}
    }
}