import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {
  constructor() { }

  setItem(key: string, value: any): void {
    localStorage.setItem(key, JSON.stringify(value));
  }

  getItem<T>(key: string): T | undefined {
    const item = localStorage.getItem(key);
    return !!item ? JSON.parse(item) : undefined;
  }

  removeItem(key: string): void {
    localStorage.removeItem(key);
  }

  clear(): void {
    localStorage.clear();
  }

  clearLocalStorageAndRefresh(): void {
    localStorage.clear();
    location.reload();
  }
}
