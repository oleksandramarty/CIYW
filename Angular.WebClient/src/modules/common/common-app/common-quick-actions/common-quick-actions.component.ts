import { Component, HostListener, OnInit, ViewChild, ElementRef, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-common-quick-actions',
  templateUrl: './common-quick-actions.component.html',
  styleUrls: ['./common-quick-actions.component.scss']
})
export class CommonQuickActionsComponent {
  public quickActionsOpen: boolean = false;

  @ViewChild('quickActionsContainer') quickActionsContainer!: ElementRef;
  @ViewChild('icon') icon!: ElementRef;

  public quickActionsList = [
    {
      icon: 'fas fa-plus',
      title: 'Add New',
      action: 'add'
    },
    {
      icon: 'fas fa-search',
      title: 'Search',
      action: 'search'
    },
    {
      icon: 'fas fa-filter',
      title: 'Filter',
      action: 'filter'
    },
    {
      icon: 'fas fa-sort',
      title: 'Sort',
      action: 'sort'
    }
  ];

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    if (this.quickActionsContainer && !this.quickActionsContainer.nativeElement.contains(target) && this.quickActionsOpen) {
     this.toggleQuickActions();
    }
  }

  public toggleQuickActions() {
    this.quickActionsOpen = !this.quickActionsOpen;
    const icon = this.icon.nativeElement;
    icon.classList.add('rotate');
    setTimeout(() => {
      icon.classList.remove('rotate');
    }, 500);
  }
}