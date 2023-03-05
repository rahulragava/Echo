using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View.ProfileView
{
    public sealed partial class FollowerUserControl : UserControl
    {
        
        public FollowerUserControl()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty FollowerIdProperty = DependencyProperty.Register(
            nameof(FollowerId), typeof(string), typeof(FollowerUserControl), new PropertyMetadata(default(string)));

        public string FollowerId
        {
            get => (string)GetValue(FollowerIdProperty);
            set => SetValue(FollowerIdProperty, value);
        }
    }
}
