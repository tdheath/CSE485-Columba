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

namespace CapGUI
{
    public partial class Block : UserControl
    {
        //Actual name of the block (IF-Block, While-Block, etc.)
        private string blockName;
        public string Text { get { return blockName; } set { blockName = value; } }
        //Color the block is rendered in
        private Color privateColor;
        public Color blockColor// { get; set; }
        {
            get
            {
                return privateColor;
            }
            set
            {
                privateColor = value;
                LayoutRoot.Background = new SolidColorBrush(privateColor);
            }
        }
        //Index in the editor window
        public int index { get; set; }
        //Determines where the block can be placed-- STATEMENT, PLUGIN, STATEMENT/PLUGIN
        public string type { get; set; }
        //If this is a variable, method, or constant, the block's return type (STRING/INT/BOOLEAN)
        public string returnType { get; set; }

        //API Flags
        public bool flag_loopOnly { get; set; }                     //If true, this can only be socketed into a loop block 
        public bool flag_socketsMustMatch { get; set; }             //If true, all plugins must match each others' return types
        public bool flag_intDisabled { get; set; }                  //If true, plugins that return an INT can't be socketed here
        public bool flag_stringDisabled { get; set; }               //If true, plugins that return a STRING can't be socketed here
        public bool flag_booleanDisabled { get; set; }              //If true, plugins that return a BOOLEAN can't be socketed here

        //Holds the types / names of each of the fields in the block. (e.g. [String, Block, ...])
        public List<String> typeFieldList;
        ///Holds the actual values of the fields defined by typeFieldList. (e.g. ["If", ConditionBlockInstance, ...])
        public List<Object> dataList;

        public Block(string newBlockName, Color newBlockColor)
        {
            InitializeComponent();
            Text = newBlockName;
            blockColor = newBlockColor;
            LayoutRoot.Background = new SolidColorBrush(newBlockColor);
            fore.Text = Text;

            typeFieldList = new List<String>();
            dataList = new List<Object>();

            index = -1;
            initFlags(); 
        }

        public Block(string newBlockName)
        {
            InitializeComponent();
            Text = newBlockName;
            blockColor = Colors.White;
            LayoutRoot.Background = new SolidColorBrush(blockColor);
            fore.Text = Text;

            typeFieldList = new List<String>();
            dataList = new List<Object>();

            index = -1;
            initFlags();
        }
        
        /// <summary>
        /// Private method to default all flags to false
        /// </summary>
        private void initFlags()
        {
            flag_loopOnly = false;
            flag_socketsMustMatch = false;
            flag_intDisabled = false;
            flag_stringDisabled = false;
            flag_booleanDisabled = false;
        }
        
        /// <summary>
        /// Appends a new field and data to the end of the block's definition.
        /// Use this when constructing a new instance of a block (in the editor window, etc.)
        /// </summary>
        /// Note that non-template blocks in the editor window can change dynamically-- adding/chaining conditionals, etc.
        /// <param name="newTypeField">Name of the data type (String, Block, etc.)</param>
        /// <param name="newData">Actual data to store in the block ("If", instance, etc.)</param>
        public void addField(string newTypeField, Object newData)
        {
            typeFieldList.Add(newTypeField);
            dataList.Add(newData);
        }

        /// <summary>
        /// Appends a new field and data to the end of the block's definition.
        /// Use this when constructing block definitions (in blockPallete, etc.)
        /// </summary>
        /// <param name="newTypeField">Name of the data type (String, Block, etc.)</param>
        public void addField(string newTypeField)
        {
            typeFieldList.Add(newTypeField);
            dataList.Add(null);
        }

        /// <summary>
        /// Sets the data at a given index. (e.g. set the 'Condition' data field at index = 1 to a Block we provide)
        /// </summary>
        /// <param name="index">Index of the datafield to set</param>
        /// <param name="newData">Object to be set into the given index</param>
        public void setData(int index, object newData)
        {
            dataList[index] = newData;
        }

        public override string ToString()
        {
            return Text + " Index: " + index;
        }

    }
}
