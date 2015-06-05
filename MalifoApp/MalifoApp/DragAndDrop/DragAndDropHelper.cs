using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace MalifoApp.DragAndDrop
{
    public class DragAndDropHelper
    {
        private static readonly Lazy<DragAndDropHelper> _lazy =
            new Lazy<DragAndDropHelper>(() => new DragAndDropHelper());

        public static DragAndDropHelper Instance { get { return _lazy.Value; } }

        private DataFormat format = DataFormats.GetDataFormat("DragAndDropFormat");
        private DraggedAdorner draggedAdorner;
        private Window topWindow;
        private FrameworkElement sourceFrameworkElement;
        private Point initialMousePosition;
        private Point initialMouseOffset;
        private object draggedDataContext;


        #region Dependency Properties

        /// <summary>
        /// serves as a flag if the control supports being dragged
        /// </summary>
        public static readonly DependencyProperty IsDragSourceProperty =
            DependencyProperty.RegisterAttached("IsDragSource", typeof(bool), typeof(DragAndDropHelper), new UIPropertyMetadata(false, IsDragSourceChanged));

        public static bool GetIsDragSource(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDragSourceProperty);
        }

        public static void SetIsDragSource(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDragSourceProperty, value);
        }

        /// <summary>
        /// serves as a flag if the control supports accepting drops
        /// </summary>
        public static readonly DependencyProperty IsDropTargetProperty =
            DependencyProperty.RegisterAttached("IsDropTarget", typeof(bool), typeof(DragAndDropHelper), new UIPropertyMetadata(false, IsDropTargetChanged));

        public static bool GetIsDropTarget(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDropTargetProperty);
        }

        public static void SetIsDropTarget(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDropTargetProperty, value);
        }

        /// <summary>
        /// a property to pass a visual representation to display while an item is dragged
        /// </summary>
        public static readonly DependencyProperty DragDropTemplateProperty =
            DependencyProperty.RegisterAttached("DragDropTemplate", typeof(DataTemplate), typeof(DragAndDropHelper), new UIPropertyMetadata(null));
        
        public static DataTemplate GetDragDropTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(DragDropTemplateProperty);
        }

        public static void SetDragDropTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(DragDropTemplateProperty, value);
        }

        /// <summary>
        /// allow for binding commands that we will call
        /// </summary>
        public static readonly DependencyProperty DropCommandProperty =
            DependencyProperty.RegisterAttached("DropCommand", typeof(ICommand), typeof(DragAndDropHelper), new UIPropertyMetadata(null));

        public static ICommand GetDropCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DropCommandProperty);
        }

        public static void SetDropCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DropCommandProperty, value);
        }

        private static void IsDragSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var dragSource = obj as UIElement;
            if (dragSource != null)
            {
                if (Object.Equals(e.NewValue, true))
                {
                    dragSource.PreviewMouseLeftButtonDown += Instance.DragSource_PreviewMouseLeftButtonDown;
                    dragSource.PreviewMouseLeftButtonUp += Instance.DragSource_PreviewMouseLeftButtonUp;
                    dragSource.PreviewMouseMove += Instance.DragSource_PreviewMouseMove;
                }
                else
                {
                    dragSource.PreviewMouseLeftButtonDown -= Instance.DragSource_PreviewMouseLeftButtonDown;
                    dragSource.PreviewMouseLeftButtonUp -= Instance.DragSource_PreviewMouseLeftButtonUp;
                    dragSource.PreviewMouseMove -= Instance.DragSource_PreviewMouseMove;
                }
            }
        }

        private static void IsDropTargetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var dropTarget = obj as UIElement;
            if (dropTarget != null)
            {
                if (Object.Equals(e.NewValue, true))
                {
                    dropTarget.AllowDrop = true;
                    dropTarget.PreviewDragEnter += Instance.DropTarget_PreviewDragEnter;
                    dropTarget.PreviewDragOver += Instance.DropTarget_PreviewDragOver;
                    dropTarget.PreviewDragLeave += Instance.DropTarget_PreviewDragLeave;
                    dropTarget.PreviewDrop += Instance.DropTarget_PreviewDrop;
                }
                else
                {
                    dropTarget.AllowDrop = false;
                    dropTarget.PreviewDragEnter -= Instance.DropTarget_PreviewDragEnter;
                    dropTarget.PreviewDragOver -= Instance.DropTarget_PreviewDragOver;
                    dropTarget.PreviewDragLeave -= Instance.DropTarget_PreviewDragLeave;
                    dropTarget.PreviewDrop -= Instance.DropTarget_PreviewDrop;
                }
            }
        }

        #endregion

        #region interaction handlers

        #region drag handlers

        private void DragSource_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement control = sender as FrameworkElement;
            if (control != null)
            {
                sourceFrameworkElement = control;
                topWindow = Window.GetWindow(control);

                initialMousePosition = e.GetPosition(topWindow);

                // we want to pass to the data context to the command later
                draggedDataContext = control.DataContext;
            }
            
        }

        private void DragSource_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            draggedDataContext = null;
        }

        private void DragSource_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (draggedDataContext != null)
            {
                // Only drag when user moved the mouse by a reasonable amount.
                if (IsMovementBigEnough(initialMousePosition, e.GetPosition(topWindow)))
                {
                    initialMouseOffset = e.GetPosition(sourceFrameworkElement);

                    DataObject data = new DataObject(format.Name, draggedDataContext);

                    // Adding events to the window to make sure dragged adorner comes up when mouse is not over a drop target.
                    bool previousAllowDrop = this.topWindow.AllowDrop;
                    topWindow.AllowDrop = true;
                    topWindow.DragEnter += TopWindow_DragEnter;
                    topWindow.DragOver += TopWindow_DragOver;
                    topWindow.DragLeave += TopWindow_DragLeave;

                    // synchronous call to initiate a drag and drop operation, this blocks until drag and drop is complete
                    DragDropEffects effects = DragDrop.DoDragDrop((DependencyObject)sender, data, DragDropEffects.Move);

                    // Without this call, there would be a bug in the following scenario: Click on a data item, and drag
                    // the mouse very fast outside of the window. When doing this really fast, for some reason I don't get 
                    // the Window leave event, and the dragged adorner is left behind.
                    // With this call, the dragged adorner will disappear when we release the mouse outside of the window,
                    // which is when the DoDragDrop synchronous method returns.
                    RemoveAdorner();

                    topWindow.AllowDrop = previousAllowDrop;
                    topWindow.DragEnter -= TopWindow_DragEnter;
                    topWindow.DragOver -= TopWindow_DragOver;
                    topWindow.DragLeave -= TopWindow_DragLeave;

                    draggedDataContext = null;
                }
            }
        }

        private bool IsMovementBigEnough(Point origin, Point currentPosition)
        {
            return (Math.Abs(currentPosition.X - origin.X) >= SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(currentPosition.Y - origin.Y) >= SystemParameters.MinimumVerticalDragDistance);
        }

        #endregion

        #region window handlers

        private void TopWindow_DragEnter(object sender, DragEventArgs e)
        {
            ShowAdorner(e.GetPosition(topWindow));
            e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        private void TopWindow_DragOver(object sender, DragEventArgs e)
        {
            ShowAdorner(e.GetPosition(topWindow));
            e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        private void TopWindow_DragLeave(object sender, DragEventArgs e)
        {
            RemoveAdorner();
            e.Handled = true;
        }

        #endregion

        #region drop handlers

        private void DropTarget_PreviewDragEnter(object sender, DragEventArgs e)
        {
            if (draggedDataContext != null)
            {
                ShowAdorner(e.GetPosition(topWindow));
                e.Handled = true;
            }
        }

        private void DropTarget_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (draggedDataContext != null)
            {
                ShowAdorner(e.GetPosition(topWindow));
                e.Handled = true;
            }
        }

        private void DropTarget_PreviewDragLeave(object sender, DragEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void DropTarget_PreviewDrop(object sender, DragEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("drop: " + sender + "; " + e);
            FrameworkElement element = sender as FrameworkElement;
            if (element != null)
            {
                ICommand cmd = GetDropCommand(element);
                System.Diagnostics.Debug.WriteLine("cmd = " + cmd);
                if (cmd != null && cmd.CanExecute(draggedDataContext))
                {
                    cmd.Execute(draggedDataContext);
                }
                RemoveAdorner();
            }
        }

        #endregion

        #endregion

        #region adorners

        private void ShowAdorner(Point currentPosition)
        {
            if (this.draggedAdorner == null)
            {
                UIElement adornedElement = topWindow.Content as UIElement;
                if (adornedElement == null)
                    return;

                var adornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
                this.draggedAdorner = new DraggedAdorner(draggedDataContext, GetDragDropTemplate(sourceFrameworkElement), adornerLayer, adornedElement);
            }
            draggedAdorner.SetPosition(currentPosition.X - initialMouseOffset.X, currentPosition.Y - initialMouseOffset.Y);
        }

        private void RemoveAdorner()
        {
            if (this.draggedAdorner != null)
            {
                this.draggedAdorner.Detach();
                this.draggedAdorner = null;
            }
        }

        #endregion
    }
}
