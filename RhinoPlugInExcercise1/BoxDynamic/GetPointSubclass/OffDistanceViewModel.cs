namespace RhinoPlugInExcercise1
{
    internal class OffDistanceViewModel:BaseViewModel
    {
        public OffDistanceViewModel()
        {
            Input1 = 0.6;
            Input2 = -0.6;
        }

        private double input1;

        public double Input1
        {
            get { return input1; }
            set
            {
                if (input1 != value)
                {
                    input1 = value;
                    OnPropertyChanged(nameof(Input1));
                }
            }
        }

        private double input2;

        public double Input2
        {
            get { return input2; }
            set
            {
                if (input2 != value)
                {
                    input2 = value;
                    OnPropertyChanged(nameof(Input2));
                }
            }
        }

    }
}