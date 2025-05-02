using Modules.AnalyticsService.Interfaces;
using Modules.AnalyticsService.Models;
using Modules.Common.Interfaces;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Modules.AnalyticsService.Implementation
{
    /// <summary>
    /// Сервис для отправки аналитики.
    /// Один из возможных вариантов реализации <see cref="IAnalyticsService" />
    public class AnalyticsService : IAnalyticsService, IDisposable
    {
        private IAnalyticsSaver _saver;
        private IAnalyticsSendConfigLoader _configLoader;
        private IAnalyticsSender _sender;

        private IMonoBehaviourCycle _monoBehaviourCycle;

        private float _resendTime;
        private float _resendTimer;
        private bool _isResending;


        /// <summary>
        /// Конструктор
        /// </summary>
        [Inject]
        public AnalyticsService(IMonoBehaviourCycle monoBehaviourCycle)
        {
            _monoBehaviourCycle = monoBehaviourCycle;

            _saver = new AnalyticsSaver();
            _configLoader = new AnalyticsSendConfigLoader();
            var config = _configLoader.GetConfig();
            _sender = new AnalyticsSender(config);

            _resendTime = config.ResendTime;

            monoBehaviourCycle.SubscribeToApplicationFocus(SaveOnUnfocus);
            monoBehaviourCycle.SubscribeToUpdate(CountdownToResend);

            SendMessageAsync("AnalyticsService successfully inited");
        }

        /// <inheritdoc />
        public async Task SendMessageAsync(string eventData)
        {
            var message = new Message()
            {
                SendDateTime = DateTime.Now,
                Content = eventData,
            };

            // если идёт переотправка - добавляем сообщение в конец очереди
            if (_isResending)
            {
                _saver.AddUnsentMessage(message);
            }
            else
            {
                await SendMessageAsync(message);
            }
        }

        /// <summary>
        /// Высвобождение
        /// </summary>
        public void Dispose()
        {
            if (_monoBehaviourCycle != null)
            {
                _monoBehaviourCycle.UnsubscribeFromApplicationFocus(SaveOnUnfocus);
                _monoBehaviourCycle.UnsubscribeFromUpdate(CountdownToResend);
            }
        }

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns>Успешность</returns>
        private async Task<bool> SendMessageAsync(Message message)
        {
            var success = await _sender.SendMessageAsync(message);

            if (!success && !_saver.Contains(message))
            {
                _saver.AddUnsentMessage(message);
            }
            else if (success && _saver.Contains(message))
            {
                _saver.RemoveUnsentMessage(message);
            }
            return success;
        }

        /// <summary>
        /// Сохранение на анфокусе
        /// </summary>
        /// <param name="focus">Фокус</param>
        private void SaveOnUnfocus(bool focus)
        {
            if (focus)
            {
                return;
            }
            _saver.Save();
        }

        /// <summary>
        /// Отчёт для переотправки
        /// </summary>
        private void CountdownToResend()
        {
            if (!_saver.GetFirstUnsentMessage().HasValue || _isResending)
            {
                return;
            }
            _resendTimer += Time.deltaTime;
            if(_resendTimer >= _resendTime)
            {
                _resendTimer = 0;
                ResendUnsentMessages();
            }
        }

        /// <summary>
        /// Переотправка неотправленных сообщений
        /// </summary>
        private async Task ResendUnsentMessages()
        {
            _isResending = true;
            var message = _saver.GetFirstUnsentMessage();
            while (message.HasValue)
            {
                var success = await SendMessageAsync(message.Value);
                if (!success)
                {
                    break;
                }
                message = _saver.GetFirstUnsentMessage();
            }
            _isResending = false;
        }
    }
}