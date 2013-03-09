using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace BrokeThePig.UC
{
    public class SoundController
    {
        public void PlaySound(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                using (var stream = TitleContainer.OpenStream(path))
                {
                    if (stream != null)
                    {
                        try
                        {

                            var effect = SoundEffect.FromStream(stream);
                            FrameworkDispatcher.Update();
                            effect.Play();
                        }
                        catch (Exception e)
                        {
                            Debugger.Log(0,"sound","can't play: "+ path+" reson is : "+e.Message);
                        }
                    }
                }
            }
        }
    }
}
