using System;

namespace PidSharp
{
    /// <summary>
    /// A (P)roportional, (I)ntegral, (D)erivative Controller
    /// </summary>
    /// <remarks>
    /// The controller should be able to control any process with a
    /// measureable value, a known ideal value and an input to the
    /// process that will affect the measured value.
    /// </remarks>
    /// <see cref="https://en.wikipedia.org/wiki/PID_controller"/>
    public sealed class PidController
    {
        private double processVariable = 0;
        DateTime lastcall;

        public PidController(double GainProportional, double GainIntegral, double GainDerivative, double OutputMax, double OutputMin)
        {
            this.D = GainDerivative;
            this.I = GainIntegral;
            this.P = GainProportional;
            this.OutputMax = OutputMax;
            this.OutputMin = OutputMin;
            lastcall = DateTime.Now;
        }

        /// <summary>
        /// The controller output
        /// </summary>
        public double ControlOutput
        {
            get
            {
                TimeSpan timeSinceLastUpdate = DateTime.Now - lastcall;
                lastcall = DateTime.Now;
                double error = TargetValue - CurrentValue;

                // integral term calculation
                IntegralTerm += (I * error * timeSinceLastUpdate.TotalSeconds);
                IntegralTerm = Clamp(IntegralTerm);

                // derivative term calculation
                double dInput = processVariable - ProcessVariableLast;
                double derivativeTerm = D * (dInput / timeSinceLastUpdate.TotalSeconds);

                // proportional term calcullation
                double proportionalTerm = P * error;

                double output = proportionalTerm + IntegralTerm - derivativeTerm;

                output = Clamp(output);

                return output;

            }
        }

        /// <summary>
        /// The derivative term is proportional to the rate of
        /// change of the error
        /// </summary>
        public double D { get; set; } = 0;

        /// <summary>
        /// The integral term is proportional to both the magnitude
        /// of the error and the duration of the error
        /// </summary>
        public double I { get; set; } = 0;

        /// <summary>
        /// The proportional term produces an output value that
        /// is proportional to the current error value
        /// </summary>
        /// <remarks>
        /// Tuning theory and industrial practice indicate that the
        /// proportional term should contribute the bulk of the output change.
        /// </remarks>
        public double P { get; set; } = 0;

        /// <summary>
        /// The max output value the control device can accept.
        /// </summary>
        public double OutputMax { get; private set; } = 0;

        /// <summary>
        /// The minimum ouput value the control device can accept.
        /// </summary>
        public double OutputMin { get; private set; } = 0;

        /// <summary>
        /// Adjustment made by considering the accumulated error over time
        /// </summary>
        /// <remarks>
        /// An alternative formulation of the integral action, is the
        /// proportional-summation-difference used in discrete-time systems
        /// </remarks>
        public double IntegralTerm { get; private set; } = 0;


        /// <summary>
        /// The current value
        /// </summary>
        public double CurrentValue
        {
            get { return processVariable; }
            set
            {
                ProcessVariableLast = processVariable;
                processVariable = value;
            }
        }

        /// <summary>
        /// The last reported value (used to calculate the rate of change)
        /// </summary>
        public double ProcessVariableLast { get; private set; } = 0;

        /// <summary>
        /// The desired value
        /// </summary>
        public double TargetValue { get; set; } = 0;

        /// <summary>
        /// Limit a variable to the set OutputMax and OutputMin properties
        /// </summary>
        /// <returns>
        /// A value that is between the OutputMax and OutputMin properties
        /// </returns>
        /// <remarks>
        /// Inspiration from http://stackoverflow.com/questions/3176602/how-to-force-a-number-to-be-in-a-range-in-c
        /// </remarks>
        private double Clamp(double variableToClamp)
        {
            if (variableToClamp <= OutputMin) { return OutputMin; }
            if (variableToClamp >= OutputMax) { return OutputMax; }
            return variableToClamp;
        }
    }
}
