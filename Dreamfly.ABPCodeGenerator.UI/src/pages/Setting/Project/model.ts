import { Effect, Reducer } from 'umi';
import { message } from 'antd';
import { getProject, updateProject } from './service';

export interface AuthorType {
  name?: string;
  email?: string;
  remark?: string;
}
export interface ProjectType {
  name?: string;
  author?: AuthorType;
  version?: string;
}

export interface ModelType {
  namespace: string;
  state: {
    project?: ProjectType;
  };
  effects: {
    updateProject: Effect;
    getProject: Effect;
  };
  reducers: {
    save: Reducer<ProjectType>;
  };
}

const Model: ModelType = {
  namespace: 'project',
  state: {},
  effects: {
    *updateProject({ payload }, { call }) {
      yield call(updateProject, payload);
      message.success('保存成功');
    },
    *getProject({ payload }, { call, put }) {
      const response = yield call(getProject, payload);
      yield put({
        type: 'save',
        payload: response,
      });
    },
  },
  reducers: {
    save(state, action) {
      return {
        ...state,
        ...action.payload,
      };
    },
  },
};

export default Model;
