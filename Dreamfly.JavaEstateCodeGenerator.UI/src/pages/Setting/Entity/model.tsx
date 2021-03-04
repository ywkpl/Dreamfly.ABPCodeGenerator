export interface EntityItemType {
  name: string;
  columnName?: string;
  type: string;
  length?: number;
  fraction?: number;
  hasTime: boolean;
  isRequired: boolean;
  description?: string;
  inQuery: boolean;
  inCreate: boolean;
  inResponse: boolean;
}

export interface EntityType {
  name: string;
  tableName?: string;
  description: string;
  entityItems: EntityItemType[];
}
