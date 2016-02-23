﻿using System.IO;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Extensions;

namespace Digipost.Signature.Api.Client.Core
{
    public class Document : IAsiceAttachable
    {
        public Document(string subject, string message, string fileName, FileType fileType, byte[] documentBytes)
        {
            Subject = subject;
            Message = message;
            FileName = fileName;
            MimeType = fileType.ToMimeType();
            Bytes = documentBytes;
        }

        public Document(string subject, string message, string fileName, FileType fileType, string documentPath)
            : this(subject, message, fileName, fileType, File.ReadAllBytes(documentPath))
        {
        }

        public string Subject { get; private set; }

        public string Message { get; private set; }

        public string FileName { get; }

        public string Id => "Id_0";

        public string MimeType { get; }

        public byte[] Bytes { get; }
    }
}