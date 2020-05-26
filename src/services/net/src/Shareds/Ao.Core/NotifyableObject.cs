using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;

namespace Ao.Core
{
    public abstract class NotifyableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 通知属性更改了
        /// </summary>
        /// <param name="name"></param>
        public virtual void RaisePropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        }
        /// <summary>
        /// 交互值并<inheritdoc cref="RaisePropertyChanged(string)"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        /// <param name="name"></param>
        public virtual void RaisePropertyChanged<T>(ref T prop, T value, [CallerMemberName] string name = "")
        {
            prop = value;
            RaisePropertyChanged(name);
        }
    }
}
