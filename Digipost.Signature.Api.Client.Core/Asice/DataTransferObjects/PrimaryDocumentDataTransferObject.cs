﻿using System;
using System.Xml.Serialization;

namespace Digipost.Signature.Api.Client.Core.Asice.DataTransferObjects
{
    [Serializable]
    [XmlType(TypeName = "primary-document")]
    public class PrimaryDocumentDataTransferObject
    {
        [XmlElement("title")]
        public string Title { get; set; }
        
        [XmlElement("description")]
        public string Descritpion { get; set; }

        [XmlAttribute("href")]
        public string Href { get; set; }

        [XmlAttribute("mime")]
        public string Mime { get; set; }
    }
}