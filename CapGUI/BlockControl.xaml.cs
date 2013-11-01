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
    /// <summary>
    /// Controls the look and feel of a generic block, and handles rendering its contents (prompts, internal blocks, etc.)
    /// </summary>
    public partial class BlockControl : UserControl
    {

        Block block;
        int numFields;
        Color blockColor;

        public BlockControl(Block newBlock)
        {
            InitializeComponent();
            block = newBlock;

            //Set up basic look and feel-- get color from block, define rectangle
            blockColor = block.blockColor;
            //

            //Collect the number of fields in the block & partition space equally according to number of fields
            numFields = block.typeFieldList.Count;
            //

            for (int i = 0; i < numFields; i++)
            {
                //Render each sub component according to its type
                string currentField = block.typeFieldList[i];
                //Todo: switch is case sensitive-- decide if we want case sensitivity among type names, get list of valid type names
                switch (currentField)
                {
                    case "String":
                        //Render text in this partition
                        break;
                    case "Block":
                        //Create a new BlockControl for the subBlock and have it handle its own rendering. Just provide a space to render in
                        break;
                    case "Condition":   //?
                        //Render conditional here. Enforce type restriction elsewhere. 
                        break;
                    case "DropMenu":
                        //Render dropdown menu here-- user can select multiple values
                        break;
                    case "Textbox":
                        //Render textbox here-- user can enter in values
                        break;

                    default:
                        //Unknown / Missing type definition. Log and throw an error.
                        break;
                }
            }

            //Any final rendering code

        }

        private void InitializeComponent()
        {
            //throw new NotImplementedException();
        }

    }
}
