export interface AuthorType {
  name?: string;
  email?: string;
  remark?: string;
}

export interface ProjectTemplate {
  file: string;
  remark: string;
  isExecute: boolean;
  outputFolder: string;
  outputName: string;
  projectFile: string;
}

export interface ProjectType {
  name?: string;
  outputPath?: string;
  author?: AuthorType;
  version?: string;
  templates: ProjectTemplate[];
}
