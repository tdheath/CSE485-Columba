using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace CapGUI
{
    public partial class MainPage : UserControl
    {
        private List<Panel> colPanels = new List<Panel>();
        private ListBox editorPanel;
        private ListBox blockPalette;
        private ListBox methodPalette;
        private ListBox variablePalette;
        private Canvas testingCanvas;
        //Point StartingDragPoint;
        //Panel blocksPanel;

        // Global variables used to keep track of the 
        // mouse position and whether the object is captured
        // by the mouse.
        bool isMouseCaptured;
        double mouseVerticalPosition;
        double mouseHorizontalPosition;

        public MainPage()
        {
            InitializeComponent();
            //Canvas x = replaceBlock(10,10);
            editorPanel = testListBox();
            blockPalette = testListBox();
            methodPalette = testListBox();
            variablePalette = testListBox();

            //LayoutRoot.Children.Add(x);
            LayoutRoot.Children.Add(editorPanel);
            LayoutRoot.Children.Add(blockPalette);
            LayoutRoot.Children.Add(methodPalette);
            LayoutRoot.Children.Add(variablePalette);
            testingCanvas = replaceBlock(63, 261, "prints here");
            LayoutRoot.Children.Add(testingCanvas);
            //Grid.SetRow(testingCanvas, 1);
            //Grid.SetColumn(testingCanvas, 0);
            //FrameworkElement frameworkElement = (FrameworkElement)thisOne2;

            //Need to use PreviewMouseDownHandler to access the items in the listbox to drag and drop
            //this.blockPalette.MouseLeftButtonDown += new MouseButtonEventHandler(Handle_MouseDown);
            blockPalette.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(Handle_MouseDown), true);
            
            //Grid.SetRow(x, 0);
            //Grid.SetColumn(x, 0);
            Grid.SetRowSpan(editorPanel, 4);
            Grid.SetRow(editorPanel, 0);
            Grid.SetColumn(editorPanel, 1);
            Grid.SetRow(blockPalette, 1);
            Grid.SetColumn(blockPalette, 0);
            Grid.SetRow(methodPalette, 2);
            Grid.SetColumn(methodPalette, 0);
            Grid.SetRow(variablePalette, 3);
            Grid.SetColumn(variablePalette, 0);
        }

        public void Handle_MouseDown(object sender, MouseEventArgs args)
        {
            ListBox item = sender as ListBox;
            //int index = item.SelectedIndex;
            String indexString = item.SelectedItem.ToString();
            //testingCanvas = replaceBlock(10, 10, indexString);
            //LayoutRoot.Children.Add(testingCanvas);
            //Grid.SetRow(testingCanvas, 0);
            //Grid.SetColumn(testingCanvas, 0);
            Point position = args.GetPosition(this);
            double mouseVerticalPosition = position.Y;
            double mouseHorizontalPosition = position.X;
            Debug.WriteLine(mouseHorizontalPosition);
            Debug.WriteLine(mouseVerticalPosition);
            testingCanvas = replaceBlock(mouseHorizontalPosition, mouseVerticalPosition, indexString);
            LayoutRoot.Children.Add(testingCanvas);
            testingCanvas.MouseMove += new MouseEventHandler(Handle_MouseMove);
            isMouseCaptured = true;
            item.CaptureMouse();
        }

        public void Handle_MouseMove(object sender, MouseEventArgs args)
        {
            Canvas item = sender as Canvas;
            if (isMouseCaptured)
            {

                // Calculate the current position of the object.
                double deltaV = args.GetPosition(null).Y - mouseVerticalPosition;
                double deltaH = args.GetPosition(null).X - mouseHorizontalPosition;
                double newTop = deltaV + (double)item.GetValue(Canvas.TopProperty);
                double newLeft = deltaH + (double)item.GetValue(Canvas.LeftProperty);

                // Set new position of object.
                item.SetValue(Canvas.TopProperty, newTop);
                item.SetValue(Canvas.LeftProperty, newLeft);

                // Update position global variables.
                mouseVerticalPosition = args.GetPosition(null).Y;
                mouseHorizontalPosition = args.GetPosition(null).X;
            }
        }

        public void Handle_MouseUp(object sender, MouseEventArgs e)
        {
            Canvas item = sender as Canvas;
            isMouseCaptured = false;
            item.ReleaseMouseCapture();
            mouseVerticalPosition = -1;
            mouseHorizontalPosition = -1;
            FrameworkElement objFrameworkElement = (FrameworkElement)sender;
            //Canvas x = replaceBlock(10, 10);
            //LayoutRoot.Children.Add(x);

            //LayoutRoot.Children.Remove((FrameworkElement)sender);


             /*FrameworkElement objFrameworkElement = (FrameworkElement)sender;
            /*objFrameworkElement.ReleaseMouseCapture();

            objFrameworkElement.MouseMove -= 
                new MouseEventHandler(Handle_MouseMove);
            objFrameworkElement.MouseLeftButtonUp -= 
                new MouseButtonEventHandler(Handle_MouseUp);

            // If it is an element marked [draggable]
            // try to drop it on a panel stored in the colPanels collection
            if (objFrameworkElement.Tag.ToString().Contains("[draggable]"))
            {
                Point tmpPoint = e.GetPosition(null);
                // Build a list of elements at the current mouse position

                List<UIElement> hits = VisualTreeHelper.FindElementsInHostCoordinates( tmpPoint, this ) 
                    as List<UIElement>;

                bool foundPanel = false;
                
                // Loop through all the Panels in the colPanels collection
                foreach (Panel objPanel in colPanels)
                {
                    if (hits.Contains(objPanel))
                    {
                        // Grab the position of the element being dragged in relation to it's position on the 
                        // main canvas and it's position in relation to the panel it may be dropped on
                        Point mousePos1 = e.GetPosition(objPanel);
                        Point mousePos2 = e.GetPosition(objFrameworkElement);

                        // Remove the element from the main canvas
                        this.LayoutRoot.Children.Remove(objFrameworkElement);

                        // Import content
                        // Get a reference to the parent of the current panel
                        UserControl objUserControl = (UserControl)objPanel.Parent;
                        // See if that parent implements an interface called "ImportContent"
                        object objObject = objUserControl.GetType().GetInterface("ImportContent", true);

                        // If the object is not null then the parent object has a method called "ImportContent"
                        if (!(objObject == null))
                        {
                            // Create a parmeters array
                            object[] parameters = new object[1];
                            // Add the elemnt that is being dragged to the array
                            parameters.SetValue(objFrameworkElement, 0);
                            // invoke the "ImportContent" on the parent object passing the parameters array that 
                            // contains the element being dragged
                            bool boolImport = (bool)objUserControl.GetType().InvokeMember("ImportContent", 
                                BindingFlags.InvokeMethod, null, objUserControl, parameters);

                            // If the import was not successful simply add the element to the panel
                            if (!boolImport)
                            {
                                // Add the element to the panel
                                objPanel.Children.Add(objFrameworkElement);
                                Canvas.SetLeft(objFrameworkElement, mousePos1.X - mousePos2.X);
                                Canvas.SetTop(objFrameworkElement, mousePos1.Y - mousePos2.Y);
                            }
                        }
                        else
                        {
                            // The parent object does not implement the "ImportContent" Interface
                            // Add the element to the panel
                            objPanel.Children.Add(objFrameworkElement);
                            Canvas.SetLeft(objFrameworkElement, mousePos1.X - mousePos2.X);
                            Canvas.SetTop(objFrameworkElement, mousePos1.Y - mousePos2.Y);
                        }
                        foundPanel = true;
                        break;
                    }
                }

                if (!foundPanel)
                {
                    this.LayoutRoot.Children.Remove(objFrameworkElement);
                    System.Diagnostics.Debug.WriteLine("Failed");
                    blocksPanel.Children.Add(objFrameworkElement);
                    Canvas.SetLeft(objFrameworkElement, StartingDragPoint.X);
                    Canvas.SetTop(objFrameworkElement, StartingDragPoint.Y);
                }
            }*/
        
        }

        #region ImportContent
        public bool ImportContent(FrameworkElement objFrameworkElement)
        {
            // This import method will only import text content contained
            // in TextBlocks that are placed on a Canvas

            StringBuilder StringBuilder = new StringBuilder();
            Canvas objCanvas = objFrameworkElement as Canvas;

            // If the element being imported is not a canvas return false
            if (objCanvas == null)
            {
                return false;
            }

            try
            {
                // Loop through all the UIElements in the Canvas 
                foreach (UIElement objUIElement in objCanvas.Children)
                {
                    // Try to cast the UIElement as a TextBlock
                    TextBlock objTextBlock = objUIElement as TextBlock;
                    if (objTextBlock != null)
                    {
                        // Add the contents of the TextBlock to the output
                        StringBuilder.Append(String.Format(" {0}", objTextBlock.Text));
                    }
                }
            }
            catch
            {
                return false;
            }

            // Was content in the output ?
            if (StringBuilder.Length > 0)
            {
                // Add output to the Textbox display
               // this.txtContent.Text = StringBuilder.ToString();
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        private Canvas replaceBlock(double x, double y, String data)
        {
            Canvas newCanvas = new Canvas();
            newCanvas.Width = 72;
            newCanvas.Height = 35;
            TextBlock text = new TextBlock();
            text.Text = data;
            Canvas.SetLeft(newCanvas, x);
            Canvas.SetTop(newCanvas, y);
            //newCanvas.SetValue(Canvas.LeftProperty, x);
            //newCanvas.SetValue(Canvas.TopProperty, y);
            Debug.WriteLine("canvas x:" + x);
            Debug.WriteLine("canvas y:" + y);
            newCanvas.SetValue(Canvas.BackgroundProperty, new SolidColorBrush(Colors.Red));
            newCanvas.Children.Add(text);
            return newCanvas;
        }

        private ListBox testListBox()
        {
            ListBox newListBox = new ListBox();
            List<String> newList = new List<string>();
            newList.Add("Test");
            newList.Add("This");
            /*for (int i = 0; i < 100; i++)
            {
                newList.Add("butts");
                newList.Add("hello");
            }*/
            //newListBox.MouseLeftButtonDown(Handle_MouseDown);//="Handle_MouseDown"
            //    MouseMove="Handle_MouseMove"
             //   MouseLeftButtonUp="Handle_MouseUp"
            newListBox.ItemsSource = newList;
            return newListBox;
        }

    }
}
