﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Materia.MathHelpers;

namespace Materia.Nodes.MathNodes
{
    public class RandomNode : MathNode
    {
        NodeInput input;
        NodeOutput output;

        public RandomNode(int w, int h, GraphPixelType p = GraphPixelType.RGBA) : base()
        {
            //we ignore w,h,p

            CanPreview = false;

            Name = "Random";
            Id = Guid.NewGuid().ToString();
            shaderId = "S" + Id.Split('-')[0];

            input = new NodeInput(NodeType.Float | NodeType.Float2, this, "Float Input");
            output = new NodeOutput(NodeType.Float, this);

            Inputs.Add(input);

            input.OnInputAdded += Input_OnInputAdded;
            input.OnInputChanged += Input_OnInputChanged;

            Outputs.Add(output);
        }

        private void Input_OnInputChanged(NodeInput n)
        {
            TryAndProcess();
        }

        private void Input_OnInputAdded(NodeInput n)
        {
            Updated();
        }

        public override void TryAndProcess()
        {
            if (input.HasInput)
            {
                Process();
            }
        }

        public override string GetShaderPart(string currentFrag)
        {
            if (!input.HasInput) return "";

            int seed = 0;

            if (ParentGraph != null)
            {
                seed = ParentGraph.RandomSeed;
            }

            var s = shaderId + "1";
            var n1id = (input.Input.Node as MathNode).ShaderId;

            var index = input.Input.Node.Outputs.IndexOf(input.Input);

            n1id += index;

            if (input.Input.Type == NodeType.Float2)
            {
                return "float " + s + " = abs(rand(" + n1id + " + " + seed + "));\r\n";
            }
            else
            { 
                return "float " + s + " = abs(rand(pos + " + seed + ")) * " + n1id + ";\r\n";
            }
        }

        float fract(float f)
        {
            return f - (float)Math.Floor(f);
        }

        float rand(ref MVector vec2)
        {
            return fract((float)Math.Sin(MVector.Dot(vec2, new MVector(12.9898f, 78.233f))) * 43758.5453f);
        }

        void Process()
        {
            if (input.Input.Data == null) return;

            object o = input.Input.Data;

            float seed = 0;

            if(ParentGraph != null)
            {
                seed = ParentGraph.RandomSeed;
            }

            if (o is float || o is int || o is double)
            {
                float v = Convert.ToSingle(o);
                MVector v2 = new MVector(v, 1.0f - v) + seed;
                output.Data = rand(ref v2);
                if (Outputs.Count > 0)
                {
                    Outputs[0].Changed();
                }
            }
            else if(o is MVector)
            {
                MVector v = (MVector)o + seed;
                output.Data = rand(ref v);
                if (Outputs.Count > 0)
                {
                    Outputs[0].Changed();
                }
            }
            else
            {
                output.Data = 0;
                if (Outputs.Count > 0)
                {
                    Outputs[0].Changed();
                }
            }

            if (ParentGraph != null)
            {
                FunctionGraph g = (FunctionGraph)ParentGraph;

                if (g != null && g.OutputNode == this)
                {
                    g.Result = output.Data;
                }
            }
        }
    }
}
