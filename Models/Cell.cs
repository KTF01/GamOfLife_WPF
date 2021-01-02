using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GameOfLife.Models
{
    class Cell
    {
        public bool IsAlive { get;set; }
        public Cell()
        {
            IsAlive = false;
        }
        public void SwitchStatus()
        {
            if (IsAlive) IsAlive = false;
            else IsAlive = true;
        }

    }
}
