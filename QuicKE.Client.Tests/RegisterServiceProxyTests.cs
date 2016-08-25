using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;


namespace QuicKE.Client.Tests
{
    [TestClass]
    public class RegisterServiceProxyTests
    {
        [TestInitialize]
        public async Task Setup()
        {
            await MFundiRuntime.Start("Tests");
            // set...
            ServiceProxyFactory.Current.SetHandler(typeof(IRegisterServiceProxy),
                typeof(FakeRegisterServiceProxy));
        }

        [TestMethod]
        public async Task TestRegisterOk()
        {
            var proxy = ServiceProxyFactory.Current.GetHandler<IRegisterServiceProxy>();
            // ok...
            var result = await proxy.RegisterAsync("test", "254712704404","test@gmail.com", "123456");
            Assert.IsFalse(result.HasErrors);
        }
    }
}
