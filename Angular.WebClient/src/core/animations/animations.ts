import { trigger, state, style, animate, transition } from '@angular/animations';

export const fadeInOut = trigger('fadeInOut', [
    state('void', style({ opacity: 0 })),
    state('*', style({ opacity: 1 })),
    transition(':enter', [
        style({ opacity: 0 }),
        animate('150ms ease-in')
    ]),
    transition(':leave', [
        animate('150ms ease-out', style({ opacity: 0 }))
    ])
]);