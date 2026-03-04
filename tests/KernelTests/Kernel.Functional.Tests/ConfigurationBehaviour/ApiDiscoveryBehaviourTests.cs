using Api;
using NUnit.Framework.Internal;
using TestShared.Fixtures;


namespace Kernel.Functional.Tests.ConfigurationBehaviour
{
    [Category(TestingCategoryConstants.ApiStartupTests)]
    [NonParallelizable]
    public class ApiDiscoveryBehaviourTests : SharedContextTestFixture
    {
        [Test]
        [TestCase(true, System.Net.HttpStatusCode.OK)]
        [TestCase(false, System.Net.HttpStatusCode.NotFound)]
        public async Task ApiDiscovery_In_AppSettings_Toggle_Scalar_And_OpenApi_Endpoints(bool enableApiDiscovery, System.Net.HttpStatusCode httpStatusCode)
        {
            //Arrange
            Action<AppSettings>? appSettingConfigurationAction = o => o.EnableApiDiscovery = enableApiDiscovery;
            await TestingContext.ResetTestContextAsync(TestingConstants.TestingEnvironmentName, appSettingConfigurationAction);
            var httpClient = TestingContext.GetHttpClient();
            var appSettings = TestingContext.GetAppSettings();
            var scalarUrl = new Uri(appSettings.GetApplicationBaseUrlAsUri, "scalar");
            var openApiJsonUrl = new Uri(appSettings.GetApplicationBaseUrlAsUri, "openapi/v1.json");

            //Act
            var scalarReponse = await httpClient.GetAsync(scalarUrl.AbsoluteUri);
            var openApireponse = await httpClient.GetAsync(openApiJsonUrl.AbsoluteUri);

            using (Assert.EnterMultipleScope())
            {
                //Assert
                Assert.That(appSettings.EnableApiDiscovery, Is.EqualTo(enableApiDiscovery));
                Assert.That(scalarReponse.StatusCode, Is.EqualTo(httpStatusCode));
                Assert.That(openApireponse.StatusCode, Is.EqualTo(httpStatusCode));
            }
        }
    }
}
