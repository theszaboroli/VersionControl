﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorldsHardestGame;

namespace week10
{
    public partial class Form1 : Form
    {
        GameController gc = new GameController();
        GameArea ga;
        public Form1()
        {
            ga = gc.ActivateDisplay();
            this.Controls.Add(ga);
            InitializeComponent();

            gc.AddPlayer();
            gc.Start(true);

        }
    }
}
