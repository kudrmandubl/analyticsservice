using Modules.AnalyticsService.Interfaces;
using Modules.AnalyticsService.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Modules.AnalyticsService.Implementation
{
    /// <summary>
    /// Сущность для сохранения списка неотправленных сообщений, используя файловую систему.
    /// Один из возможных вариантов реализации <see cref="IAnalyticsSaver" />
    /// </summary>
    public class AnalyticsSaver : IAnalyticsSaver
    {
        private const string SaveFileName = "analyticsUnsent.json";

        private UnsentMessages _unsentMessages;
        private string _savePath;
        
        /// <summary>
        /// Конструктор
        /// Можно переписать на использование DI
        /// </summary>
        public AnalyticsSaver()
        {
            _unsentMessages = new UnsentMessages();
            _unsentMessages.Messages = new LinkedList<Message>();
            _savePath = $"{Application.persistentDataPath}/{SaveFileName}";
            Load();
        }

        /// <inheritdoc />
        public void AddUnsentMessage(Message message)
        {
            _unsentMessages.Messages.AddLast(message);
        }

        /// <inheritdoc />
        public void RemoveUnsentMessage(Message message)
        {
            if (!Contains(message))
            {
                return;
            }
            _unsentMessages.Messages.Remove(message);
        }

        /// <inheritdoc />
        public Message? GetFirstUnsentMessage()
        {
            return _unsentMessages.Messages.First?.Value;
        }

        /// <inheritdoc />
        public bool Contains(Message message)
        {
            return _unsentMessages.Messages.Contains(message);
        }

        /// <inheritdoc />
        public void Save()
        {
            using (StreamWriter streamWriter = new StreamWriter(_savePath))
            {
                string saveDataString = JsonConvert.SerializeObject(_unsentMessages);
                streamWriter.Write(saveDataString);
            }
        }

        /// <inheritdoc />
        public void Load()
        {
            if (!File.Exists(_savePath))
            {
                return;
            }

            using (StreamReader streamReader = new StreamReader(_savePath))
            {
                string savedDataString = streamReader.ReadToEnd();
                var savedMessages = JsonConvert.DeserializeObject<UnsentMessages>(savedDataString);
                // после загрузки добавляем последовательно записи в unsentMessages,
                // на случай, если до загрузки были добавлены неотправленные сообщения
                // перебор с конца в начало
                var nextMessage = savedMessages?.Messages?.Last;
                while (nextMessage != null)
                {
                    _unsentMessages.Messages.AddFirst(nextMessage.Value);
                    nextMessage = nextMessage.Previous;
                }
            }
        }
    }
}
