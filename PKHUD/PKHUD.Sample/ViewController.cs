﻿//
// ViewController.cs
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

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UIKit;

namespace PKHUD.Sample
{
    /// <summary>
    /// Please note that the blow demonstrates the "porcelain" interface
    /// - a more concise and clean way to work with the HUD.
    /// If you need more options and flexbility, 
    /// feel free to use the underlying "plumbing". E.g.:
    /// PKHUD.Instance.Show();
    /// PKHUD.Instance.ContentView = new SuccessView("Success!");
    /// PKHUD.Instance.Hide(TimeSpan.FromSeconds(2));
    /// </summary>
    public class ViewController : UIViewController
    {
        private const int Padding = 16;
        private const int ButtonHeight = 44;

        protected UIImageView BackgroundImage { get; private set; }

        protected UIStackView ButtonsStack { get; private set; }

        protected UIButton AnimatedSuccessButton { get; private set; }

        protected UIButton AnimatedErrorButton { get; private set; }

        protected UIButton AnimatedProgressButton { get; private set; }

        protected UIButton GraceAnimatedProgressButton { get; private set; }

        protected UIButton AnimatedStatusProgressButton { get; private set; }

        protected UIButton TextButton { get; private set; }

        private static UIButton CreateButton(string title)
        {
            var button = UIButton.FromType(UIButtonType.System);
            {
                button.TranslatesAutoresizingMaskIntoConstraints = false;
                button.SetTitleColor(UIColor.FromRGB(64, 64, 64), UIControlState.Normal);
                button.SetTitleShadowColor(UIColor.FromRGB(128, 128, 128), UIControlState.Normal);
                button.SetTitle(title, UIControlState.Normal);
                button.BackgroundColor = UIColor.FromRGB(255, 255, 255).ColorWithAlpha(.66f);
            }
            return button;
        }

        private static void AnimatedSuccessButtonOnTouchUpInside(object sender, EventArgs e)
        {
            Hud.Create()
                .WithSuccessContent()
                .Build()
                .Flash(TimeSpan.FromSeconds(2));
        }

        private static void AnimatedErrorButtonOnTouchUpInside(object sender, EventArgs e)
        {
            var hud = Hud.Create().WithErrorContent().Build();
            hud.Show();
            hud.HideWithDelay(TimeSpan.FromSeconds(2));
        }

        private static async void AnimatedProgressButtonOnTouchUpInside(object sender, EventArgs e)
        {
            Hud.Create().WithProgressContent()
                .WithBackgroundDimming(true)
                .Build()
                .Show();

            await Task.Delay(TimeSpan.FromSeconds(2));

            Hud.Create().WithSuccessContent()
                .Build()
                .Flash(TimeSpan.FromSeconds(1));
        }

        private static void GraceAnimatedProgressButtonOnTouchUpInside(object sender, EventArgs e)
        {
            Hud.Create().WithProgressContent()
                .WithGracePeriod(TimeSpan.FromSeconds(2))
                .Build()
                .Flash(TimeSpan.FromSeconds(1));
        }

        private static void AnimatedStatusProgressButtonOnTouchUpInside(object sender, EventArgs e)
        {
            Hud.Create().WithProgressContent()
                .WithTitle("Title")
                .WithSubtitle("Subtitle")
                .Build()
                .Flash(TimeSpan.FromSeconds(2));
        }

        private static async void TextButtonOnTouchUpInside(object sender, EventArgs e)
        {
            await Hud.Create().WithLabelContent()
                .WithTitle("Requesting Licence…")
                .Build()
                .FlashAsync(TimeSpan.FromSeconds(2));

            Debug.WriteLine("License Obtained");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                AnimatedSuccessButton.TouchUpInside -= AnimatedSuccessButtonOnTouchUpInside;
                AnimatedErrorButton.TouchUpInside -= AnimatedErrorButtonOnTouchUpInside;
                AnimatedProgressButton.TouchUpInside -= AnimatedProgressButtonOnTouchUpInside;
                GraceAnimatedProgressButton.TouchUpInside -= GraceAnimatedProgressButtonOnTouchUpInside;
                AnimatedStatusProgressButton.TouchUpInside -= AnimatedStatusProgressButtonOnTouchUpInside;
                TextButton.TouchUpInside -= TextButtonOnTouchUpInside;
            }

