﻿#pragma checksum "..\..\..\view\GetPositionPanel.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "17B726B484194C90309FF663C6D25C7D432C4FCB"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

using AutoClicker;
using AutoClicker.view;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace AutoClicker.view {
    
    
    /// <summary>
    /// GetPositionPanel
    /// </summary>
    public partial class GetPositionPanel : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\view\GetPositionPanel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel GetPositionMain;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\view\GetPositionPanel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox GPTChk;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\view\GetPositionPanel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AutoClicker.NumericTextBox RatioEdit;
        
        #line default
        #line hidden
        
        
        #line 130 "..\..\..\view\GetPositionPanel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid GetPositionSub;
        
        #line default
        #line hidden
        
        
        #line 131 "..\..\..\view\GetPositionPanel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AutoClicker.view.PositionTuner PositionTuner;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AutoClicker;component/view/getpositionpanel.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\view\GetPositionPanel.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.GetPositionMain = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this.GPTChk = ((System.Windows.Controls.CheckBox)(target));
            
            #line 19 "..\..\..\view\GetPositionPanel.xaml"
            this.GPTChk.Checked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked);
            
            #line default
            #line hidden
            
            #line 19 "..\..\..\view\GetPositionPanel.xaml"
            this.GPTChk.Unchecked += new System.Windows.RoutedEventHandler(this.CheckBox_Unchecked);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 51 "..\..\..\view\GetPositionPanel.xaml"
            ((System.Windows.Documents.Hyperlink)(target)).Click += new System.Windows.RoutedEventHandler(this.Btn_ShowPos);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 62 "..\..\..\view\GetPositionPanel.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Btn_Adjust);
            
            #line default
            #line hidden
            return;
            case 5:
            this.RatioEdit = ((AutoClicker.NumericTextBox)(target));
            return;
            case 6:
            
            #line 71 "..\..\..\view\GetPositionPanel.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.PresetRatio_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 72 "..\..\..\view\GetPositionPanel.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.PresetRatio_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 73 "..\..\..\view\GetPositionPanel.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.PresetRatio_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 74 "..\..\..\view\GetPositionPanel.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.PresetRatio_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 75 "..\..\..\view\GetPositionPanel.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.PresetRatio_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 114 "..\..\..\view\GetPositionPanel.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Btn_ClearTargetWinPos);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 121 "..\..\..\view\GetPositionPanel.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OkButton_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 122 "..\..\..\view\GetPositionPanel.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CancelButton_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 125 "..\..\..\view\GetPositionPanel.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CancelButton_Click);
            
            #line default
            #line hidden
            return;
            case 15:
            this.GetPositionSub = ((System.Windows.Controls.Grid)(target));
            return;
            case 16:
            this.PositionTuner = ((AutoClicker.view.PositionTuner)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

