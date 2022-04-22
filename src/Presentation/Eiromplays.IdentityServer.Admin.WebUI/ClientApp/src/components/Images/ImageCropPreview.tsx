import { PixelCrop } from 'react-image-crop';

export const ImageCropPreview = (
  image: HTMLImageElement,
  canvas: HTMLCanvasElement,
  crop: PixelCrop,
  fileName: string,
  onFileCreated?: (file: File) => void
) => {
  const ctx = canvas.getContext('2d');

  if (!ctx) {
    throw new Error('No 2d context');
  }

  const scaleX = image.naturalWidth / image.width;
  const scaleY = image.naturalHeight / image.height;
  const pixelRatio = window.devicePixelRatio || 1;

  canvas.width = Math.floor(crop.width * pixelRatio * scaleX);
  canvas.height = Math.floor(crop.height * pixelRatio * scaleY);

  ctx.scale(pixelRatio, pixelRatio);
  ctx.imageSmoothingQuality = 'high';

  const cropX = crop.x * scaleX;
  const cropY = crop.y * scaleY;
  const cropWidth = crop.width * scaleX;
  const cropHeight = crop.height * scaleY;

  ctx.drawImage(image, cropX, cropY, cropWidth, cropHeight, 0, 0, cropWidth, cropHeight);

  canvas.toBlob((blob) => {
    if (!blob) return;
    const file = blobToFile(blob, fileName);
    if (file && onFileCreated) onFileCreated(file);
  });

  const blobToFile = (theBlob: Blob, fileName: string): File => {
    return new File([theBlob], fileName, {
      lastModified: new Date().getTime(),
      type: theBlob.type,
    });
  };
};
