export enum EntityItemMapType {
  CreateInput,
  Output,
  QueryInput,
}

export interface EntityItemType {
  name: string;
  columnName?: string;
  type: string;
  length?: number;
  isRequired: boolean;
  description?: string;
  mapTypes: EntityItemMapType[];
}

export interface EntityType {
  name: string;
  tableName?: string;
  module: string;
  description: string;
  entityItems: EntityItemType[];
}
