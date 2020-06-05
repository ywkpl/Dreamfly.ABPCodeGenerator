import { Effect, Reducer } from 'umi';
import { message } from 'antd';
import { getProject, updateProject } from './service';

const InitTemplate: ProjectTemplate = {
  file: 'Templates/',
  remark: '1',
  isExecute: true,
  outputFolder: '1',
  outputName: '1',
  projectFile: '1',
};

const Model: ModelType = {
  namespace: 'project',
  state: {
    project: {},
    editModelVisible: false,
    template: InitTemplate,
  },
  effects: {
    *updateProject({ payload }, { call }) {
      yield call(updateProject, payload);
      message.success('保存成功！');
    },
    *getProject({ payload }, { call, put }) {
      const response = yield call(getProject, payload);
      yield put({
        type: 'save',
        payload: response,
      });
    },
    *changeTemplateExecute({ payload }, { put, select }) {
      const { project } = yield select((state: ProjectStateType) => state.project);
      const template = project.templates?.find((t: ProjectTemplate) => t.file === payload.file);
      if (template) {
        template.isExecute = payload.isExecute;
        yield put({
          type: 'save',
          payload: project,
        });
      }
    },
    *insertTemplate({ payload }, { put, select }) {
      const { project } = yield select((state: ProjectStateType) => state.project);
      const test = {
        ...project,
      };

      test.templates.push(payload);
      console.log(test);
      yield put({
        type: 'save',
        payload: test,
      });
      yield put({
        type: 'saveEditModelVisible',
        payload: false,
      });
    },
  },
  reducers: {
    saveEditModelVisible(state, action) {
      return {
        ...state,
        editModelVisible: action.payload,
      };
    },
    save(state, action) {
      const test = {
        ...state,
        project: action.payload as ProjectType,
      };
      console.log(test);
      return test;
    },
  },
};

//#region Interfaces
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
  author?: AuthorType;
  version?: string;
  templates?: ProjectTemplate[];
}

export interface ProjectStateType {
  project?: ProjectType;
  editModelVisible?: boolean;
  template?: ProjectTemplate;
}

export interface ModelType {
  namespace: string;
  state: ProjectStateType;
  effects: {
    updateProject: Effect;
    getProject: Effect;
    changeTemplateExecute: Effect;
    insertTemplate: Effect;
  };
  reducers: {
    saveEditModelVisible: Reducer<ProjectStateType>;
    save: Reducer<ProjectStateType>;
  };
}
//#endregion

export default Model;
