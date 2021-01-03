using System;
using System.Collections.Generic;
using Akavache;
using Camelotia.Services.Interfaces;
using Camelotia.Services.Models;
using Camelotia.Services.Providers;

namespace Camelotia.Services
{
    public sealed class CloudFactory : ICloudFactory
    {
        private readonly IAuthenticator _authenticator;
        private readonly IBlobCache _cache;

        public CloudFactory(
            IAuthenticator authenticator,
            IBlobCache cache,
            IReadOnlyCollection<CloudType> supported = null)
        {
            _cache = cache;
            _authenticator = authenticator;
            SupportedClouds = supported ?? new[]
            {
                CloudType.Local,
                CloudType.Ftp,
                CloudType.Sftp,
                CloudType.Yandex,
                CloudType.GitHub,
                CloudType.GoogleDrive,
                CloudType.VkDocs
            };
        }

        public IReadOnlyCollection<CloudType> SupportedClouds { get; }

        public ICloud CreateCloud(CloudParameters parameters) => parameters.Type switch
        {
            CloudType.Ftp => new FtpCloud(parameters),
            CloudType.GitHub => new GitHubCloud(parameters),
            CloudType.GoogleDrive => new GoogleDriveCloud(parameters, _cache),
            CloudType.Local => new LocalCloud(parameters),
            CloudType.Sftp => new SftpCloud(parameters),
            CloudType.VkDocs => new VkDocsCloud(parameters),
            CloudType.Yandex => new YandexDiskCloud(parameters, _authenticator),
            _ => throw new ArgumentOutOfRangeException(nameof(parameters))
        };
    }
}