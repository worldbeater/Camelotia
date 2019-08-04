﻿using Camelotia.Presentation.Interfaces;
using Camelotia.Presentation.ViewModels;
using Camelotia.Presentation.Wpf.Services;
using Camelotia.Services.Interfaces;
using Camelotia.Services.Providers;
using Camelotia.Services.Storages;
using Camelotia.Services.Models;
using System.Reactive.Concurrency;
using System.Collections.Generic;
using ReactiveUI;
using System;

namespace Camelotia.Presentation.Wpf
{
    public sealed class Bootstrapper
    {
        public IMainViewModel MainViewModel { get; }

        public Bootstrapper()
        {
            var current = CurrentThreadScheduler.Instance;
            var main = RxApp.MainThreadScheduler;

            Akavache.BlobCache.ApplicationName = "Camelotia";
            var cache = Akavache.BlobCache.UserAccount;
            var login = new WindowsPresentationYandexAuthenticator();
            var files = new WindowsPresentationFileManager();

            MainViewModel = new MainViewModel(
                (provider, auth) => new ProviderViewModel(
                    model => new CreateFolderViewModel(model, provider, current, main),
                    model => new RenameFileViewModel(model, provider, current, main),
                    (file, model) => new FileViewModel(model, file),
                    auth, files, provider, current, main
                ),
                provider => new AuthViewModel(
                    new DirectAuthViewModel(provider, current, main),
                    new HostAuthViewModel(provider, current, main),
                    new OAuthViewModel(provider, current, main),
                    provider, current, main
                ),
                new ProviderStorage(
                    new Dictionary<string, Func<ProviderModel, IProvider>>
                    {
                        ["Local Storage"] = id => new LocalProvider(id),
                        ["Yandex Disk"] = id => new YandexDiskProvider(id, login, cache),
                        ["Vkontakte Docs"] = id => new VkDocsProvider(id, cache),
                        ["FTP"] = id => new FtpProvider(id),
                        ["SFTP"] = id => new SftpProvider(id),
                        ["GitHub"] = id => new GitHubProvider(id, cache)
                    },
                    cache
                ),
                current, main
            );
        }
    }
}
