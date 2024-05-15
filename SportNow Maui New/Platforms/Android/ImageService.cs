using Android.Graphics;
using AndroidX.ExifInterface.Media;
using Microsoft.Extensions.Logging;
using Path = System.IO.Path;
using Stream = System.IO.Stream;

namespace SportNow
{
    public enum ImageOrientation
    {
        Undefined = 0,
        Normal = 1,
        FlipHorizontal = 2,
        Rotate180 = 3,
        FlipVertical = 4,
        Transpose = 5,
        Rotate90 = 6,
        Transverse = 7,
        Rotate270 = 8
    }
    public partial class ImageService : IImageService
    {
        public ImageService()
        {
        }

        public async Task<FileResult> CapturePhotoAsync(MediaPickerOptions options = null)
        {
            var fileResult = await MediaPicker.Default.CapturePhotoAsync(options);
            if (fileResult == null)
                return null;
            await using var stream = await fileResult.OpenReadAsync();
            stream.Position = 0;
            var orientation = GetImageOrientation(stream);
            stream.Position = 0;
            var originalBitmap = await BitmapFactory.DecodeStreamAsync(stream);
            var matrix = new Matrix();
            switch (orientation)
            {
                case ImageOrientation.Normal:
                    break;
                case ImageOrientation.FlipHorizontal:
                    break;
                case ImageOrientation.Rotate180:
                    break;
                case ImageOrientation.FlipVertical:
                    matrix.PreRotate(180);
                    break;
                case ImageOrientation.Transpose:
                    matrix.PreRotate(90);
                    break;
                case ImageOrientation.Rotate90:
                    matrix.PreRotate(90);
                    break;
                case ImageOrientation.Transverse:
                    matrix.PreRotate(-90);
                    break;
                case ImageOrientation.Rotate270:
                    matrix.PreRotate(-90);
                    break;
            }
            var normalizedBitmap = Bitmap.CreateBitmap(
                originalBitmap,
                0,
                0,
                originalBitmap.Width,
                originalBitmap.Height,
                matrix,
                true);
            using var outStream = new MemoryStream();
            await normalizedBitmap.CompressAsync(Bitmap.CompressFormat.Jpeg, 100, outStream);
            outStream.Position = 0;
            var jpegFilename = Path.Combine(FileSystem.CacheDirectory, $"{Guid.NewGuid()}.jpg");
            await File.WriteAllBytesAsync(jpegFilename, outStream.ToArray());
            return new FileResult(jpegFilename);
        }

        public async Task<FileResult> PickPhotoAsync(MediaPickerOptions options = null)
        {
            var fileResult = await MediaPicker.Default.PickPhotoAsync(options);
            if (fileResult == null)
                return null;
            await using var stream = await fileResult.OpenReadAsync();
            stream.Position = 0;
            var orientation = GetImageOrientation(stream);
            stream.Position = 0;
            var originalBitmap = await BitmapFactory.DecodeStreamAsync(stream);
            var matrix = new Matrix();
            switch (orientation)
            {
                case ImageOrientation.Normal:
                    break;
                case ImageOrientation.FlipHorizontal:
                    break;
                case ImageOrientation.Rotate180:
                    break;
                case ImageOrientation.FlipVertical:
                    matrix.PreRotate(180);
                    break;
                case ImageOrientation.Transpose:
                    matrix.PreRotate(90);
                    break;
                case ImageOrientation.Rotate90:
                    matrix.PreRotate(90);
                    break;
                case ImageOrientation.Transverse:
                    matrix.PreRotate(-90);
                    break;
                case ImageOrientation.Rotate270:
                    matrix.PreRotate(-90);
                    break;
            }
            var normalizedBitmap = Bitmap.CreateBitmap(
                originalBitmap,
                0,
                0,
                originalBitmap.Width,
                originalBitmap.Height,
                matrix,
                true);
            using var outStream = new MemoryStream();
            await normalizedBitmap.CompressAsync(Bitmap.CompressFormat.Jpeg, 100, outStream);
            outStream.Position = 0;
            var jpegFilename = Path.Combine(FileSystem.CacheDirectory, $"{Guid.NewGuid()}.jpg");
            await File.WriteAllBytesAsync(jpegFilename, outStream.ToArray());
            return new FileResult(jpegFilename);
        }

        private ImageOrientation GetImageOrientation(Stream stream)
        {
            var exif = new ExifInterface(stream);
            var tag = exif.GetAttribute(ExifInterface.TagOrientation);
            var orientation = string.IsNullOrEmpty(tag) ?
                ImageOrientation.Undefined :
                (ImageOrientation)Enum.Parse(typeof(ImageOrientation), tag);
            exif.Dispose();

            return orientation;
        }
    }
}