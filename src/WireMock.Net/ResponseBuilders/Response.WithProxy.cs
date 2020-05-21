using System.Net.Http;
using WireMock.Http;
using WireMock.Settings;
using WireMock.Validation;

namespace WireMock.ResponseBuilders
{
    public partial class Response
    {
        private HttpClient _httpClientForProxy;

        /// <summary>
        /// The Proxy URL to use.
        /// </summary>
        public string ProxyUrl { get; private set; }
        public string PrefixToRemoveFromLocalPath { get; private set; }
        /// <summary>
        /// The WebProxy settings.
        /// </summary>
        public IWebProxySettings WebProxySettings { get; private set; }

        /// <inheritdoc cref="IProxyResponseBuilder.WithProxy(string, string)"/>
        public IResponseBuilder WithProxy(string proxyUrl, string clientX509Certificate2ThumbprintOrSubjectName = null, string prefixToRemoveFromLocalPath = null)
        {
            Check.NotNullOrEmpty(proxyUrl, nameof(proxyUrl));

            var settings = new ProxyAndRecordSettings
            {
                Url = proxyUrl,
                ClientX509Certificate2ThumbprintOrSubjectName = clientX509Certificate2ThumbprintOrSubjectName
            };

            return WithProxy(settings, prefixToRemoveFromLocalPath);
        }

        /// <inheritdoc cref="IProxyResponseBuilder.WithProxy(IProxyAndRecordSettings)"/>
        public IResponseBuilder WithProxy(IProxyAndRecordSettings settings, string prefixToRemoveFromLocalPath = null)
        {
            Check.NotNull(settings, nameof(settings));

            ProxyUrl = settings.Url;
            WebProxySettings = settings.WebProxySettings;
            PrefixToRemoveFromLocalPath = prefixToRemoveFromLocalPath;

            _httpClientForProxy = HttpClientHelper.CreateHttpClient(settings);
            return this;
        }
    }
}