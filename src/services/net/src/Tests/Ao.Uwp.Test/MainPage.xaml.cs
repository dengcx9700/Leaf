using Ao.Shared.ForView;
using PkgEditor.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Ao.Uwp.Test
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var sb = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("dsadas"), AssemblyBuilderAccess.Run);
            var mb = sb.DefineDynamicModule("dsadsa");

            var fb =ClassBuilder.FromInfos(new TypeItem("dsadsa", "var")
            {
                Items =
                {
                    new PropertyItem{ Name="AAA", Type=typeof(int)}
                }
            },mb);
            var t = fb.BuildType();
        }
    }
}
