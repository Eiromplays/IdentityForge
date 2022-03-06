import React, { useState, useRef, useEffect, useCallback } from 'react';
import ReactCrop, { centerCrop, makeAspectCrop, Crop, PixelCrop } from 'react-image-crop';

import { ImageCropPreview } from './ImageCropPreview';

import 'react-image-crop/src/ReactCrop.scss';

export type ImageCropperProps = {
  imgSrc: string;
  fileName: string;
  onFileCreated?: (file: File) => void;
};

export const ImageCropper = ({ imgSrc, fileName, onFileCreated }: ImageCropperProps) => {
  const imgRef = useRef<any>(null);
  const previewCanvasRef = useRef<any>(null);
  const [crop, setCrop] = useState<Crop>();
  const [completedCrop, setCompletedCrop] = useState<PixelCrop>();

  function onImageLoad(e: React.SyntheticEvent<HTMLImageElement>) {
    imgRef.current = e.currentTarget;

    const { width, height } = e.currentTarget;

    const crop = centerCrop(
      makeAspectCrop(
        {
          unit: '%',
          width: 40,
        },
        1 / 1,
        width,
        height
      ),
      width,
      height
    );

    setCrop(crop);
  }

  const updateCropPreview = useCallback(() => {
    if (completedCrop && previewCanvasRef.current && imgRef.current) {
      ImageCropPreview(
        imgRef.current,
        previewCanvasRef.current,
        completedCrop,
        fileName,
        onFileCreated
      );
    }
  }, [completedCrop, fileName, onFileCreated]);

  useEffect(() => {
    updateCropPreview();
  }, [updateCropPreview]);

  return (
    <>
      {imgSrc && (
        <ReactCrop
          crop={crop}
          onChange={(_, percentCrop) => setCrop(percentCrop)}
          onComplete={(c) => setCompletedCrop(c)}
          aspect={1 / 1}
        >
          <img
            alt={`Cropped ${fileName}`}
            src={imgSrc}
            style={{ transform: `scale(1) rotate(0deg)` }}
            onLoad={onImageLoad}
          />
        </ReactCrop>
      )}
      <div>
        {previewCanvasRef && (
          <canvas
            ref={previewCanvasRef}
            style={{
              // Rounding is important for sharpness.
              width: Math.floor(completedCrop?.width ?? 0),
              height: Math.floor(completedCrop?.height ?? 0),
            }}
            className="w-52 h-52 rounded-full"
          />
        )}
      </div>
    </>
  );
};
