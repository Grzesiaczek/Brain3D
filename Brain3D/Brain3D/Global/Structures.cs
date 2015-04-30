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

    enum Mode { Chart, Creation, Manual, Query }
    enum SequenceElementType { Normal, Built, Active, Activated, Receptor, ActiveReceptor}
    enum StateBarPhase { Idle, Activation, BalanceNormal, BalanceExtra}
    enum SpaceMode { Box, Sphere }

    #endregion

    class NeuronData
    {
        #region deklaracje

        bool active;
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