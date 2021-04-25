using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using OffLogs.Web.Core.Constants;
using OffLogs.Web.Core.Models.Toast;

namespace OffLogs.Web.Services
{
    public class ToastService
    {
        private List<ToastMessageModel> _messages = new ();
        public List<ToastMessageModel> Messages
        {
            get => _messages;
        }
        public event Action OnListUpdated;
        public event Action<ToastMessageModel> OnRemoveMessage;
        private void NotifyListChanged() => OnListUpdated?.Invoke();
        private void NotifyMessageRemove(ToastMessageModel messageModel) => OnRemoveMessage?.Invoke(messageModel);

        private readonly Timer _timer;
        private readonly double _interval = 1 * 1000;
        
        public ToastService()
        {
            _timer = new Timer();
            _timer.Interval = _interval;
            _timer.Elapsed += OnTimerTick;
            _timer.Start();
        }

        public void AddMessage(ToastMessageType type, string title, string text = null)
        {
            _messages.Add(new ToastMessageModel()
            {
                Type = type,
                Title = title,
                Content = text,
                CreatedAt = DateTime.Now
            });
            NotifyListChanged();
        }
        
        public void RemoveMessage(ToastMessageModel messageModel)
        {
            _messages.Remove(messageModel);
            NotifyListChanged();
        }

        private void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            var tempMessage = _messages.ToList();
            foreach (var message in tempMessage)
            {
                var timeDifference = DateTime.Now - message.CreatedAt;
                if (timeDifference.Seconds >= 5)
                {
                    NotifyMessageRemove(message);
                }
            }
        }
    }
}