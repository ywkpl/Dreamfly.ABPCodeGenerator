export interface EntityItemType {
  index: number;
  id: number;
  name: string;
  columnName?: string;
  type: string;
  length?: number;
  fraction?: number;
  isRequired: boolean;
  description?: string;
  inQuery: boolean;
  inCreate: boolean;
  inResponse: boolean;
  inAllResponse: boolean;
  relateType?: string;
  cascadeType?: string;
  fetchType?: string;
  relateEntity?: string;
  relateDirection?: string;
  joinName?: string;
  foreignKeyName?: string;
  relateEntityInModule: boolean;
  order?: number;
}

export interface EntityType {
  id: number;
  name: string;
  tableName?: string;
  description: string;
  entityItems: EntityItemType[];
}

export interface SavedEntity {
  entityDto: EntityType;
  sql: string;
}
