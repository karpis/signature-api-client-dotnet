﻿using System.Linq;
using Digipost.Signature.Api.Client.Core.Asice.AsiceSignature;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests.Asice.AsiceSignature
{
    [TestClass]
    public class SignatureGeneratorTests
    {
        internal SignatureGenerator GetSignaturGenerator()
        {
            var document = DomainUtility.GetDirectDocument();
            var sender = CoreDomainUtility.GetSender();
            var signer = DomainUtility.GetSigner();
            var manifest = new DirectManifest(sender, document, signer);
            var x509Certificate2 = CoreDomainUtility.GetTestCertificate();
            var signaturGenerator = new SignatureGenerator(x509Certificate2, document, manifest);
            return signaturGenerator;
        }

        [TestClass]
        public class ConstructorMethod : SignatureGeneratorTests
        {
            [TestMethod]
            public void InitializesWithDocumentDirectManifestAndCertificate()
            {
                //Arrange
                var document = DomainUtility.GetDirectDocument();
                var sender = CoreDomainUtility.GetSender();
                var manifest = new DirectManifest(sender, document, DomainUtility.GetSigner());
                var x509Certificate2 = CoreDomainUtility.GetTestCertificate();

                //Act
                var signatur = new SignatureGenerator(x509Certificate2, document, manifest);

                //Assert
                Assert.AreEqual(document, signatur.Attachables.ElementAt(0));
                Assert.AreEqual(manifest, signatur.Attachables.ElementAt(1));
                Assert.AreEqual(x509Certificate2, signatur.Certificate);
            }
        }

        [TestClass]
        public class FileNameMethod : SignatureGeneratorTests
        {
            [TestMethod]
            public void ReturnsCorrectStaticFileName()
            {
                //Arrange
                var signaturGenerator = GetSignaturGenerator();
                const string expectedFileName = "META-INF/signatures.xml";

                //Act

                //Assert
                Assert.AreEqual(expectedFileName, signaturGenerator.FileName);
            }
        }

        [TestClass]
        public class IdMethod : SignatureGeneratorTests
        {
            [TestMethod]
            public void ReturnsCorrectStaticId()
            {
                //Arrange
                var signaturGenerator = GetSignaturGenerator();
                const string expectedId = "Id_0";

                //Act

                //Assert
                Assert.AreEqual(expectedId, signaturGenerator.Id);
            }
        }

        [TestClass]
        public class XmlMethod : SignatureGeneratorTests
        {
            [TestMethod]
            public void GeneratesValidSignatureXml()
            {
                //Arrange
                var signatureGenerator = DomainUtility.GetSignature();
                var xml = signatureGenerator.Xml().InnerXml;
                var signatureValidator = new SignatureValidator();

                //Act
                var isValidSignatureXml = signatureValidator.Validate(xml);
                var signatureLength = xml.Length;

                //Assert
                Assert.IsTrue(isValidSignatureXml);
                Assert.IsTrue(signatureLength > 3200);
            }
        }
    }
}