            base.Dispose(disposing);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            BackgroundImage = new UIImageView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Image = UIImage.FromBundle("Background"),
                ContentMode = UIViewContentMode.ScaleAspectFill
            };

            ButtonsStack = new UIStackView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Axis = UILayoutConstraintAxis.Vertical,
                Spacing = 8
            };

            AnimatedSuccessButton = CreateButton("Animated Success HUD");
            AnimatedSuccessButton.TouchUpInside += AnimatedSuccessButtonOnTouchUpInside;

            AnimatedErrorButton = CreateButton("Animated Error HUD");
            AnimatedErrorButton.TouchUpInside += AnimatedErrorButtonOnTouchUpInside;

            AnimatedProgressButton = CreateButton("Animated Progress HUD");
            AnimatedProgressButton.TouchUpInside += AnimatedProgressButtonOnTouchUpInside;

            GraceAnimatedProgressButton = CreateButton("Grace Animated Progress HUD");
            GraceAnimatedProgressButton.TouchUpInside += GraceAnimatedProgressButtonOnTouchUpInside;

            AnimatedStatusProgressButton = CreateButton("Animated Status Progress HUD");
            AnimatedStatusProgressButton.TouchUpInside += AnimatedStatusProgressButtonOnTouchUpInside;

            TextButton = CreateButton("Text HUD");
            TextButton.TouchUpInside += TextButtonOnTouchUpInside;

            ButtonsStack.AddArrangedSubview(AnimatedSuccessButton);
            ButtonsStack.AddArrangedSubview(AnimatedErrorButton);
            ButtonsStack.AddArrangedSubview(AnimatedProgressButton);
            ButtonsStack.AddArrangedSubview(GraceAnimatedProgressButton);
            ButtonsStack.AddArrangedSubview(AnimatedStatusProgressButton);
            ButtonsStack.AddArrangedSubview(TextButton);

            View.AddSubviews(BackgroundImage, ButtonsStack);

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                View.AddConstraint(ButtonsStack.LeadingAnchor.ConstraintEqualTo(
                    View.SafeAreaLayoutGuide.LeadingAnchor,
                    Padding
                ));
                View.AddConstraint(ButtonsStack.TrailingAnchor.ConstraintEqualTo(
                    View.SafeAreaLayoutGuide.TrailingAnchor,
                    -Padding
                ));
                View.AddConstraint(ButtonsStack.BottomAnchor.ConstraintEqualTo(
                    View.SafeAreaLayoutGuide.BottomAnchor,
                    -Padding
                ));
            }
            else
            {
                View.AddConstraint(ButtonsStack.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, Padding));
                View.AddConstraint(ButtonsStack.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -Padding));
                View.AddConstraint(ButtonsStack.BottomAnchor.ConstraintEqualTo(View.BottomAnchor, -Padding));
            }

            View.AddConstraints(new[]
            {
                BackgroundImage.TopAnchor.ConstraintEqualTo(View.TopAnchor),
                BackgroundImage.BottomAnchor.ConstraintEqualTo(View.BottomAnchor),
                BackgroundImage.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor),
                BackgroundImage.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor),
                AnimatedSuccessButton.HeightAnchor.ConstraintEqualTo(ButtonHeight),
                AnimatedErrorButton.HeightAnchor.ConstraintEqualTo(ButtonHeight),
                AnimatedProgressButton.HeightAnchor.ConstraintEqualTo(ButtonHeight),
                GraceAnimatedProgressButton.HeightAnchor.ConstraintEqualTo(ButtonHeight),
                AnimatedStatusProgressButton.HeightAnchor.ConstraintEqualTo(ButtonHeight),
                TextButton.HeightAnchor.ConstraintEqualTo(ButtonHeight)
            });
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.AllButUpsideDown;
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.LightContent;
        }
    }
}
