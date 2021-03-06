﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Materia.UI.Components
{
    /// <summary>
    /// Interaction logic for PropertySection.xaml
    /// </summary>
    public partial class PropertySection : UserControl
    {
        public string Title
        {
            get
            {
                return LabelContent.Text;
            }
            set
            {
                LabelContent.Text = value;
            }
        }

        public PropertySection()
        {
            InitializeComponent();
        }
    }
}
