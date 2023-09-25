import { Injectable } from '@angular/core';
import Vibrant from 'node-vibrant'

@Injectable({
  providedIn: 'root',
})
export class ColorExtractorService {
  constructor() {}

  extractDominantColor(imgPath: string) {
    return new Promise((resolve, reject) => {
      Vibrant.from(imgPath).getPalette((err: any, palette: any) => {
        if (err) {
          reject(err);
        } else {
          // Extract the dominant color, you can also extract 'Vibrant', 'DarkVibrant', etc.
          const dominantColor = palette?.DominantSwatch?.hex;
          resolve(dominantColor);
        }
      });
    });
  }
}
