// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RotateOut180CounterClockwiseAnimator.cs" company="XamlNinja">
//   2011 Richard Griffin and Ollie Riches
// </copyright>
// <summary>
// </summary>
// <credits>
//  Kevin Marshall http://blogs.claritycon.com/blog/2010/10/13/wp7-page-transitions-sample/
// </credits>
// <credits>
//  Silverlight toolkit http://silverlight.codeplex.com/
// </credits>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace MyToolkit.Animation.Transitions
{
	public class RotateOut180CounterClockwiseAnimator : RotateAnimator
    {
        #region Constants and Fields

        /// <summary>
        /// The storyboard.
        /// </summary>
        private static Storyboard storyboard;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RotateOut180CounterClockwiseAnimator"/> class. 
        /// </summary>
        public RotateOut180CounterClockwiseAnimator()
        {
            if (storyboard == null)
            {
                storyboard = XamlReader.Load(Storyboards.Rotate180CounterClockwiseOutStoryboard) as Storyboard;
            }

            this.Storyboard = storyboard;
        }

        #endregion
    }
}