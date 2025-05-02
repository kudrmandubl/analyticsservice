using Modules.AnalyticsService.Interfaces;
using Modules.AnalyticsService.Models;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Text;
using UnityEngine;

namespace Modules.AnalyticsService.Implementation
{
    /// <summary>
    /// Сущность для отправки аналитики, используя HttpClient.
    /// Один из возможных вариантов реализации <see cref="IAnalyticsSender" />
    /// </summary>
    public class AnalyticsSender : IAnalyticsSender
    {
        private SendConfig _sendConfig;
        private HttpClient _httpClient;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="sendConfig">Конфиг отправки</param>
        public AnalyticsSender(SendConfig sendConfig)
        {
            _sendConfig = sendConfig;

            _httpClient = new HttpClient();
        }

        ///  <inheritdoc />
        public async Task<bool> SendMessageAsync(Message message)
        {
            try
            {
                var encoding = Encoding.UTF8;

                var content = new StringContent(message.Content, encoding, _sendConfig.ContentType);

                if (_sendConfig.TestUnsuccess)
                {
                    throw new HttpRequestException();
                }

                var response = await _httpClient.PostAsync(_sendConfig.Url, content);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new HttpRequestException();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return false;
            }

            return true;
        }
    }
}
