export enum EntityItemMapType {
  CreateInput,
  Output,
  QueryInput,
}

export interface EntityItemType {
  name: string;
  type: string;
  length?: number;
  isRequired: boolean;
  description?: string;
  mapTypes: EntityItemMapType[];
}

export interface EntityType {
  name: string;
  module: string;
  entityItems: EntityItemType[];
}
