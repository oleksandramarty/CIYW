import {DataItem, IDataItem} from "./data-item.model";
import {extractClassName} from "../../helpers/dom.helper";

export interface IIconDataItemPicker {
    items: IIconDataItem[] | undefined;
}

export interface IIconDataItem {
    title: string | undefined;
    items: IDataItem[] | undefined;
}

export class IconDataItem implements IIconDataItem {
    title: string | undefined;
    items: DataItem[] | undefined;

    constructor(title?: string, items?: DataItem[]) {
        this.title = title;
        this.items = items;
    }
}

export class IconDataItemPicker implements IIconDataItemPicker {
    items: IconDataItem[] | undefined;

    constructor() {
        this.createIconsDataItems();
    }

    private createIconsDataItems(): void {
        this.addCategory('ICONS.BUILDINGS',
            [
                'fa-solid fa-house',
                'fa-solid fa-warehouse',
                'fa-solid fa-hospital',
                'fa-solid fa-shop',
                'fa-solid fa-landmark',
                'fa-solid fa-school',
                'fa-solid fa-mountain-city',
                'fa-solid fa-building-shield',
                'fa-solid fa-tree-city',
                'fa-solid fa-school-flag',
                'fa-solid fa-landmark-flag',
                'fa-solid fa-igloo',
                'fa-solid fa-oil-well',
                'fa-solid fa-dungeon',
                'fa-solid fa-dumpster',
            ]);

        this.addCategory('ICONS.PEOPLE',
            [
                'fa-solid fa-baby',
                'fa-solid fa-user-secret',
                'fa-solid fa-user-tie',
                'fa-solid fa-user-shield',
                'fa-solid fa-user-ninja',
                'fa-solid fa-user-injured',
                'fa-solid fa-user-graduate',
                'fa-solid fa-user-doctor',
                'fa-solid fa-user-astronaut',
                'fa-solid fa-person-walking-with-cane',
                'fa-solid fa-person-walking-luggage',
                'fa-solid fa-person-walking',
                'fa-solid fa-person-swimming',
                'fa-solid fa-person-snowboarding',
                'fa-solid fa-person-skiing-nordic',
                'fa-solid fa-person-skiing',
                'fa-solid fa-person-skating',
                'fa-solid fa-person-shelter',
                'fa-solid fa-person-rifle',
                'fa-solid fa-person-pregnant',
                'fa-solid fa-person-military-to-person',
                'fa-solid fa-person-military-rifle',
                'fa-solid fa-person-military-pointing',
                'fa-solid fa-person-hiking',
                'fa-solid fa-person-half-dress',
                'fa-solid fa-person-drowning',
                'fa-solid fa-person-breastfeeding',
                'fa-solid fa-person-biking',
                'fa-solid fa-people-roof',
                'fa-solid fa-people-carry-box',
                'fa-solid fa-children',
                'fa-solid fa-chalkboard-user',
                'fa-solid fa-users-viewfinder',
                'fa-solid fa-hands-holding-child',
                'fa-solid fa-clipboard-user',
                'fa-solid fa-baby-carriage',
                'fa-solid fa-handshake',
                'fa-solid fa-hand-holding-heart',
            ]);

        this.addCategory('ICONS.TRANSPORT',
            [
                'fa-solid fa-car',
                'fa-solid fa-truck',
                'fa-solid fa-train',
                'fa-solid fa-ship',
                'fa-solid fa-motorcycle',
                'fa-solid fa-truck-monster',
                'fa-solid fa-plane',
                'fa-solid fa-bicycle',
                'fa-solid fa-van-shuttle',
                'fa-solid fa-truck-pickup',
                'fa-solid fa-truck-moving',
                'fa-solid fa-truck-medical',
                'fa-solid fa-truck-field',
                'fa-solid fa-train-tram',
                'fa-solid fa-train-subway',
                'fa-solid fa-trailer',
                'fa-solid fa-tractor',
                'fa-solid fa-taxi',
                'fa-solid fa-sailboat',
                'fa-solid fa-jet-fighter-up',
                'fa-solid fa-ferry',
                'fa-solid fa-caravan',
                'fa-solid fa-car-rear',
                'fa-solid fa-car-burst',
                'fa-solid fa-bus-simple',
                'fa-solid fa-bus-simple',
                'fa-solid fa-bus',
                'fa-solid fa-snowplow',
                'fa-solid fa-gas-pump',
                'fa-solid fa-charging-station',
                'fa-solid fa-oil-can',
                'fa-solid fa-car-battery',
                'fa-solid fa-dolly',
            ]);

        this.addCategory('ICONS.ANIMALS',
            [
                'fa-solid fa-paw',
                'fa-solid fa-hippo',
                'fa-solid fa-shield-dog',
                'fa-solid fa-shield-cat',
                'fa-solid fa-shrimp',
                'fa-solid fa-horse',
                'fa-solid fa-dove',
                'fa-solid fa-dog',
                'fa-solid fa-cat',
                'fa-solid fa-cow',
                'fa-solid fa-bone',
            ]);

        this.addCategory('ICONS.ACTIVITIES',
            [
                'fa-solid fa-bomb',
                'fa-solid fa-mug-hot',
                'fa-solid fa-gift',
                'fa-solid fa-joint',
                'fa-solid fa-wine-bottle',
                'fa-solid fa-dice',
                'fa-solid fa-wine-glass',
                'fa-solid fa-umbrella-beach',
                'fa-solid fa-dumbbell',
                'fa-solid fa-water-ladder',
                'fa-solid fa-volleyball',
                'fa-solid fa-utensils',
                'fa-regular fa-futbol',
                'fa-solid fa-champagne-glasses',
                'fa-solid fa-cake-candles',
                'fa-solid fa-candy-cane',
                'fa-solid fa-cake-candles',
                'fa-solid fa-burger',
                'fa-solid fa-baseball-bat-ball',
                'fa-solid fa-baseball',
                'fa-solid fa-guitar',
                'fa-solid fa-masks-theater',
                'fa-solid fa-gifts',
                'fa-solid fa-table-tennis-paddle-ball',
                'fa-solid fa-martini-glass-citrus',
                'fa-solid fa-football',
                'fa-solid fa-glass-water',
                'fa-solid fa-basketball',
                'fa-solid fa-pizza-slice',
                'fa-solid fa-ticket',
                'fa-solid fa-image',
                'fa-solid fa-cart-shopping',
                'fa-solid fa-basket-shopping',
                'fa-solid fa-bag-shopping',
                'fa-solid fa-shirt',
                'fa-solid fa-film',
                'fa-solid fa-snowman',
            ]);

        this.addCategory('ICONS.HI_TECH',
            [
                'fa-solid fa-microchip',
                'fa-solid fa-robot',
                'fa-solid fa-fingerprint',
                'fa-solid fa-phone',
                'fa-solid fa-music',
                'fa-solid fa-camera-retro',
                'fa-solid fa-wifi',
                'fa-solid fa-gamepad',
                'fa-solid fa-phone-volume',
                'fa-solid fa-database',
                'fa-solid fa-mobile',
                'fa-solid fa-laptop',
                'fa-solid fa-desktop',
                'fa-solid fa-radio',
                'fa-solid fa-pager',
                'fa-solid fa-mobile-retro',
                'fa-solid fa-ethernet',
                'fa-solid fa-display',
                'fa-solid fa-coins',
                'fa-solid fa-microphone-lines',
                'fa-solid fa-sim-card',
                'fa-solid fa-satellite-dish',
                'fa-solid fa-podcast',
                'fa-solid fa-floppy-disk',
                'fa-solid fa-blender',
                'fa-solid fa-rectangle-ad',
            ]);

        this.addCategory('ICONS.HEALTHCARE',
            [
                'fa-solid fa-shield-halved',
                'fa-solid fa-stethoscope',
                'fa-solid fa-soap',
                'fa-solid fa-glasses',
                'fa-solid fa-tooth',
                'fa-solid fa-teeth-open',
                'fa-solid fa-syringe',
                'fa-solid fa-suitcase-medical',
                'fa-solid fa-staff-snake',
                'fa-solid fa-spray-can-sparkles',
                'fa-solid fa-smoking',
                'fa-solid fa-skull',
                'fa-solid fa-pump-soap',
                'fa-solid fa-pills',
                'fa-solid fa-mortar-pestle',
                'fa-solid fa-microscope',
                'fa-solid fa-dna',
                'fa-solid fa-bandage',
                'fa-solid fa-capsules',
                'fa-solid fa-lungs',
                'fa-solid fa-hospital-user',
                'fa-solid fa-heart-pulse',
                'fa-solid fa-head-side-mask',
                'fa-solid fa-mask-face',
                'fa-solid fa-cannabis',
                'fa-solid fa-bong',
                'fa-solid fa-ban-smoking',
                'fa-solid fa-bacterium',
                'fa-solid fa-bread-slice',
                'fa-solid fa-hand-holding-medical',
                'fa-solid fa-hand-holding-droplet',
                'fa-solid fa-bed-pulse',
            ]
        );

        this.addCategory('ICONS.FINANCES',
            [
                'fa-solid fa-clover',
                'fa-solid fa-wallet',
                'fa-solid fa-piggy-bank',
                'fa-solid fa-money-bill',
                'fa-solid fa-credit-card',
                'fa-regular fa-credit-card',
                'fa-solid fa-cash-register',
                'fa-solid fa-vault',
                'fa-solid fa-money-check-dollar',
                'fa-solid fa-money-check',
                'fa-solid fa-money-bills',
                'fa-solid fa-money-bill-trend-up',
                'fa-solid fa-money-bill-transfer',
                'fa-solid fa-money-bill-1',
                'fa-solid fa-circle-dollar-to-slot',
                'fa-solid fa-comments-dollar',
                'fa-solid fa-hand-holding-dollar',
                'fa-solid fa-bitcoin-sign',
                'fa-solid fa-gem',
                'fa-solid fa-chart-pie',
                'fa-solid fa-receipt',
            ]
        );

        this.addCategory('ICONS.TRAVEL',
            [
                'fa-solid fa-language',
                'fa-solid fa-cart-flatbed-suitcase',
                'fa-solid fa-campground',
                'fa-solid fa-map-location-dot',
                'fa-solid fa-monument',
                'fa-solid fa-mountain',
                'fa-solid fa-panorama',
                'fa-solid fa-signs-post',
                'fa-solid fa-torii-gate',
                'fa-solid fa-road',
                'fa-solid fa-route',
                'fa-solid fa-volcano',
                'fa-solid fa-tent',
                'fa-solid fa-passport',
                'fa-solid fa-suitcase-rolling',
                'fa-solid fa-globe',
                'fa-solid fa-mountain-sun',
                'fa-solid fa-bell-concierge',
                'fa-solid fa-bed',
                'fa-solid fa-book-atlas',
                'fa-solid fa-compass',
            ]
        );

        this.addCategory('ICONS.CONSTRUCTION',
            [
                'fa-solid fa-droplet',
                'fa-solid fa-fire',
                'fa-solid fa-paint-roller',
                'fa-solid fa-brush',
                'fa-solid fa-flask',
                'fa-solid fa-briefcase',
                'fa-solid fa-ruler',
                'fa-solid fa-lightbulb',
                'fa-solid fa-trowel-bricks',
                'fa-solid fa-trowel',
                'fa-solid fa-toolbox',
                'fa-solid fa-screwdriver-wrench',
                'fa-solid fa-screwdriver',
                'fa-solid fa-pen-ruler',
                'fa-solid fa-gear',
                'fa-solid fa-plug',
                'fa-solid fa-fire-flame-curved',
                'fa-solid fa-faucet-drip',
                'fa-solid fa-fire-burner',
            ]
        );

        this.addCategory('ICONS.OTHER',
            [
                    'fa-solid fa-crown',
                    'fa-solid fa-envelope',
                    'fa-solid fa-star',
                    'fa-solid fa-location-dot',
                    'fa-solid fa-poo',
                    'fa-solid fa-bolt',
                    'fa-solid fa-ghost',
                    'fa-solid fa-umbrella',
                    'fa-solid fa-book',
                    'fa-solid fa-print',
                    'fa-solid fa-eye',
                    'fa-solid fa-tree',
                    'fa-solid fa-bath',
                    'fa-solid fa-fish',
                    'fa-solid fa-rocket',
                    'fa-solid fa-icons',
                    'fa-solid fa-chair',
                    'fa-solid fa-recycle',
                    'fa-solid fa-keyboard',
                    'fa-solid fa-microphone',
                    'fa-solid fa-trash-can',
                    'fa-solid fa-tower-observation',
                    'fa-solid fa-tower-cell',
                    'fa-solid fa-tower-broadcast',
                    'fa-solid fa-toilet-paper',
                    'fa-solid fa-toilet',
                    'fa-solid fa-stapler',
                    'fa-solid fa-sink',
                    'fa-solid fa-scissors',
                    'fa-solid fa-scale-balanced',
                    'fa-solid fa-radiation',
                    'fa-solid fa-pen-clip',
                    'fa-solid fa-peace',
                    'fa-solid fa-paintbrush',
                    'fa-solid fa-meteor',
                    'fa-solid fa-ice-cream',
                    'fa-solid fa-handcuffs',
                    'fa-solid fa-gun',
                    'fa-solid fa-graduation-cap',
                    'fa-solid fa-gavel',
                    'fa-solid fa-fire-extinguisher',
                    'fa-solid fa-explosion',
                    'fa-solid fa-envelopes-bulk',
                    'fa-solid fa-envelope-open-text',
                    'fa-solid fa-drumstick-bite',
                    'fa-solid fa-dice-one',
                    'fa-solid fa-dice-two',
                    'fa-solid fa-dice-three',
                    'fa-solid fa-dice-four',
                    'fa-solid fa-dice-five',
                    'fa-solid fa-dice-six',
                    'fa-solid fa-cubes',
                    'fa-solid fa-bullhorn',
                    'fa-solid fa-bucket',
                    'fa-solid fa-broom',
                    'fa-solid fa-box-tissue',
                    'fa-solid fa-biohazard',
                    'fa-solid fa-bacon'
            ]
        );
    }

    private addCategory(title: string, items: string[]): void {
        if (!this.items) {
            this.items = [];
        }
        this.items.push({
            title: title,
            items: items.map(i => this.createIconsDataItem(i))
        });
    }

    private createIconsDataItem(el: string): DataItem {
        const icon = el; //extractClassName(el);
        return new DataItem(undefined, icon, icon, icon);
    }
}