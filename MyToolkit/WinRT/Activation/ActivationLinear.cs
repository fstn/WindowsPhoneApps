﻿using System;

namespace MyToolkit.MachineLearning.WinRT.Activation
{
    public class ActivationLinear : ActivationFunction
    {
		public override double Function(double input)
        {
			return input;
        }

		public override double DerivativeFunction(double input)
        {
			throw new NotSupportedException();
        }
    }
}
