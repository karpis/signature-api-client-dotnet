﻿using System;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
using Schemas;

namespace Digipost.Signature.Api.Client.Direct.Internal
{
    internal class CreateAction : Core.Internal.CreateAction
    {
        public static readonly Func<IRequestContent, string> SerializeFunc = content => SerializeUtility.Serialize(DataTransferObjectConverter.ToDataTransferObject((Job) content));
        public static readonly Func<string, JobResponse> DeserializeFunc = content => new JobResponse(SerializeUtility.Deserialize<directsignaturejobresponse>(content));

        public CreateAction(Job job, DocumentBundle documentBundle)
            : base(job, documentBundle, SerializeFunc)
        {
        }
    }
}
