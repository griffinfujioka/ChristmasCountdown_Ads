using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Windows.Foundation;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;       // ellipse
using Windows.UI.Xaml.Media.Animation;  // storyboard
using Windows.UI.Xaml.Media;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Markup;
using Windows.UI;           // colors
using Windows.Globalization.DateTimeFormatting; // datetime formating
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;     /* tile API */
using ChristmasCountdown_Ads.Common;
using Windows.ApplicationModel.Activation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace ChristmasCountdown_Ads
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : LayoutAwarePage
    {
        //public event System.EventHandler ScenarioLoaded;
        //public bool AutoSizeInputSectionWhenSnapped = true;
        

        //public Windows.ApplicationModel.Activation.LaunchActivatedEventArgs LaunchArgs
        //{
        //    get
        //    {
        //        return ((App)App.Current).LaunchArgs;
        //    }
        //}

        //public static MainPage Current;

        //private Frame HiddenFrame = null;

        private DispatcherTimer timer;

        #region On Navigated To
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e); 
        }
        #endregion

        private static Random random;
        //private List<Control> _layoutAwareControls;
        #region Constructor
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            //Current = this;

            //// This frame is hidden, meaning it is never shown.  It is simply used to load
            //// each scenario page and then pluck out the input and output sections and
            //// place them into the UserControls on the main page.
            //HiddenFrame = new Windows.UI.Xaml.Controls.Frame();
            //HiddenFrame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            //ContentPanel.Children.Add(HiddenFrame);

            Loaded += OnLoaded;

           
            random = new Random();
            this.StartFallingSnowAnimation();

            // We keep results in this variable
            StringBuilder results = new StringBuilder();
            results.AppendLine();

            // Create basic date/time formatters.
            DateTimeFormatter[] basicFormatters = new[]
            {
                // Default date formatters
                new DateTimeFormatter("shortdate"),
                new DateTimeFormatter("longdate"),

                // Default time formatters
                new DateTimeFormatter("shorttime"),
                new DateTimeFormatter("longtime"),
             };

            // Create date/time to format, manipulate and display.
            DateTime dateTime = DateTime.Now;
            // Try to format and display date/time if calendar supports it.
            foreach (DateTimeFormatter formatter in basicFormatters)
            {
                try
                {
                    // Format and display date/time.
                    results.AppendLine(formatter.Template + ": " + formatter.Format(dateTime));
                }
                catch (ArgumentException)
                {
                    // Retrieve and display formatter properties. 
                    results.AppendLine(String.Format(
                        "Unable to format Gregorian DateTime {0} using formatter with template {1} for languages [{2}], region {3}, calendar {4} and clock {5}",
                        dateTime,
                        formatter.Template,
                        string.Join(",", formatter.Languages),
                        formatter.GeographicRegion,
                        formatter.Calendar,
                        formatter.Clock));
                }
            }

            // Display the results
            //this.todaysDateTxtBlock.Text = results.ToString();

            DateTime Christmas = new DateTime(DateTime.Today.Year, 12, 25);
            TimeSpan ts = Christmas - DateTime.Now;
            int days = ts.Days;
            int hours = ts.Hours;
            //diffTxtBlock.Text = days + " days " + hours + "hours"; 
        }


        #endregion

       
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //Window.Current.SizeChanged += Current_SizeChanged;

            //var control = sender as Control;
            //if (control == null) return;

            //VisualStateManager.GoToState(control, ApplicationView.Value.ToString(), false);

            //if (this._layoutAwareControls == null)
            //{
            //    this._layoutAwareControls = new List<Control>();
            //}

            //this._layoutAwareControls.Add(control);



            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            timer.Tick += new EventHandler<object>(OnTick);

            timer.Start();
        }

        //void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        //{
        //    string visualState = ApplicationView.Value.ToString();

        //    if (this._layoutAwareControls != null)
        //    {
        //        foreach (var layoutAwareControl in this._layoutAwareControls)
        //        {
        //            VisualStateManager.GoToState(layoutAwareControl, visualState, false);
        //        }

        //    }
        //}

        void Scenarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (Scenarios.SelectedItem != null)
            //{
            //    NotifyUser("", NotifyType.StatusMessage);

            //    ListBoxItem selectedListBoxItem = Scenarios.SelectedItem as ListBoxItem;
            //    SuspensionManager.SessionState["SelectedScenarioIndex"] = Scenarios.SelectedIndex;

            //    Scenario scenario = selectedListBoxItem.Content as Scenario;
            //    LoadScenario(scenario.ClassType);
            //    InvalidateSize();

            //    // Fire the ScenarioLoaded event since we know that everything is loaded now.
            //    if (ScenarioLoaded != null)
            //    {
            //        ScenarioLoaded(this, new EventArgs());
            //    }
            //}
        }


        private void SetTimeTextBlock(string str)
        {
            Countdown.Text = str;
        }

        //private void InvalidateViewState()
        //{
        //    if (ApplicationView.Value == ApplicationViewState.Snapped)
        //    {
        //        Grid.SetRow(Countdown, 3);
        //        Grid.SetColumn(Countdown, 0);

        //        Grid.SetRow(untilTxtBlock, 2);
        //        Grid.SetColumn(untilTxtBlock, 0);
        //    }
        //    else
        //    {
        //        Grid.SetRow(Countdown, 1);
        //        Grid.SetColumn(Countdown, 1);

        //        Grid.SetRow(untilTxtBlock, 1);
        //        Grid.SetColumn(untilTxtBlock, 1);
        //    }

        //    // Since we don't load the scenario page in the traditional manner (we just pluck out the
        //    // input and output sections from the page) we need to ensure that any VSM code used
        //    // by the scenario's input and output sections is fired.
        //    // VisualStateManager.GoToState((Control)untilTxtBlock, "Input" + ApplicationView.Value.ToString(), false);
        //    // VisualStateManager.GoToState((Control)OutputSection, "Output" + ApplicationView.Value.ToString(), false);
        //}

        private void OnTick(object sender, object e)
        {
            var christmas = new DateTime(DateTime.Today.Year, 12, 25);
            var timeLeft = christmas - DateTime.Now;

            Countdown.Text = string.Format("{0:D2} days\n{1:D2} hours\n{2:D2} minutes\n{3:D2} seconds", timeLeft.Days, timeLeft.Hours, timeLeft.Minutes, timeLeft.Seconds);
        }

        #region Start the snow fall
        private void StartFallingSnowAnimation()
        {
            for (int i = 0; i < 40; i++)
            {
                Ellipse localCopy = this.GenerateEllipse();
                localCopy.SetValue(Canvas.LeftProperty, i * 30 + 1.0);

                double y = Canvas.GetTop(localCopy);
                double x = Canvas.GetLeft(localCopy);

                double speed = 5 * random.NextDouble();
                double index = 0;
                double radius = 30 * speed * random.NextDouble();

                localCopy.Opacity = .3 + random.NextDouble();

                CompositionTarget.Rendering += delegate(object o, object arg)
                {
                    index += Math.PI / (180 * speed);

                    if (y < this.ContentPanel.DesiredSize.Height)
                    {
                        y += .3 + speed;
                    }
                    else
                    {
                        y = -localCopy.Height;
                    }

                    Canvas.SetTop(localCopy, y);
                    Canvas.SetLeft(localCopy, x + radius * Math.Cos(index));
                    Storyboard animation = this.CreateStoryboard(localCopy, y, x + radius * Math.Cos(index));
                    Storyboard.SetTarget(animation, localCopy);

                    animation.Begin();

                };
            }
        }
        #endregion

        #region Generate an Ellipse
        private Ellipse GenerateEllipse()
        {
            Ellipse element = new Ellipse();
            element.Fill = new SolidColorBrush(Colors.White);
            element.Height = 10.0;
            element.Width = 10.0;
            this.ContentPanel.Children.Add(element);
            return element;
        }
        #endregion

        #region Create storyboard
        private Storyboard CreateStoryboard(UIElement element, double to, double toLeft)
        {
            Storyboard result = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation();
            animation.To = to;
            //Storyboard.SetTargetProperty(animation, new PropertyPath("(Canvas.Top)"));
            Storyboard.SetTargetProperty(animation, "(Canvas.Top)");
            Storyboard.SetTarget(animation, element);

            DoubleAnimation animationLeft = new DoubleAnimation();
            animationLeft.To = toLeft;
            //Storyboard.SetTargetProperty(animationLeft, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTargetProperty(animationLeft, "(Canvas.Left)");
            Storyboard.SetTarget(animationLeft, element);

            result.Children.Add(animation);
            result.Children.Add(animationLeft);

            return result;
        }
        #endregion

        private void TextBlock_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutPage));
        }
    }
}
