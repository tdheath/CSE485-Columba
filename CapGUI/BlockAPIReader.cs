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
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;

namespace CapGUI
{
    /// <summary>
    /// Reads in blocks from the API file provided (XML, etc.) and creates lists of block definitions in our system
    /// </summary>
    public class BlockAPIReader
    {

        //List of names for each package read in
        private List<String> packageNames;
        //List of Lists of blocks contained in each package (e.g. index 0=list of blocks in package0)
        //List<List<Block>> packageBlocks;

        public BlockAPIReader()
        {
            packageNames = new List<String>();
        }

        /// <summary>
        /// Reads in blocks from an API file.
        /// </summary>
        /// <returns>List of block packages, each containing a list of blocks</returns>
        public List<List<Block>> readBlockDefinitions()
        {
            List<List<Block>> blockList = new List<List<Block>>();

            try
            {
                //Get and create the XML document to read from
                String path = Directory.GetCurrentDirectory() + "\\blockAPI.xml";
                XDocument apiDoc = XDocument.Load(path);

                //Get an enumerator for each package node
                IEnumerable<XElement> fullAPI = apiDoc.Elements();
                IEnumerable<XElement> packages = fullAPI.Elements();


                foreach (var package in packages)
                {
                    //Gets each package
                    String pkgName = package.Attribute("name").Value.ToString();
                    packageNames.Add(pkgName);

                    IEnumerable<XElement> blocks = package.Elements();
                    List<Block> packageBlocksList = new List<Block>();

                    foreach (var block in blocks)
                    {
                        //Gets each block in the package
                        packageBlocksList.Add(readBlock(block));
                    }

                    //Finally, add the list created for each package to the API list
                    blockList.Add(packageBlocksList);

                }
            }
            catch (IOException e)
            {
                Debug.WriteLine("File not found.");
                Debug.WriteLine(e.StackTrace);
            }

            return blockList;
        }

        /// <summary>
        /// Used by readBlockDefinitions to handle individual block creation.
        /// </summary>
        /// <param name="b">block node</param>
        /// <returns>Actual API block to add to the list.</returns>
        private Block readBlock(XElement b)
        {
            Block newBlock = new Block(b.Element("name").Value);
            newBlock.type = b.Element("type").Value;

            IEnumerable<XElement> contains = b.Element("contains").Elements();
            IEnumerable<XElement> properties = b.Element("properties").Elements();

            //Add block fields
            foreach (var field in contains)
            {
                //string, socket, or textbox
                newBlock.addField(field.Name.ToString());
            }

            //Set block flags
            foreach (var flag in properties)
            {
                bool flagValue = false;

                switch (flag.Name.ToString())
                {
                    case "color":
                        //newBlock.color = flag.Value.ToString();
                        break;
                    case "loopOnly":
                        if (Boolean.TryParse(flag.Value.ToString(), out flagValue))
                            newBlock.flag_loopOnly = flagValue;
                        else
                            Debug.WriteLine("Parse failed for " + flag.Value.ToString());
                        break;

                    case "socketsMustMatch":
                        if (Boolean.TryParse(flag.Value.ToString(), out flagValue))
                            newBlock.flag_socketsMustMatch = flagValue;
                        else
                            Debug.WriteLine("Parse failed for " + flag.Value.ToString());
                        break;

                    case "intDisabled":
                        if (Boolean.TryParse(flag.Value.ToString(), out flagValue))
                            newBlock.flag_intDisabled = flagValue;
                        else
                            Debug.WriteLine("Parse failed for " + flag.Value.ToString());
                        break;

                    case "stringDisabled":
                        if (Boolean.TryParse(flag.Value.ToString(), out flagValue))
                            newBlock.flag_stringDisabled = flagValue;
                        else
                            Debug.WriteLine("Parse failed for " + flag.Value.ToString());
                        break;

                    case "booleanDisabled":
                        if (Boolean.TryParse(flag.Value.ToString(), out flagValue))
                            newBlock.flag_booleanDisabled = flagValue;
                        else
                            Debug.WriteLine("Parse failed for " + flag.Value.ToString());
                        break;

                    default:
                        Debug.WriteLine("Unrecognized or unimplemented flag: " + flag.Name.ToString());
                        break;
                }
            }

            return newBlock;
        }



        /// <summary>
        /// Use after readBlockDefinitions(). Gets package names
        /// </summary>
        /// <returns>List of string names for each package. </returns>
        public List<String> getPackageNames()
        {
            return packageNames;
        }
    }
}
