import {Component, OnInit, Renderer2} from '@angular/core';
import {AuthService} from "../../../../core/services/auth.service";
import {BaseInitializationService} from "../../../../core/services/base-initialization.service";

@Component({
  selector: 'app-night-sky',
  standalone: true,
  imports: [],
  templateUrl: './night-sky.component.html',
  styleUrl: './night-sky.component.scss'
})
export class NightSkyComponent implements OnInit{
  constructor(
      private renderer: Renderer2
  ) {
  }

  ngOnInit(): void {
    this.generateStars();
  }

  private generateStars(): void {
    const starsContainer = document.querySelector('.stars');
    if (!starsContainer) {
      return
    }
    const nightsky = ["#280F36", "#632B6C", "#BE6590", "#FFC1A0", "#FE9C7F"];

    for (let i = 0; i < 500; i++) {
      this.createStar(starsContainer, 'star-1', this.getRandomInt(0, 40), this.getRandomInt(0, 100), this.getRandomInt(2, 5));
      this.createStar(starsContainer, 'star-2', this.getRandomInt(20, 70), this.getRandomInt(0, 100), this.getRandomInt(4, 8));
    }

    for (let i = 0; i < 150; i++) {
      this.createStar(starsContainer, 'star-0', this.getRandomInt(0, 50), this.getRandomInt(0, 100), this.getRandomInt(1, 2.5));
      this.createStar(starsContainer, 'star-1', this.getRandomInt(0, 50), this.getRandomInt(0, 100), this.getRandomInt(2.5, 4));
      this.createStar(starsContainer, 'star-2', this.getRandomInt(0, 50), this.getRandomInt(0, 100), this.getRandomInt(4, 5));
    }

    for (let i = 0; i < 100; i++) {
      this.createStar(starsContainer, 'star-0', this.getRandomInt(40, 75), this.getRandomInt(0, 100), this.getRandomInt(1, 3));
      this.createStar(starsContainer, 'star-1', this.getRandomInt(40, 75), this.getRandomInt(0, 100), this.getRandomInt(2, 4));
    }

    for (let i = 0; i < 250; i++) {
      this.createStar(starsContainer, 'star-0', this.getRandomInt(0, 100), this.getRandomInt(0, 100), this.getRandomInt(1, 2));
      this.createStar(starsContainer, 'star-1', this.getRandomInt(0, 100), this.getRandomInt(0, 100), this.getRandomInt(2, 5));
      this.createStar(starsContainer, 'star-2', this.getRandomInt(0, 100), this.getRandomInt(0, 100), this.getRandomInt(1, 4));
      this.createStar(starsContainer, 'star-4', this.getRandomInt(0, 70), this.getRandomInt(0, 100), this.getRandomInt(5, 7));
    }

    for (let i = 0; i < 150; i++) {
      this.createStar(starsContainer, 'star-4', this.getRandomInt(0, 100), this.getRandomInt(0, 100), this.getRandomInt(5, 7));
    }
  }

  private createStar(container: Element, starClass: string, top: number, left: number, duration: number): void {
    const star = this.renderer.createElement('div');
    this.renderer.addClass(star, 'star');
    this.renderer.addClass(star, starClass);
    this.renderer.setStyle(star, 'top', `${top}vh`);
    this.renderer.setStyle(star, 'left', `${left}vw`);
    this.renderer.setStyle(star, 'animation-duration', `${duration}s`);
    this.renderer.appendChild(container, star);
  }

  private getRandomInt(min: number, max: number): number {
    return Math.random() * (max - min) + min;
  }
}
