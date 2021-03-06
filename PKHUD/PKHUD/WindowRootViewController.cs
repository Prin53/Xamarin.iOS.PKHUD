﻿//
// WindowRootViewController.cs
//
// Author:
//       Denys Fiediaiev <prineduard@gmail.com>
//
// Copyright (c) 2017 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using UIKit;

namespace PKHUD
{
    /// <summary>
    /// Serves as a configuration relay controller, tapping into the main window's rootViewController settings.
    /// </summary>
    internal class WindowRootViewController : UIViewController
    {
        protected static UIViewController RootViewController =>
            UIApplication.SharedApplication.Delegate?.GetWindow()?.RootViewController;
        
        public override UIStatusBarAnimation PreferredStatusBarUpdateAnimation 
            => RootViewController?.PreferredStatusBarUpdateAnimation ?? UIStatusBarAnimation.None;

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return RootViewController?.GetSupportedInterfaceOrientations() ?? UIInterfaceOrientationMask.Portrait;
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return PresentingViewController?.PreferredStatusBarStyle()
                   ?? UIApplication.SharedApplication.StatusBarStyle;
        }

        public override bool PrefersStatusBarHidden()
        {
            return PresentingViewController?.PrefersStatusBarHidden()
                   ?? UIApplication.SharedApplication.StatusBarHidden;
        }

        public override bool ShouldAutorotate()
        {
            return RootViewController?.ShouldAutorotate() ?? false;
        }
    }
}
