﻿namespace Digipost.Signature.Api.Client.Core.Tests.Smoke
{
    public class SmokeTests
    {
        public static Client Endpoint => Client.Localhost;

        public enum Client
        {
            Localhost,
            DifiTest,
            DifiQa,
            Test,
            Qa
        }
    }
}