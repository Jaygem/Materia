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
using Materia.Nodes;

namespace Materia.UI.Components
{
    /// <summary>
    /// Interaction logic for CustomParameter.xaml
    /// </summary>
    public partial class GraphParameter : UserControl
    {
        public delegate void Remove(GraphParameter c);
        public event Remove OnRemove;

        public GraphParameterValue Param { get; protected set; }

        public string Id { get; protected set; }

        public GraphParameter()
        {
            InitializeComponent();
        }


        public GraphParameter(GraphParameterValue v, string id = "")
        {
            InitializeComponent();
            Param = v;
            Params.Set(v, false);
            DisplayDefaultParam();
            Id = id;
        }

        private void GraphParameterValue_OnGraphParameterTypeChanged(GraphParameterValue param)
        {
            if (param == Param)
            {
                DisplayDefaultParam();
            }
        }

        public void Set(GraphParameterValue v, string id = "")
        {
            Param = v;
            Params.Set(v, false, "Type");
            Id = id;
            DisplayDefaultParam();
        }

        protected void DisplayDefaultParam()
        {
            ParamDefaultStack.Children.Clear();
            var p = new ParameterMap(new List<GraphParameterValue>(new GraphParameterValue[] { Param }), true);
            ParamDefaultStack.Children.Add(p);
        }

        private void RemoveParam_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Remove Paramater: " + Param.Name, "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                if (OnRemove != null)
                {
                    OnRemove.Invoke(this);
                }
            }
        }
    }
}
