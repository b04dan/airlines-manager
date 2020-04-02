using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Airlines.Behaviors
{
    // вспомогательный класс, для привязки команд к событиям начала/конца перетаскивания слайдера
    // Создано на основе: https://stackoverflow.com/questions/14331272/issue-with-thumb-dragstarted-event-with-mvvmlight
    public static class SliderDragBehavior
    {
        public static readonly DependencyProperty DragCompletedCommandProperty =
            DependencyProperty.RegisterAttached("DragCompletedCommand", typeof(ICommand), typeof(SliderDragBehavior),
            new FrameworkPropertyMetadata(DragCompleted));

        public static readonly DependencyProperty DragStartedCommandProperty =
            DependencyProperty.RegisterAttached("DragStartedCommand", typeof(ICommand), typeof(SliderDragBehavior),
                new FrameworkPropertyMetadata(DragStarted));

        private static void DragCompleted(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var slider = (Slider)element;
            var thumb = GetThumbFromSlider(slider);

            thumb.DragCompleted += thumb_DragCompleted;
        }

        private static void DragStarted(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var slider = (Slider)element;
            var thumb = GetThumbFromSlider(slider);

            thumb.DragStarted += thumb_DragStarted;
        }

        private static void thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            element.Dispatcher.Invoke(() =>
            {
                var command = GetDragCompletedCommand(element);
                var slider = FindParentControl<Slider>(element) as Slider;
                command.Execute(slider.Value);
            });
        }

        private static void thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            element.Dispatcher.Invoke(() =>
            {
                var command = GetDragStartedCommand(element);
                var slider = FindParentControl<Slider>(element) as Slider;
                command.Execute(slider.Value);
            });
        }

        public static void SetDragCompletedCommand(UIElement element, ICommand value)
        {
            element.SetValue(DragCompletedCommandProperty, value);
        }

        public static ICommand GetDragCompletedCommand(FrameworkElement element)
        {
            var slider = FindParentControl<Slider>(element);
            return (ICommand)slider.GetValue(DragCompletedCommandProperty);
        }

        public static void SetDragStartedCommand(UIElement element, ICommand value)
        {
            element.SetValue(DragStartedCommandProperty, value);
        }

        public static ICommand GetDragStartedCommand(FrameworkElement element)
        {
            var slider = FindParentControl<Slider>(element);
            return (ICommand)slider.GetValue(DragStartedCommandProperty);
        }

        private static Thumb GetThumbFromSlider(Slider slider)
        {
            var track = slider.Template.FindName("PART_Track", slider) as Track;
            return track?.Thumb;
        }

        private static DependencyObject FindParentControl<T>(DependencyObject control)
        {
            var parent = VisualTreeHelper.GetParent(control);
            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent;
        }
    }
}
