using Assets.Modules.AnalyticsService.Example.Scripts;
using Modules.AnalyticsService.Example;
using Modules.AnalyticsService.Interfaces;
using Modules.AnalyticsService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Modules.AnalyticsService.Implementation
{
    /// <summary>
    /// Сервис для отправки аналитики.
    /// Один из возможных вариантов реализации <see cref="IAnalyticsService" />
    public class AnalyticsService : IAnalyticsService
    {
        private IAnalyticsSaver _saver;
        private IAnalyticsSendConfigLoader _configLoader;
        private IAnalyticsSender _sender;

        private float _resendTime;
        private float _resendTimer;
        private bool _isReseneding;

        /// <summary>
        /// Конструктор
        /// </summary>
        public AnalyticsService()
        {
            _saver = new AnalyticsSaver();
            _configLoader = new AnalyticsSendConfigLoader();
            var config = _configLoader.GetConfig();
            _sender = new AnalyticsSender(config);

            _resendTime = config.ResendTime;

            // можно добавить метод уничтожения сервиса и отписки
            ExampleMonoBehaviourCycle.OnApplicationFocusChange += SaveOnUnfocus;
            ExampleMonoBehaviourCycle.OnUpdate += CountdownToResened;
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
            if (_isReseneding)
            {
                _saver.AddUnsentMessage(message);
            }
            else
            {
                await SendMessageAsync(message);
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
        private void CountdownToResened()
        {
            if (!_saver.GetFirstUnsentMessage().HasValue || _isReseneding)
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
            _isReseneding = true;
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
            _isReseneding = false;
        }
    }
}