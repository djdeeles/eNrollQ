using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Enroll.Managers;

namespace eNroll.Helpers
{
    /// <summary>
    /// 	Summary description for ImageHelper
    /// </summary>
    public class ImageHelper
    {
        public static Image ResizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = (size.Width/(float) sourceWidth);
            nPercentH = (size.Height/(float) sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            var destWidth = (int) (sourceWidth*nPercent);
            var destHeight = (int) (sourceHeight*nPercent);

            var b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }


        public static bool SaveJpeg(string path, Bitmap img, long quality)
        {
            try
            {
                // Encoder parameter for image quality
                var qualityParam = new EncoderParameter(Encoder.Quality, quality);
                // Jpeg image codec
                ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
                if (jpegCodec == null)
                    return false;
                var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = qualityParam;
                img.Save(path, jpegCodec, encoderParams);
                img.Dispose();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                return false;
            }
            return true;
        }

        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            try
            {
                // Get image codecs for all image formats
                var codecs = ImageCodecInfo.GetImageEncoders();
                // Find the correct image codec
                for (int i = 0; i < codecs.Length; i++)
                    if (codecs[i].MimeType == mimeType)
                        return codecs[i];
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return null;
        }

        public static void DeleteImage(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }
}