
namespace Brain3D
{
    #region enumeratory

    enum SpaceMode { Box, Sphere, Chart }

    enum ActivityPhase { Normal, Active, Break, Start, Finish}

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

    class NeuronActivity
    {
        #region deklaracje

        ActivityPhase phase;

        double value;
        double refraction;

        #endregion

        #region konstruktory

        public NeuronActivity()
        {
            phase = ActivityPhase.Normal;
        }

        public NeuronActivity(ActivityPhase phase, double value, double refraction)
        {
            this.phase = phase;
            this.value = value;
            this.refraction = refraction;
        }

        public void Zero()
        {
            phase = ActivityPhase.Normal;
            value = 0;
            refraction = 0;
        }

        #endregion

        #region właściwości

        public ActivityPhase Phase
        {
            get
            {
                return phase;
            }
            set
            {
                phase = value;
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