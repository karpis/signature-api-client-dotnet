﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Digipost.Signature.Api.Client.Core.Internal;

namespace Digipost.Signature.Api.Client.Core
{
    public abstract class BaseClient
    {
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

        private HttpClient MutualTlsClient()
        {
            var mutualTlsHandler = MutualTlsHandler();
            var userAgentHttpHandler = new UserAgentHttpHandler(mutualTlsHandler);
            var loggingHandler = new LoggingHandler(userAgentHttpHandler);

            return new HttpClient(loggingHandler)
            {
                Timeout = TimeSpan.FromMilliseconds(5000),
                BaseAddress = ClientConfiguration.Environment.Url
            };
        }

        private WebRequestHandler MutualTlsHandler()
        {
            var certificateCollection = new X509Certificate2Collection {ClientConfiguration.Certificate};
            var mutualTlsHandler = new WebRequestHandler();
            mutualTlsHandler.ClientCertificates.AddRange(certificateCollection);
            mutualTlsHandler.ServerCertificateValidationCallback = IsValidServerCertificate;

            return mutualTlsHandler;
        }

        private bool IsValidServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            return CertificateValidator.IsValidServerCertificate(ClientConfiguration.Environment.Sertifikatkjedevalidator, new X509Certificate2(certificate), ClientConfiguration.ServerCertificateOrganizationNumber);
        }
    }
}