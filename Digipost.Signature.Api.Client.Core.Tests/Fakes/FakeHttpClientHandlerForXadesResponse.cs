﻿using System.Net.Http;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Portal.Tests.Fakes;

namespace Digipost.Signature.Api.Client.Core.Tests.Fakes
{
    internal class FakeHttpClientHandlerForXadesResponse : FakeHttpClientHandlerResponse
    {
        public override HttpContent GetContent()
        {
            return new StreamContent(CoreResponseUtility.GetXades());
        }
    }
}
