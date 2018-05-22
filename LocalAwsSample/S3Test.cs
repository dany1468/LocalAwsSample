using System;
using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using NUnit.Framework;

namespace LocalAwsSample
{
	[TestFixture]
	public class S3Test
	{
		[Test]
		public void Test()
		{
			const string bucketName = "sample_bucket";
			const string key = "sample";

			using (var client = Client())
			{
				if (!AmazonS3Util.DoesS3BucketExist(client, bucketName))
				{
					var putRequest1 = new PutBucketRequest
					{
						BucketName = bucketName,
						UseClientRegion = true
					};

					client.PutBucket(putRequest1);
				}

				const string objectBody = "test sample";

				var putRequest = new PutObjectRequest
				{
					BucketName = bucketName,
					Key = key,
					ContentBody = objectBody
				};
				client.PutObject(putRequest);

				var request = new GetObjectRequest
				{
					BucketName = bucketName,
					Key = key
				};

				using (var response = client.GetObject(request))
				using (var responseStream = response.ResponseStream)
				using (var reader = new StreamReader(responseStream))
				{
					var body = reader.ReadToEnd();
					Assert.That(body, Is.EqualTo(objectBody));
				}
			}
		}


		private static IAmazonS3 Client()
		{
			return new AmazonS3Client(new AmazonS3Config {UseHttp = true});
		}

		// config での設定をせず、コードで設定する場合は AmazonS3Config に URL と region を指定できる
//		private static IAmazonS3 Client()
//		{
//			var config = new AmazonS3Config {ServiceURL = "http://localhost:5000", AuthenticationRegion = "us-east-1"};
//			return new AmazonS3Client("foo", "foo", config);
//		}

		// profile を使う場合
//		private static IAmazonS3 Client()
//		{
//			var sharedFile = new SharedCredentialsFile();
//			if (sharedFile.TryGetProfile("my-profile", out var basicProfile) &&
//			    AWSCredentialsFactory.TryGetAWSCredentials(basicProfile, sharedFile, out var awsCredentials))
//			{
//				return new AmazonS3Client(awsCredentials, basicProfile.Region);
//			}
//
//			return null;
//		}
	}
}
