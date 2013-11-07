using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CapGUI
{
    /// <summary>
    /// Represents an actual block read in from the API.
    /// </summary>
    public class Block
    {
        //Actual name of the block (IF-Block, While-Block, etc.)
        public string blockName;
        //Color the block is rendered in
        public Color blockColor;
        //Index in the editor window
        public int index { get; set; }
        //further metadata can go here-- package name, maybe function?, etc.

        //Holds the types / names of each of the fields in the block. (e.g. [String, Block, ...])
        public List<String> typeFieldList;
        ///Holds the actual values of the fields defined by typeFieldList. (e.g. ["If", ConditionBlockInstance, ...])
        public List<Object> dataList;

        public Block(string newBlockName, Color newBlockColor)
        {
            blockName = newBlockName;
            blockColor = newBlockColor;

            typeFieldList = new List<String>();
            dataList = new List<Object>();

            index = -1;
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
            return blockName + "  Index: " + index;
        }

    }
}
