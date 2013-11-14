using System;
using System.Collections.Generic; //SelectionCollection
using System.Linq; //used for generic calls
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
using System.Diagnostics; //debug
using System.Windows.Controls.Internals; //toolkit
using System.Collections.ObjectModel; //ObservableCollection
using System.ComponentModel; //ICollectionView
using System.Windows.Data; //CollectionViewSource

namespace CapGUI
{
    public partial class MainPage : UserControl
    {
        private List<Panel> colPanels = new List<Panel>();

        //private String[] whatDoesTheFoxSay = { "Woof", "Meow", "Tweet", "Squeak", "Moo", "Croak", "Toot", "Quack", "Blub", "Ow Ow Ow" };
        private String[] programStructure = { "CONDITION", "IF", "ELSE IF", "ELSE", "LOOP", "LOOP FOR", "WAIT", "WAIT FOR" };
        private String[] robotFunctions = { "SENSOR FUNCTIONS", "MOTOR FUNCTIONS" };
        private List<Block> programStructureList;
        private List<Block> robotFunctionsList;
        //private ObservableCollection<Block> editorList { get; set; } 
        
        public MainPage()
        {
            /*
             * Buttons that you can call
             * createVariableBtn
             * editVariableBtn
             * createMethodBtn
             * editMethodBtn
             * connectToRobotBtn
             * loadProgramBtn
             * runBtn
             * stopBtn
             * redoBtn
             * undoBtn
             * 
             * Tab Control (see other project for example on how to use)
             * editorTabControl
             * editorMain (TabItem in the TabControl)
             *
             */

            /* Currently the panel are all stretching but
             * listbox data is no longer visible.
             * We need to find a way to fix this or just hard
             * code a max size.
             */ 

            InitializeComponent();

            //Lists
            //editorList = new ObservableCollection<Block>();
            programStructureList = new List<Block>();
            robotFunctionsList = new List<Block>();

            for (int i = 0; i < programStructure.Length; i++)
            {
                if(i < robotFunctions.Length)
                    robotFunctionsList.Add(new Block(robotFunctions[i], Colors.Red));
                programStructureList.Add(new Block(programStructure[i], Colors.Cyan));
                //editorList.Add(new Block(programStructure[i], Colors.Cyan));
            }

            //Set ItemsSource of ListBox to desired Lists
            //blockPalette.ItemsSource = editorList;
            blockPalette.ItemsSource = programStructureList;
            robotPalette.ItemsSource = robotFunctionsList;

            //Allow blocks to be placed in trash
            editorPalette.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(Handle_EditorMouseDown), true);

            //Disable dragging blocks to trash
            robotPalette.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(Handle_OtherMouseDown), true);
            blockPalette.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(Handle_OtherMouseDown), true);
            variablePalette.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(Handle_OtherMouseDown), true);
            methodPalette.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(Handle_OtherMouseDown), true);


            //trying to reprint row numbers
            /*ICollectionView cv = CollectionViewSource.GetDefaultView(listviewNames.ItemsSource);
            if (listviewNames.Items.CurrentItem != null)
            {
                mySourceList.RemoveAt(cv.CurrentPosition);
                cv.Refresh();
            }*/

            //TEST AREA//
           /* List<TestBlock> tester = new List<TestBlock>();
            for (int i = 0; i < programStructureList.Count; i++)
            {              
                TestBlock x = new TestBlock(programStructureList[i].Tex, programStructureList[i].blockColor);
                tester.Add(x);
            }
            testZone.ItemsSource = tester;

            //END TEST/*/
        }

        public void Handle_OtherMouseDown(object sender, MouseEventArgs args)
        {
            trashDrapDrop.AllowAdd = false;
        }

        public void Handle_EditorMouseDown(object sender, MouseEventArgs args)
        {
            ListBox listBox = sender as ListBox;
            if (listBox.SelectedItem != null)
            {
                trashDrapDrop.AllowAdd = true;
                ((Block)listBox.SelectedItem).index = listBox.SelectedIndex;
            }

            /*/////TEST AREA/////////
            infoTest.Items.Add(editorPalette.Items.ElementAt(0));
            TabItem x = new TabItem();
            x.Header = "sandwich";
            ListBox temp = new ListBox();
            temp.Name = "temp";
            x.Content = temp;
            infoTabs.Items.Add(x);
            temp.Items.Add(editorPalette.Items.ElementAt(0));
            //////////END TEST/////*/
        }

        private void Handle_ButtonOnClick(object sender, EventArgs args)
        {
            ListBox listBox = sender as ListBox;
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
                infoTest.Items.Add(editorPalette.Items.ElementAt(i));
            }
            
            //Bind(this.editorPalette, DragDropList);
            
        }
    }
}