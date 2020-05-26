using System.Windows;

namespace Ao.Wpf.ViewModels
{
    /// <summary>
    /// 数组视图适配器
    /// </summary>
    public interface IArrayViewAdapter
    {
        object MakeObject(ArrayViewAdapterContext context);

        UIElement GetView(object obj,ArrayViewAdapterContext context);
    }
}
