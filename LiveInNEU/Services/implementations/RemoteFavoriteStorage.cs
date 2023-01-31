using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using LiveInNEU.Confidential;
using LiveInNEU.Models;
using LiveInNEU.Utils;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using ICSharpCode.SharpZipLib.Core;

namespace LiveInNEU.Services.implementations {
    public class RemoteFavoriteStorage : IRemoteFavoriteStorage {
        /******** 公开变量 ********/

        /******** 私有变量 ********/

        private IAlertService alertService;

        private const string Server = "OneDrive服务器";

        private string[] scopes = OneDriveOAuthSettings.Scopes.Split(' ');
        private IPublicClientApplication pca;
        private GraphServiceClient graphClient;
        private IInfoStorage infoStorage;

        private static string path ;

        public RemoteFavoriteStorage(IAlertService alertService, IInfoStorage infoStorage) {
            this.alertService = alertService;
            this.infoStorage = infoStorage;
            var builder =
                PublicClientApplicationBuilder.Create(OneDriveOAuthSettings
                    .ApplicationId);

            if (!string.IsNullOrEmpty(App.iOSKeychainSecurityGroup)) {
                builder =
                    builder.WithIosKeychainSecurityGroup(
                        App.iOSKeychainSecurityGroup);
            }
            pca = builder.Build();
            graphClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(async (requestMessage) => {
                    var accounts = await pca.GetAccountsAsync();

                    var result = await pca
                        .AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                        .ExecuteAsync();

                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer",
                            result.AccessToken);
                }));
        }

        /// <summary>
        /// 是否已登录。
        /// </summary>
        public async Task<bool> IsSignedInAsync() {
            Status = "正在检查OneDrive登录状态";
            string accessToken = string.Empty;
            try {
                var accounts = await pca.GetAccountsAsync();
                if (accounts.Any()) {
                    var silentAuthResult = await pca
                        .AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                        .ExecuteAsync();
                    accessToken = silentAuthResult.AccessToken;
                }
            } catch (MsalUiRequiredException) {
                return false;
            }
            return !string.IsNullOrEmpty(accessToken);
        }

        /// <summary>
        /// 登录。
        /// </summary>
        public async Task<bool> SignInAsync() {
            Status = "正在登录OneDrive";
            path = "/" + await infoStorage.Get(LessonStorageConstants.ACCOUNT_KEY,
                "username") + ".zip";
            try {
                var interactiveRequest = pca.AcquireTokenInteractive(scopes);

                if (App.AuthUIParent != null) {
                    interactiveRequest = interactiveRequest
                        .WithParentActivityOrWindow(App.AuthUIParent);
                }

                await interactiveRequest.ExecuteAsync();
            } catch (MsalClientException e) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 注销。
        /// </summary>
        public async Task SignOutAsync() {
            Status = "正在退出OneDrive登录";

            var accounts = await pca.GetAccountsAsync();
            while (accounts.Any()) {
                await pca.RemoveAsync(accounts.First());
                accounts = await pca.GetAccountsAsync();
            }
        }

        /// <summary>
        /// 状态。
        /// </summary>
        public string Status {
            get => _status;
            set {
                _status = value;
                StatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private string _status;

        /// <summary>
        /// 状态改变事件。
        /// </summary>
        public event EventHandler StatusChanged;



        public async Task<ServiceResult> SaveFavoriteItemsAsync(
            IList<Store> favoriteList) {
            Status = "正在压缩远程收藏项";

            var json = JsonConvert.SerializeObject(favoriteList);

            MemoryStream fileStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(fileStream);
            zipStream.SetLevel(3);

            ZipEntry newEntry = new ZipEntry("Store.json");
            newEntry.DateTime = DateTime.Now;
            zipStream.PutNextEntry(newEntry);

            var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            StreamUtils.Copy(jsonStream, zipStream, new byte[1024]);
            jsonStream.Close();
            zipStream.CloseEntry();
            zipStream.IsStreamOwner = false;
            zipStream.Close();

            Status = "正在上传远程收藏项";
            fileStream.Position = 0;
            try {
                await graphClient.Me.Drive.Root
                    .ItemWithPath(path).Content.Request()
                    .PutAsync<DriveItem>(fileStream);
            } catch (ServiceException e) {
                await alertService.ShowAlertAsync(
                    ErrorMessages.HTTP_CLIENT_ERROR_TITLE,
                    ErrorMessages.HttpClientErrorMessage(Server, e.Message),
                    ErrorMessages.HTTP_CLIENT_ERROR_BUTTON);
                return new ServiceResult {
                    Status = ServiceResultStatus.Exception, Message = e.Message
                };
            } finally {
                fileStream.Close();
            }

            return new ServiceResult { Status = ServiceResultStatus.Ok };
        }


        public async Task<IList<Store>> GetFavoriteItemsAsync()
        {
            var rootChildren = await graphClient.Me.Drive.Root.Children
                .Request().GetAsync();
            if (!rootChildren.Any(p => p.Name == path))
            {
                return new List<Store>();
            }
            var fileStream = await graphClient.Me.Drive.Root
                .ItemWithPath(path).Content.Request().GetAsync();

            ZipInputStream zipStream = new ZipInputStream(fileStream);
            ZipEntry zipEntry = zipStream.GetNextEntry();
            if (zipEntry == null)
            {
                return new List<Store>();
            }
            byte[] buffer = new byte[1024];
            var jsonStream = new MemoryStream();
            StreamUtils.Copy(zipStream, jsonStream, buffer);
            zipStream.Close();
            fileStream.Close();

            jsonStream.Position = 0;
            var jsonReader = new StreamReader(jsonStream);
            var favoriteList =
                JsonConvert.DeserializeObject<IList<Store>>(
                    await jsonReader.ReadToEndAsync());
            jsonReader.Close();
            jsonStream.Close();
            return favoriteList?.ToList() ?? new List<Store>();
        }




    }

}