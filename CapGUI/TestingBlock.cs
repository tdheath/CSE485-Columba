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

namespace CapGUI
{
    public class TestingBlock
    {
        public string Name { get; set; }
        public int Index { get; set; }

        public TestingBlock(String name)
        {
            Name = name;
            Index = -1;
        }
        public override string ToString()
        {
            return Name + "  Index: " + Index;
        }

    }
}
