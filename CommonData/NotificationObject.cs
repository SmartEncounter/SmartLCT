using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Nova.SmartLCT.Interface
{
    public class NotificationST : INotifyPropertyChanged
    {

        public string GetPropertyName<T>(Expression<Func<NotificationST, T>> expr)
        {
            var name = ((MemberExpression)expr.Body).Member.Name;
            return name;
        }

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }

}
