using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Ao.Core
{
    public static class ObservableCollectionExtensions
    {
        public static void Sort<T, TSort>(this ObservableCollection<T> collection,Func<T,TSort> func,bool desc=true)
        {
            List<T> sortedList = null;
            if (desc)
            {
                sortedList = collection.OrderByDescending(x => func(x)).ToList();//这里用降序
            }
            else
            {
                sortedList = collection.OrderBy(x => func(x)).ToList();
            }
            for (int i = 0; i < sortedList.Count; i++)
            {
                collection.Move(collection.IndexOf(sortedList[i]), i);
            }
        }
    }
}
