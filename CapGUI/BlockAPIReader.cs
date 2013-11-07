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
    /// Reads in blocks from the API file provided (XML, etc.) and creates lists of block definitions in our system
    /// </summary>
    public class BlockAPIReader
    {
        public BlockAPIReader()
        {

        }

        /// <summary>
        /// Reads in blocks from an API file and returns a list of these blocks. 
        /// </summary>
        /// <returns></returns>
        public List<Block> readBlockDefinitions()
        {
            List<Block> blockList = new List<Block>();

            //Iterate through and parse XML etc. here
            /*
             * foreach XML node, 
             * create a block 
             * foreach field specified by the XML
             * add field to block
             * finally, add block to blockList
            */

            return blockList;
        }
    }
}
