using System;

namespace OffLogs.Web.Core.Storage
{
    public class AppStateContainer
    {
        #region Common

        private string _savedString;
        public string Property
        {
            get => _savedString;
            set
            {
                _savedString = value;
                NotifyStateChanged();
            }
        }
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        #endregion
    }
}