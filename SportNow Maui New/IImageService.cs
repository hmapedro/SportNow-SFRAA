using System;
namespace SportNow
{
    public interface IImageService
    {
        Task<FileResult> CapturePhotoAsync(MediaPickerOptions options = null);

        Task<FileResult> PickPhotoAsync(MediaPickerOptions options = null);
    }
}

