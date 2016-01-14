﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Digipost.Signature.Api.Client.Core.Asice.AsiceManifest;
using Digipost.Signature.Api.Client.Direct;

namespace Digipost.Signature.Api.Client.Core.Tests.Utilities
{
    public static class DomainUtility
    {
        static readonly ResourceUtility ResourceUtility = new ResourceUtility("Digipost.Signature.Api.Client.Core.Tests.Resources");

        public static Manifest GetManifest()
        {
            return new Manifest(
                GetSender(),
                GetDocument(),
                GetSigners(2)
                );
        }

        public static DirectJob GetDirectJob()
        {
            return new DirectJob("Reference", GetSigner(), GetDocument(), GetExitUrls());
        }

        public static Document GetDocument()
        {
            return new Document("Testdocument", "A test document from domain Utility", "TestFileName", FileType.Pdf, GetPdfDocumentBytes());
        }

        public static Sender GetSender()
        {
            return new Sender("123456789");
        }

        public static Signer GetSigner()
        {
            return GetSigners(1).First();
        }

        public static List<Signer> GetSigners(int count)
        {
            if (count > 9)
            {
                throw new ArgumentException("Maximum of 9 senders.");
            }

            var signers = new List<Signer>();

            const string basePersonalIdentificationNumber = "01234567890";
            for (var i = 0; i < count; i++)
            {
                signers.Add(new Signer(basePersonalIdentificationNumber + i));
            }

            return signers;
        }

        public static byte[] GetPdfDocumentBytes()
        {
            return ResourceUtility.ReadAllBytes(true, "Documents", "Dokument.pdf");
        }

        public static X509Certificate2 GetCertificate()
        {
            return EternalTestCertificateWithPrivateKey();
        }

        private static X509Certificate2 EternalTestCertificateWithPrivateKey()
        {
            return new X509Certificate2(ResourceUtility.ReadAllBytes(true, "Certificates", "Unittests", "DigipostCert.p12"), password: "", keyStorageFlags: X509KeyStorageFlags.Exportable);
        }

        public static ResponseUrls GetResponseUrls()
        {
            //Arrange
            var redirectUrl = new Uri("http://responseurl.no");
            var statusUrl = new Uri("http://statusurl.no");


            //Act
            return new ResponseUrls(
                redirectUrl,
                statusUrl
                );
        }

        public static ExitUrls GetExitUrls()
        {
            var completionUrl = new Uri("http://localhost/completion");
            var cancellationUrl = new Uri("http://localhost/cancellation");
            var errorUrl = new Uri("http://localhost/error");

            return new ExitUrls(completionUrl, cancellationUrl, errorUrl);
        }

    }
}
