using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Brain3D
{
    #region enumeratory

    enum SpaceMode { Box, Sphere, Chart }

    #endregion

    class Change
    {
        float start;
        float finish;
        float change;

        public Change(float value)
        {
            start = value;
            finish = value;
        }

        public Change(float start, float finish)
        {
            this.start = start;
            this.finish = finish;
            change = finish - start;
        }

        public float Start
        {
            get
            {
                return start;
            }
        }

        public float Finish
        {
            get
            {
                return finish;
            }
        }

        public float Value
        {
            get
            {
                return change;
            }
        }
    }

    class NeuronData
    {
        #region deklaracje

        bool active;
        bool treshold;

        double value;
        double refraction;

        #endregion

        #region konstruktory

        public NeuronData()
        {
            active = false;
            value = 0;
        }

        public NeuronData(bool active, double value, double refraction)
        {
            this.active = active;
            this.value = value;
            this.refraction = refraction;
        }

        #endregion

        #region właściwości

        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        public bool Treshold
        {
            get
            {
                return treshold;
            }
            set
            {
                treshold = value;
            }
        }

        public double Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        public double Refraction
        {
            get
            {
                return refraction;
            }
            set
            {
                refraction = value;
            }
        }

        #endregion
    }
}