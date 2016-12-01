﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Difi.Felles.Utility;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Internal;

namespace Digipost.Signature.Api.Client.Core
{
    public abstract class BaseClient
    {
        protected const int TooManyRequestsStatusCode = 429;
        protected const string NextPermittedPollTimeHeader = "X-Next-permitted-poll-time";
        private HttpClient _httpClient;

        protected BaseClient(ClientConfiguration clientConfiguration)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            ClientConfiguration = clientConfiguration;
            HttpClient = MutualTlsClient();
            RequestHelper = new RequestHelper(HttpClient);
        }

        public ClientConfiguration ClientConfiguration { get; }

        internal HttpClient HttpClient
        {
            get { return _httpClient; }
            set
            {
                _httpClient = value;
                RequestHelper = new RequestHelper(value);
            }
        }

        internal RequestHelper RequestHelper { get; set; }

        protected Sender CurrentSender(Sender jobSender)
        {
            var sender = jobSender ?? ClientConfiguration.GlobalSender;
            if (sender == null)
            {
                throw new SenderNotSpecifiedException();
            }

            ValidateSenderCertificateThrowIfInvalid(sender);

            return sender;
        }

        private void ValidateSenderCertificateThrowIfInvalid(Sender sender)
        {
            var x509Certificate2 = new X509Certificate2(ClientConfiguration.Certificate);

            var validationResult = CertificateValidator.ValidateCertificateAndChain(x509Certificate2, sender.OrganizationNumber, ClientConfiguration.Environment.CertificateChainValidator.CertificateStore);

            if (validationResult.Type != CertificateValidationType.Valid)
            {
                throw new SecurityException($"Sertificate used for {nameof(sender)} is not valid. Are you trying to use a test certificate in a production environment or the other way around? The reason is '{validationResult.Type}', description: '{validationResult.Message}'", null);
            }
        }

        private HttpClient MutualTlsClient()
        {
            var client = HttpClientFactory.Create(
                MutualTlsHandler(),
                new XsdRequestValidationHandler(),
                new UserAgentHandler(),
                new LoggingHandler()
            );

            client.Timeout = TimeSpan.FromMilliseconds(ClientConfiguration.HttpClientTimeoutInMilliseconds);
            client.BaseAddress = ClientConfiguration.Environment.Url;

            return client;
        }

        private WebRequestHandler MutualTlsHandler()
        {
            var certificateCollection = new X509Certificate2Collection {ClientConfiguration.Certificate};
            var mutualTlsHandler = new WebRequestHandler();
            mutualTlsHandler.ClientCertificates.AddRange(certificateCollection);
            mutualTlsHandler.ServerCertificateValidationCallback = ValidateServerCertificateThrowIfInvalid;

            return mutualTlsHandler;
        }

        private bool ValidateServerCertificateThrowIfInvalid(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            var x509Certificate2 = new X509Certificate2(certificate);

            var validationResult = CertificateValidator.ValidateCertificateAndChain(x509Certificate2, ClientConfiguration.ServerCertificateOrganizationNumber, ClientConfiguration.Environment.CertificateChainValidator.CertificateStore);

            if (validationResult.Type != CertificateValidationType.Valid)
            {
                throw new SecurityException($"Certificate received in response is not valid. The reason is '{validationResult.Type}', description: '{validationResult.Message}'", null);
            }

            return true;
        }
    }
}