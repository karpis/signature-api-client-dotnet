﻿using System;
using System.Xml.Serialization;

namespace Digipost.Signature.Api.Client.Core.Asice.DataTransferObjects
{
    [Serializable]
    [XmlType(TypeName = "signer")]
    public class SignerDataTranferObject
    {
        [XmlElement("personal-identification-number")]
        public string PersonalIdentificationNumber { get; set; }
    }
}