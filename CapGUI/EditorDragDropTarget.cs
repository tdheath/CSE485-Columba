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

namespace CapGUI
{
    public class EditorDragDropTarget : ListBoxDragDropTarget
    {   
        protected override void OnDropOverride(Microsoft.Windows.DragEventArgs args)
        {
            bool canAddItemFlag = true;
            object data = args.Data.GetData(args.Data.GetFormats()[0]);
            
            //cast from generic object to ItemDragEventArgs and add to SelectionCollection
            ItemDragEventArgs itemDragEventArgs = data as ItemDragEventArgs;
            SelectionCollection selectionCollection = itemDragEventArgs.Data as SelectionCollection;
            
            //get the target listbox from DragEventArgs
            ListBox dropTarget = GetDropTarget(args);
            foreach (Selection selection in selectionCollection)
            {
                if (!CanAddItem(dropTarget, selection.Item))
                {
                    canAddItemFlag = false;
                }
            }

            if (dropTarget != null && canAddItemFlag)
            {
                int? index = GetDropTargetInsertionIndex(dropTarget, args);
                Debug.WriteLine("Index: " + index);
                if (index != null)
                {
                    foreach (Selection selection in selectionCollection)
                    {
                        if (selection.Item.GetType().Equals(typeof(TestingBlock)))
                        {
                            ((TestingBlock)selection.Item).Index = (int)index;
                        }
                    }
                }
            }
            
            base.OnDropOverride(args);
            var dropTargetList = dropTarget.Items.Cast<TestingBlock>().ToList();
            int i = 0;
            foreach (TestingBlock block in dropTargetList)
            {
                Debug.WriteLine("Name: " + block.Name);
                block.Index = i++;
                Debug.WriteLine("Index: " + block.Index);
            }
            //dropTarget.ItemsSource = null;
            //dropTarget.ItemsSource = dropTargetList;
        }
    }
}
