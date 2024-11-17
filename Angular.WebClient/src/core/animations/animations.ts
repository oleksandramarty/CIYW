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

export const slideInFromLeft = trigger('slideInFromLeft', [
    state('open', style({
        left: '0',
        zIndex: 1000
    })),
    state('closed', style({
        left: '-100%',
        zIndex: -1
    })),
    transition('closed => open', [
        animate('300ms ease-in')
    ]),
    transition('open => closed', [
        animate('300ms ease-out')
    ])
]);

export const slideInOut = trigger('slideInOut', [
    state('in', style({
        height: '*',
        overflow: 'hidden'
    })),
    state('out', style({
        height: '0',
        overflow: 'hidden'
    })),
    transition('in <=> out', animate('300ms ease-in-out'))
]);
