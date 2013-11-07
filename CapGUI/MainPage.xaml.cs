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
using System.Windows.Controls.Internals;
//using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CapGUI
{
    public partial class MainPage : UserControl
    {
        private List<Panel> colPanels = new List<Panel>();
        //private ListBox editorPalette;
        //private ListBox blockPalette;
        //private ListBox methodPalette;
        //private ListBox variablePalette;
        private ListBox trash;
        //private String passInfo; //used for testing the passing
        private List<String> newList;
        private List<Block> DragDropList;
        private TrashDragDropTarget trashDragDrop;
        //private EditorDragDropTarget editorDragDrop;
        private ObservableCollection<Block> editorList; 
        
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

            trashDragDrop = new TrashDragDropTarget();
            //editorDragDrop = new EditorDragDropTarget();
            trash = new ListBox();
            //editorPalette = new ListBox();
            ListBox testingBox = new ListBox();

            editorList = new ObservableCollection<Block>();
            DragDropList = new List<Block>();
            DragDropList.Add(new Block("Woof", Colors.Cyan));
            DragDropList.Add(new Block("Meow", Colors.Cyan));
            DragDropList.Add(new Block("Tweet", Colors.Cyan));
            DragDropList.Add(new Block("Squeak", Colors.Cyan));
            DragDropList.Add(new Block("Moo", Colors.Cyan));
            DragDropList.Add(new Block("Croak", Colors.Cyan));
            DragDropList.Add(new Block("Toot", Colors.Cyan));
            DragDropList.Add(new Block("Quack", Colors.Cyan));
            DragDropList.Add(new Block("Blub", Colors.Cyan));
            DragDropList.Add(new Block("Ow Ow Ow", Colors.Cyan));
            blockPalette.ItemsSource = DragDropList;
            
            
            testingBox.ItemsSource = DragDropList;
            testingBox.Background = new SolidColorBrush(Colors.Orange);
            dragDrop.Content = testingBox;

            //Adding to trash stack panel in the layoutroot in the xaml
            trash.Height = 200;
            trash.Width = 380;
            trashDragDrop.Content = trash;
            trashDragDrop.AllowDrop = true;
            trashDragDrop.AllowAdd = false;
            //trashDragDrop.Opacity = 0;
            trashPanel.Children.Add(trashDragDrop);

            //editorPalette.Height = 830;
            //editorPalette.Width = 830;
            //editorPalette.ItemsSource = editorList;
            //editorDragDrop.Content = editorPalette;
            //editorDragDrop.AllowDrop = true;
            //editorPanel.Children.Add(editorDragDrop);
            editorPalette.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(Handle_EditorMouseDown), true);
            //editorPalette.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(Handle_EditorMouseUp), true);

            blockPalette.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(Handle_OtherMouseDown), true);
            methodPalette.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(Handle_OtherMouseDown), true);
            variablePalette.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(Handle_OtherMouseDown), true);
            dragDrop.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(Handle_OtherMouseDown), true);
            
        }

        private void Bind(ListBox listbox, List<Block> list)
        {
            listbox.ItemsSource = null;
            listbox.ItemsSource = list;
        }

        public void Handle_OtherMouseDown(object sender, MouseEventArgs args)
        {
            Debug.WriteLine("fire");
            trashDragDrop.AllowAdd = false;
        }

        public void Handle_EditorMouseDown(object sender, MouseEventArgs args)
        {
            ListBox listBox = sender as ListBox;
            if (listBox.SelectedItem != null)
            {
                trashDragDrop.AllowAdd = true;
                ((Block)listBox.SelectedItem).index = listBox.SelectedIndex;
            }
            isMouseCaptured = true;
            listBox.CaptureMouse();
        }

        private void Handle_ButtonOnClick(object sender, EventArgs args)
        {
            if (trash.Items.Count != 0)
            {
                Block tempBlock = trash.Items[0] as Block;
                this.tblText.Text = "There is an item! " + tempBlock.ToString();
                trash.Items.Remove(trash.Items[0]);
                editorPalette.Items.Insert(tempBlock.index, tempBlock);
            }
            else
                this.tblText.Text = "There are no items!";
        }

        public void Handle_EditorMouseUp(object sender, MouseEventArgs args)
        {
            //ListBox listBox = sender as ListBox;
            for (int i = 0; i < this.editorPalette.Items.Count; i++)
            {
                ((Block)this.editorPalette.Items[i]).index = (i);
            }
            
            //Bind(this.editorPalette, DragDropList);
            //isMouseCaptured = false;
            //listBox.ReleaseMouseCapture();
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
            ListBox item = sender as ListBox;
            //Debug.WriteLine("string passed: " + passInfo);
            //if (passInfo != null)
            //{
            //    editorPanel.Items.Add(passInfo);
            //}
            //String indexString = item.SelectedItem.ToString();
            isMouseCaptured = false;
            item.ReleaseMouseCapture();
            mouseVerticalPosition = -1;
            mouseHorizontalPosition = -1;
            //FrameworkElement objFrameworkElement = (FrameworkElement)sender;
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
            newList = new List<string>();
            newList.Add("Test");
            newList.Add("This");
            /*for (int i = 0; i < 100; i++)
            {
                newList.Add("butts");
                newList.Add("hello");
            }*/
            newListBox.ItemsSource = newList;
            return newListBox;
        }

    }
}
