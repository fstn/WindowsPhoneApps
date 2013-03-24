﻿#region Header
//
//   Project:           WriteableBitmapEx - WriteableBitmap extensions
//   Description:       Collection of blit (copy) extension methods for the WriteableBitmap class.
//
//   Changed by:        $Author$
//   Changed on:        $Date$
//   Changed in:        $Revision$
//   Project:           $URL$
//   Id:                $Id$
//
//
//   Copyright © 2009-2012 Bill Reiss, Rene Schulte and WriteableBitmapEx Contributors
//
//   This Software is weak copyleft open source. Please read the License.txt for details.
//
#endregion

using System;

#if NETFX_CORE
using Windows.Foundation;

namespace Windows.UI.Xaml.Media.Imaging
#else
namespace System.Windows.Media.Imaging
#endif
{
   /// <summary>
   /// Collection of blit (copy) extension methods for the WriteableBitmap class.
   /// </summary>
   public
#if WPF
    unsafe
#endif
 static partial class WriteableBitmapExtensions
   {
      #region Enum

      /// <summary>
      /// The blending mode.
      /// </summary>
      public enum BlendMode
      {
         /// <summary>
         /// Alpha blendiing uses the alpha channel to combine the source and destination. 
         /// </summary>
         Alpha,

         /// <summary>
         /// Additive blending adds the colors of the source and the destination.
         /// </summary>
         Additive,

         /// <summary>
         /// Subtractive blending subtracts the source color from the destination.
         /// </summary>
         Subtractive,

         /// <summary>
         /// Uses the source color as a mask.
         /// </summary>
         Mask,

         /// <summary>
         /// Multiplies the source color with the destination color.
         /// </summary>
         Multiply,

         /// <summary>
         /// Ignores the specified Color
         /// </summary>
         ColorKeying,

         /// <summary>
         /// No blending just copies the pixels from the source.
         /// </summary>
         None
      }

      #endregion

      #region Methods

      /// <summary>
      /// Copies (blits) the pixels from the WriteableBitmap source to the destination WriteableBitmap (this).
      /// </summary>
      /// <param name="bmp">The destination WriteableBitmap.</param>
      /// <param name="destRect">The rectangle that defines the destination region.</param>
      /// <param name="source">The source WriteableBitmap.</param>
      /// <param name="sourceRect">The rectangle that will be copied from the source to the destination.</param>
      /// <param name="BlendMode">The blending mode <see cref="BlendMode"/>.</param>
      public static void Blit(this WriteableBitmap bmp, Rect destRect, WriteableBitmap source, Rect sourceRect, BlendMode BlendMode)
      {
         Blit(bmp, destRect, source, sourceRect, Colors.White, BlendMode);
      }

      /// <summary>
      /// Copies (blits) the pixels from the WriteableBitmap source to the destination WriteableBitmap (this).
      /// </summary>
      /// <param name="bmp">The destination WriteableBitmap.</param>
      /// <param name="destRect">The rectangle that defines the destination region.</param>
      /// <param name="source">The source WriteableBitmap.</param>
      /// <param name="sourceRect">The rectangle that will be copied from the source to the destination.</param>
      public static void Blit(this WriteableBitmap bmp, Rect destRect, WriteableBitmap source, Rect sourceRect)
      {
         Blit(bmp, destRect, source, sourceRect, Colors.White, BlendMode.Alpha);
      }

      /// <summary>
      /// Copies (blits) the pixels from the WriteableBitmap source to the destination WriteableBitmap (this).
      /// </summary>
      /// <param name="bmp">The destination WriteableBitmap.</param>
      /// <param name="destPosition">The destination position in the destination bitmap.</param>
      /// <param name="source">The source WriteableBitmap.</param>
      /// <param name="sourceRect">The rectangle that will be copied from the source to the destination.</param>
      /// <param name="color">If not Colors.White, will tint the source image. A partially transparent color and the image will be drawn partially transparent.</param>
      /// <param name="BlendMode">The blending mode <see cref="BlendMode"/>.</param>
      public static void Blit(this WriteableBitmap bmp, Point destPosition, WriteableBitmap source, Rect sourceRect, Color color, BlendMode BlendMode)
      {
         Rect destRect = new Rect(destPosition, new Size(sourceRect.Width, sourceRect.Height));
         Blit(bmp, destRect, source, sourceRect, color, BlendMode);
      }

      /// <summary>
      /// Copies (blits) the pixels from the WriteableBitmap source to the destination WriteableBitmap (this).
      /// </summary>
      /// <param name="bmp">The destination WriteableBitmap.</param>
      /// <param name="destRect">The rectangle that defines the destination region.</param>
      /// <param name="source">The source WriteableBitmap.</param>
      /// <param name="sourceRect">The rectangle that will be copied from the source to the destination.</param>
      /// <param name="color">If not Colors.White, will tint the source image. A partially transparent color and the image will be drawn partially transparent. If the BlendMode is ColorKeying, this color will be used as color key to mask all pixels with this value out.</param>
      /// <param name="BlendMode">The blending mode <see cref="BlendMode"/>.</param>
      public static void Blit(this WriteableBitmap bmp, Rect destRect, WriteableBitmap source, Rect sourceRect, Color color, BlendMode BlendMode)
      {
         if (color.A == 0)
         {
            return;
         }
         int dw = (int)destRect.Width;
         int dh = (int)destRect.Height;

         using (var srcContext = source.GetBitmapContext())
         {
            using (var destContext = bmp.GetBitmapContext())
            {
               int sourceWidth = srcContext.Width;
               int dpw = destContext.Width;
               int dph = destContext.Height;
               Rect intersect = new Rect(0, 0, dpw, dph);
               intersect.Intersect(destRect);
               if (intersect.IsEmpty)
               {
                  return;
               }

               var sourcePixels = srcContext.Pixels;
               var destPixels = destContext.Pixels;
               int sourceLength = srcContext.Length;
               int destLength = destContext.Length;
               int sourceIdx = -1;
               int px = (int)destRect.X;
               int py = (int)destRect.Y;
               int right = px + dw;
               int bottom = py + dh;
               int x;
               int y;
               int idx;
               double ii;
               double jj;
               int sr = 0;
               int sg = 0;
               int sb = 0;
               int dr, dg, db;
               int sourcePixel;
               int sa = 0;
               int da;
               int ca = color.A;
               int cr = color.R;
               int cg = color.G;
               int cb = color.B;
               bool tinted = color != Colors.White;
               var sw = (int)sourceRect.Width;
               var sdx = sourceRect.Width / destRect.Width;
               var sdy = sourceRect.Height / destRect.Height;
               int sourceStartX = (int)sourceRect.X;
               int sourceStartY = (int)sourceRect.Y;
               int lastii, lastjj;
               lastii = -1;
               lastjj = -1;
               jj = sourceStartY;
               y = py;
               for (int j = 0; j < dh; j++)
               {
                  if (y >= 0 && y < dph)
                  {
                     ii = sourceStartX;
                     idx = px + y * dpw;
                     x = px;
                     sourcePixel = sourcePixels[0];

                     // Scanline BlockCopy is much faster (3.5x) if no tinting and blending is needed,
                     // even for smaller sprites like the 32x32 particles. 
                     if (BlendMode == BlendMode.None && !tinted)
                     {
                        sourceIdx = (int)ii + (int)jj * sourceWidth;
                        var offset = x < 0 ? -x : 0;
                        var xx = x + offset;
                        var wx = sourceWidth - offset;
                        var len = xx + wx < dpw ? wx : dpw - xx;
                        if (len > sw) len = sw;
                        if (len > dw) len = dw;
                        BitmapContext.BlockCopy(srcContext, (sourceIdx + offset) * 4, destContext, (idx + offset) * 4, len * 4);
                     }

                     // Pixel by pixel copying
                     else
                     {
                        for (int i = 0; i < dw; i++)
                        {
                           if (x >= 0 && x < dpw)
                           {
                              if ((int)ii != lastii || (int)jj != lastjj)
                              {
                                 sourceIdx = (int)ii + (int)jj * sourceWidth;
                                 if (sourceIdx >= 0 && sourceIdx < sourceLength)
                                 {
                                    sourcePixel = sourcePixels[sourceIdx];
                                    sa = ((sourcePixel >> 24) & 0xff);
                                    sr = ((sourcePixel >> 16) & 0xff);
                                    sg = ((sourcePixel >> 8) & 0xff);
                                    sb = ((sourcePixel) & 0xff);
                                    if (tinted && sa != 0)
                                    {
                                       sa = (((sa * ca) * 0x8081) >> 23);
                                       sr = ((((((sr * cr) * 0x8081) >> 23) * ca) * 0x8081) >> 23);
                                       sg = ((((((sg * cg) * 0x8081) >> 23) * ca) * 0x8081) >> 23);
                                       sb = ((((((sb * cb) * 0x8081) >> 23) * ca) * 0x8081) >> 23);
                                       sourcePixel = (sa << 24) | (sr << 16) | (sg << 8) | sb;
                                    }
                                 }
                                 else
                                 {
                                    sa = 0;
                                 }
                              }
                              if (BlendMode == BlendMode.None)
                              {
                                 destPixels[idx] = sourcePixel;
                              }
                              else if (BlendMode == BlendMode.ColorKeying)
                              {
                                 sr = ((sourcePixel >> 16) & 0xff);
                                 sg = ((sourcePixel >> 8) & 0xff);
                                 sb = ((sourcePixel) & 0xff);

                                 if (sr != color.R || sg != color.G || sb != color.B)
                                 {
                                    destPixels[idx] = sourcePixel;
                                 }

                              }
                              else if (BlendMode == BlendMode.Mask)
                              {
                                 int destPixel = destPixels[idx];
                                 da = ((destPixel >> 24) & 0xff);
                                 dr = ((destPixel >> 16) & 0xff);
                                 dg = ((destPixel >> 8) & 0xff);
                                 db = ((destPixel) & 0xff);
                                 destPixel = ((((da * sa) * 0x8081) >> 23) << 24) |
                                             ((((dr * sa) * 0x8081) >> 23) << 16) |
                                             ((((dg * sa) * 0x8081) >> 23) << 8) |
                                             ((((db * sa) * 0x8081) >> 23));
                                 destPixels[idx] = destPixel;
                              }
                              else if (sa > 0)
                              {
                                 int destPixel = destPixels[idx];
                                 da = ((destPixel >> 24) & 0xff);
                                 if ((sa == 255 || da == 0) &&
                                    BlendMode != BlendMode.Additive
                                    && BlendMode != BlendMode.Subtractive
                                    && BlendMode != BlendMode.Multiply
                                    )
                                 {
                                    destPixels[idx] = sourcePixel;
                                 }
                                 else
                                 {
                                    dr = ((destPixel >> 16) & 0xff);
                                    dg = ((destPixel >> 8) & 0xff);
                                    db = ((destPixel) & 0xff);
                                    if (BlendMode == BlendMode.Alpha)
                                    {
                                       destPixel = ((((sa * sa) + ((255 - sa) * da)) >> 8) << 24) +
                                                   ((((sa * sr) + ((255 - sa) * dr)) >> 8) << 16) +
                                                   ((((sa * sg) + ((255 - sa) * dg)) >> 8) << 8) +
                                                   (((sa * sb) + ((255 - sa) * db)) >> 8);
                                    }
                                    else if (BlendMode == BlendMode.Additive)
                                    {
                                       int a = (255 <= sa + da) ? 255 : (sa + da);
                                       destPixel = (a << 24) |
                                          (((a <= sr + dr) ? a : (sr + dr)) << 16) |
                                          (((a <= sg + dg) ? a : (sg + dg)) << 8) |
                                          (((a <= sb + db) ? a : (sb + db)));
                                    }
                                    else if (BlendMode == BlendMode.Subtractive)
                                    {
                                       int a = da;
                                       destPixel = (a << 24) |
                                          (((sr >= dr) ? 0 : (sr - dr)) << 16) |
                                          (((sg >= dg) ? 0 : (sg - dg)) << 8) |
                                          (((sb >= db) ? 0 : (sb - db)));
                                    }
                                    else if (BlendMode == BlendMode.Multiply)
                                    {
                                       // Faster than a division like (s * d) / 255 are 2 shifts and 2 adds
                                       int ta = (sa * da) + 128;
                                       int tr = (sr * dr) + 128;
                                       int tg = (sg * dg) + 128;
                                       int tb = (sb * db) + 128;

                                       int ba = ((ta >> 8) + ta) >> 8;
                                       int br = ((tr >> 8) + tr) >> 8;
                                       int bg = ((tg >> 8) + tg) >> 8;
                                       int bb = ((tb >> 8) + tb) >> 8;

                                       destPixel = (ba << 24) |
                                                   ((ba <= br ? ba : br) << 16) |
                                                   ((ba <= bg ? ba : bg) << 8) |
                                                   ((ba <= bb ? ba : bb));
                                    }

                                    destPixels[idx] = destPixel;
                                 }
                              }
                           }
                           x++;
                           idx++;
                           ii += sdx;
                        }
                     }
                  }
                  jj += sdy;
                  y++;
               }
            }
         }
      }

      #endregion
   }
}