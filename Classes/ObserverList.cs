using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Classes
{
    public class AlarmObserverList<T>
        where T : class
    {
        private readonly ObservableCollection<T> list;
        public AlarmObserverList()
        {
            list = new ObservableCollection<T>();
            list.CollectionChanged += listChanged;
        }

        private void listChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }
    }
}
