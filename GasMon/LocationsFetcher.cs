﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;

namespace GasMon
{
    interface ILocationsFetcher
    {
        List<Location> LocationListMaker();
    }

    public class LocationsFetcher : ILocationsFetcher
    {
        private static readonly string BucketName = "gasmonitoring-locationss3bucket-pgef0qqmgwba";
        private const string KeyName = "locations.json";
        private static readonly RegionEndpoint BucketRegion = RegionEndpoint.EUWest2;
        private static IAmazonS3 _client;

        private static async Task<string> ReadObjectDataAsync(IAmazonS3 client)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = BucketName,
                Key = KeyName
            };
            using GetObjectResponse response = await client.GetObjectAsync(request);
            await using Stream responseStream = response.ResponseStream;
            using StreamReader reader = new StreamReader(responseStream);
            var responseBody = reader.ReadToEnd();
            return responseBody;
        }

        public List<Location> LocationListMaker()
        {
            _client = new AmazonS3Client(BucketRegion);
            var returnedData = ReadObjectDataAsync(_client).Result;
            return JsonConvert.DeserializeObject<List<Location>>(returnedData);
        }
    }
}