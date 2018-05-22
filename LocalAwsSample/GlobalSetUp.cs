using System.IO;
using Amazon;
using NUnit.Framework;

namespace LocalAwsSample
{
	[SetUpFixture]
	public class GlobalSetUp
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			if (!string.IsNullOrEmpty(AWSConfigs.EndpointDefinition))
			{
				if (AWSConfigs.EndpointDefinition.Equals(new FileInfo(AWSConfigs.EndpointDefinition).Name))
				{
					AWSConfigs.EndpointDefinition =
						Path.Combine(TestContext.CurrentContext.TestDirectory, AWSConfigs.EndpointDefinition);
				}
			}
		}
	}
}
