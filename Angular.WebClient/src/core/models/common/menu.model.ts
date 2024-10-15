export interface IMenuModel {
    activeTab: number | undefined;
    menuItems: IMenuModelItem[] | undefined;
}

export interface IMenuModelItem {
    index: number;
    title: string;
}

export class MenuModelItem implements IMenuModelItem {
    index: number = 0;
    title: string = '';

    constructor(data?: IMenuModelItem) {
        if (data) {
            this.index = data.index;
            this.title = data.title;
        }
    }
}

export class MenuModel implements IMenuModel {
    activeTab: number | undefined;
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
}
