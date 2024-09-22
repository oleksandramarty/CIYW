export interface IDataItem {
  id: string | undefined;
  name: string | undefined;
  description: string | undefined;
  isActive: boolean | undefined;
  isImportant: boolean | undefined;
}

export class DataItem implements IDataItem {
  id: string | undefined;
  name: string | undefined;
  description: string | undefined;
  isActive: boolean | undefined;
  isImportant: boolean | undefined;

  constructor(
    id: string | undefined,
    name: string | undefined,
    description: string | undefined,
    isActive: boolean | undefined,
    isImportant: boolean | undefined
  ) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.isActive = isActive;
    this.isImportant = isImportant;
  }
}
