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
        private List<ToastMessageModel> _messages;
        public List<ToastMessageModel> Messages
        {
            get => _messages;
        }
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

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
            NotifyStateChanged();
        }

        private void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Timer tick");
            var tempMessage = _messages.ToList();
            var isWasChanged = false;
            foreach (var message in tempMessage)
            {
                var timeDifference = DateTime.Now - message.CreatedAt;
                if (timeDifference.Seconds >= 5)
                {
                    _messages.Remove(message);
                    isWasChanged = true;
                }
            }

            if (isWasChanged)
            {
                NotifyStateChanged();
            }
        }
    }
}