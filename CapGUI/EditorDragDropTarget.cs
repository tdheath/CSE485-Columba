using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.ComponentModel;


namespace CapGUI
{
    public class EditorDragDropTarget : ListBoxDragDropTarget
    {   
        protected override void OnDropOverride(Microsoft.Windows.DragEventArgs args)
        {
            if ((args.AllowedEffects & Microsoft.Windows.DragDropEffects.Link) == Microsoft.Windows.DragDropEffects.Link
                   || (args.AllowedEffects & Microsoft.Windows.DragDropEffects.Move) == Microsoft.Windows.DragDropEffects.Move)
            {
                //changed
                //gets the data format which is a ItemDragEventArgs
                object data = args.Data.GetData(args.Data.GetFormats()[0]);

                //changed
                //cast from generic object to ItemDragEventArgs and add to SelectionCollection
                ItemDragEventArgs itemDragEventArgs = data as ItemDragEventArgs;
                SelectionCollection selectionCollection = itemDragEventArgs.Data as SelectionCollection;

                //changed
                //get the target listbox from DragEventArgs
                ListBox dropTarget = GetDropTarget(args);


                if (dropTarget != null && selectionCollection.All(selection => CanAddItem(dropTarget, selection.Item)))
                {
                    if ((args.Effects & Microsoft.Windows.DragDropEffects.Move) == Microsoft.Windows.DragDropEffects.Move)
                    {
                        args.Effects = Microsoft.Windows.DragDropEffects.Move;
                    }
                    else
                    {
                        args.Effects = Microsoft.Windows.DragDropEffects.Link;
                    }

                    int? index = GetDropTargetInsertionIndex(dropTarget, args);

                    if (index != null)
                    {
                        if (args.Effects == Microsoft.Windows.DragDropEffects.Move && itemDragEventArgs != null && !itemDragEventArgs.DataRemovedFromDragSource)
                        {
                            itemDragEventArgs.RemoveDataFromDragSource();
                        }

                        //major change place at top of the listbox to act as a stack for undo
                        foreach (Selection selection in selectionCollection)
                        {
                            
                            if (selection.Item.GetType().Equals(typeof(Block)))
                            {
                                Block currentTestBlock = new Block(((Block)selection.Item).Text, ((Block)selection.Item).blockColor);
                                currentTestBlock.index = index.Value;
                                InsertItem(dropTarget, index.Value, currentTestBlock);
                                for (int i = 0; i < index.Value; i++)
                                {
                                    ((Block)dropTarget.Items[i]).index = i;
                                }
                                
                            }
                            

                            /*var dropTargetList = dropTarget.Items.Cast<TestBlock>().ToList();
                            int i = 0;
                            foreach (TestBlock TestBlock in dropTargetList)
                            {
                                Debug.WriteLine("Name: " + TestBlock.TestBlockName);
                                TestBlock.index = i++;
                                Debug.WriteLine("Index: " + TestBlock.index);
                            }*/
                        }
                    }
                }
                else
                {
                    args.Effects = Microsoft.Windows.DragDropEffects.None;
                }

                if (args.Effects != args.AllowedEffects)
                {
                    args.Handled = true;
                }
            }
        }

        /*protected override bool CanAddItem(ListBox itemsControl, object data)
        {
            if (itemsControl.ItemsSource == null)
            {
                return true;
            }
            bool caninsert = CanInsert(itemsControl.ItemsSource, data);
            return caninsert;
        }

        private static bool CanInsert(System.Collections.IEnumerable collection, object data)
        {
            ICollectionView collectionView = collection as ICollectionView;
            if (collectionView != null)
            {
                return CanInsert(collectionView.SourceCollection, data);
            }
            Type type = collection.GetType().GetInterfaces().Where(interfaceType => interfaceType.FullName.StartsWith("System.Collections.Generic.IList`1", StringComparison.Ordinal)).FirstOrDefault();
            if (type != null)
            {
                return type.GetGenericArguments()[0].IsAssignableFrom(data.GetType());
            }
            return collection is System.Collections.IList;
        }*/
    }
}
