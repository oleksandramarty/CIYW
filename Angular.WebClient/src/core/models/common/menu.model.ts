export interface IMenuModel {
    activeTab?: number | undefined;
    menuItems: IMenuModelItem[] | undefined;
}

export interface IMenuModelItem {
    index?: number;
    title?: string;
    isOpen?: boolean;
    icon?: string;
    url?: string;
    menuItems?: IMenuModelItem[] | undefined;
}

export class MenuModelItem implements IMenuModelItem {
    index?: number = 0;
    title?: string = '';
    isOpen?: boolean;
    icon?: string;
    url?: string;
    menuItems?: IMenuModelItem[] | undefined;

    constructor(data?: IMenuModelItem) {
        if (data) {
            this.index = data.index;
            this.title = data.title;
            this.menuItems = data.menuItems ? data.menuItems.map(item => new MenuModelItem(item)) : undefined;
            this.isOpen = data.isOpen;
            this.icon = data.icon;
            this.url = data.url;
        }
    }
}

export class MenuModel implements IMenuModel {
    activeTab?: number | undefined;
    menuItems: MenuModelItem[] | undefined;

    executableAction: () => void = () => {};

    constructor(data?: IMenuModel) {
        if (data) {
            this.activeTab = data.activeTab;
            this.menuItems = data.menuItems ? data.menuItems.map(item => new MenuModelItem(item)) : undefined;
        }
    }

    createUserProjectMenu(activeTab: number | undefined): void {
        this.activeTab = activeTab ?? 0;
        this.menuItems = [
            new MenuModelItem({index: 0, title: 'MENU.FAVORITES'}),
            new MenuModelItem({index: 1, title: 'MENU.EXPENSES'}),
            new MenuModelItem({index: 2, title: 'MENU.PLANNED_EXPENSES'}),
        ];
    }

    createSideMenu(): void {
        this.menuItems = [
            {
                isOpen: true,
                title: 'MENU.HOME',
                icon: 'fa-solid fa-house',
                menuItems: [
                    {
                        title: 'MENU.DASHBOARD',
                        url: '/dashboard',
                        icon: 'fa-solid fa-table-columns',
                    },
                    {
                        title: 'USER_PROJECTS',
                        url: '/projects',
                        icon: 'fa-solid fa-diagram-project',
                    },
                    {
                        title: 'USER.MENU.ANALYTICS',
                        url: '/analytics',
                        icon: 'fa-solid fa-chart-pie',
                    }
                ]
            },
            {
                isOpen: false,
                title: 'MENU.PROFILE',
                icon: 'fa-solid fa-user',
                menuItems: [
                    {
                        title: 'USER.MENU.NOTIFICATIONS',
                        url: '/users/notifications',
                        icon: 'fa-solid fa-bell',
                    },
                    {
                        title: 'USER.MENU.SETTINGS',
                        url: '/users/settings',
                        icon: 'fa-solid fa-gears',
                    }
                ]
            },
            {
                title: 'MENU.ABOUT',
                icon: 'fa-solid fa-circle-info',
                url: '/about'
            },
            {
                title: 'MENU.CONTACT_US',
                icon: 'fa-solid fa-house',
                url: '/contact-us'
            },
        ];
    }

    createAdminSideMenu(): void {
        if (!this.menuItems) {
            this.createSideMenu();
        }

        this.menuItems?.forEach(menuItem => { menuItem.isOpen = false; });
        this.menuItems?.unshift({
            isOpen: true,
            title: 'ADMIN.ADMIN_AREA',
            icon: 'fa-solid fa-screwdriver-wrench',
            menuItems: [
                {
                    title: 'MENU.DASHBOARD',
                    url: '/admin/home',
                    icon: 'fa-solid fa-table-columns',
                },
                {
                    title: 'ADMIN.MENU.USERS',
                    url: '/admin/users',
                    icon: 'fa-solid fa-users',
                },
                {
                    title: 'ADMIN.MENU.AUDIT_TRAIL',
                    url: '/admin/audit-trail',
                    icon: 'fa-solid fa-chart-pie',
                }
            ]
        });
    }
}
