using FW.Common.Extensions;
using FW.Entities;
using FW.Models.ViewModel;
using FW.WebCore.Core;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WebApiTests
{
    public class RoleControllerTest
    {
        const string _testUrl = "/role/";
        const string _mediaType = "application/json";
        readonly Encoding _encoding = Encoding.UTF8;

        [Theory]
        [InlineData(1222538617050763264)]
        public async Task Delete_Id_ReturnResult( long id )
        {
            string url = $"{_testUrl}?id={id.ToString()}";
            using var host = await TestHostBuild.GetTestHost().StartAsync();//启动TestServer

            var response = await host.GetTestClientWithToken().DeleteAsync(url);
            var result = (await response.Content.ReadAsStringAsync()).GetDeserializeObject<ApiResult<ExecuteResult>>();

            Assert.Equal(result.data.IsSucceed, string.IsNullOrWhiteSpace(result.data.Message));
        }

        [Fact]
        public async Task Post_CreateRole_ReturnTrue()
        {
            RoleReq viewModel = new RoleReq
            {
                Name = "RoleForPostTest",
                DisplayName = "RoleForPostTest"
            };
            StringContent content = new StringContent(viewModel.ToJsonString(), _encoding, _mediaType);//定义post传递的参数、编码和类型
            using var host = await TestHostBuild.GetTestHost().StartAsync();//启动TestServer

            //act
            var response = await host.GetTestClientWithToken().PostAsync(_testUrl, content); //调用Post接口
            var result = (await response.Content.ReadAsStringAsync()).GetDeserializeObject<ApiResult<ExecuteResult<Role>>>();//获得返回结果并反序列化

            //assert
            Assert.True(result.data.IsSucceed);

            //测完把添加的删除
            await Delete_Id_ReturnResult(result.data.Result.Id);
        }

        [Fact]
        public async Task Put_UpdateRole_ReturnTrue()
        {
            RoleReq viewModel = new RoleReq
            {
                Name = "RoleForPutTest",
                DisplayName = "RoleForPutTest"
            };
            StringContent content = new StringContent(viewModel.ToJsonString(), _encoding, _mediaType);
            using var host = await TestHostBuild.GetTestHost().StartAsync();
            var response = await host.GetTestClientWithToken().PostAsync(_testUrl, content);
            viewModel.Id = (await response.Content.ReadAsStringAsync()).GetDeserializeObject<ApiResult<ExecuteResult<Role>>>().data.Result.Id;
            content = new StringContent(viewModel.ToJsonString(), _encoding, _mediaType);

            response = await host.GetTestClientWithToken().PutAsync(_testUrl, content);

            var result = (await response.Content.ReadAsStringAsync()).GetDeserializeObject<ApiResult<ExecuteResult>>();

            Assert.True(result.data.IsSucceed);

            //测完把添加的删除
            await Delete_Id_ReturnResult(viewModel.Id);
        }
    }
}
