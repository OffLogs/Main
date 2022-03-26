using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Fluxor;
using OffLogs.Web.Constants;
using OffLogs.Web.Core.Models.Toast;
using OffLogs.Web.Store.Shared.Toast.Actions;

namespace OffLogs.Web.Services
{
    public class ToastService: IDisposable
    {
        private readonly IDispatcher _dispatcher;
        private readonly Timer _timer;
        private readonly double _interval = 1 * 1000;
        
        public ToastService(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _timer = new Timer();
        }

        public void Start()
        {
            _timer.Interval = _interval;
            _timer.Elapsed += OnTimerTick;
            _timer.Start();
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
        
        public void AddServerErrorMessage(string text = null)
        {
            AddErrorMessage("Server error", text);
        }
        
        public void AddErrorMessage(string title, string text = null)
        {
            AddMessage(ToastMessageType.Error, title, text);
        }
        
        public void AddInfoMessage(string title, string text = null)
        {
            AddMessage(ToastMessageType.Info, title, text);
        }
        
        public void AddMessage(ToastMessageType type, string title, string text = null)
        {
            _dispatcher.Dispatch(new AddMessageAction(type, title, text));
        }

        private void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            _dispatcher.Dispatch(new RemoveMessageAction());
        }
    }
}
