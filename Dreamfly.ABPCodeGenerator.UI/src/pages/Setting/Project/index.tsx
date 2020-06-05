import React, { useEffect, useCallback } from 'react';
import { Form, Card, Input, Button, Table, Divider, Space, Switch, Modal, message } from 'antd';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import { useDispatch, useSelector, Loading, ConnectRC, connect } from 'umi';
import { ProjectType, ProjectTemplate, ProjectStateType } from './model';
import { PlusOutlined, DeleteOutlined } from '@ant-design/icons';

import moduleName from '@umijs/hooks';
import styles from './index.less';
import { ColumnProps } from 'antd/es/table';

//https://zhuanlan.zhihu.com/p/103150605 直接干掉umi可不可以

const FormItem = Form.Item;

interface ProjectPageProps {
  submitting: boolean;
  project: ProjectStateType;
}

interface ProjectPageState {
  loading: Loading;
  project: { project: ProjectType; editModelVisible: boolean; template: ProjectTemplate };
}

const Project: ConnectRC<ProjectPageProps> = (props) => {
  const [mainForm] = Form.useForm();
  const [editModelForm] = Form.useForm();

  const { project, submitting, dispatch } = props;
  const { editModelVisible, template } = project;

  // const { project, editModelVisible, submitting, template } = useSelector(
  //   (state: ProjectPageState) => ({
  //     project: state.project.project,
  //     editModelVisible: state.project.editModelVisible,
  //     submitting: state.loading.effects['project/updateProject'],
  //     template: state.project.template,
  //   }),
  // );

  const formItemLayout = {
    labelCol: {
      xs: { span: 24 },
      sm: { span: 7 },
    },
    wrapperCol: {
      xs: { span: 24 },
      sm: { span: 12 },
      md: { span: 10 },
    },
  };

  const formAllItemLayout = {
    labelCol: {
      span: 5,
    },
    wrapperCol: {
      span: 17,
    },
  };

  const columns: ColumnProps<ProjectTemplate>[] = [
    {
      key: 'file',
      title: '模板路径',
      dataIndex: 'file',
    },
    {
      key: 'remark',
      title: '说明',
      dataIndex: 'remark',
    },
    {
      key: 'isExecute',
      title: '是否生成',
      dataIndex: 'isExecute',
      align: 'center',
      // render: (value, row) => {
      //   return (
      //     <Switch
      //       checked={value}
      //       onChange={(v) => {
      //         dispatch({
      //           type: 'project/changeTemplateExecute',
      //           payload: { file: row.file, isExecute: v },
      //         });
      //       }}
      //     />
      //   );
      // },
    },
    {
      key: 'outputFolder',
      title: '生成目录',
      dataIndex: 'outputFolder',
    },
    {
      key: 'outputName',
      title: '生成文件名',
      dataIndex: 'outputName',
    },
    {
      key: 'projectFile',
      title: '项目文件',
      dataIndex: 'projectFile',
    },
  ];

  const getProject = () => {
    dispatch({
      type: 'project/getProject',
    });
  };

  // const getProject = useCallback(() => {
  //   dispatch({
  //     type: 'project/getProject',
  //   });
  // }, [project]);

  useEffect(() => {
    getProject();
  }, []);

  useEffect(() => {
    console.log(project.project);
  }, [project.project]);

  const handleAdd = () => {
    dispatch({
      type: 'project/saveEditModelVisible',
      payload: true,
    });
  };

  const handleSave = () => {
    mainForm.validateFields().then((values) => {
      console.log({
        ...values,
        templates: project.templates,
      });
      // dispatch({
      //   type: 'project/updateProject',
      //   payload: {
      //     ...project,
      //     ...values,
      //   },
      // });
    });
  };

  const saveButton = (
    <Button type="primary" htmlType="submit">
      保存
    </Button>
  );

  const handleEditModelOk = () => {
    editModelForm.validateFields().then((values) => {
      console.log({
        ...values,
      });
      message.success('test');
      // dispatch({
      //   type: 'project/updateProject',
      //   payload: {
      //     ...project,
      //     ...values,
      //   },
      // });

      dispatch({
        type: 'project/insertTemplate',
        payload: values,
      });
    });
  };
  const handleEditModelCancel = () => {
    dispatch({
      type: 'project/saveEditModelVisible',
      payload: false,
    });
  };

  const modelEdit = (
    <Modal
      title="模板明细"
      destroyOnClose
      visible={editModelVisible}
      onOk={handleEditModelOk}
      okText="保存"
      onCancel={handleEditModelCancel}
    >
      <Form style={{ marginTop: 8 }} form={editModelForm} initialValues={template} name="model">
        <FormItem
          {...formAllItemLayout}
          label="模板路径"
          name="file"
          rules={[
            {
              required: true,
              message: '请输入模板路径',
            },
          ]}
        >
          <Input placeholder="模板路径" />
        </FormItem>
        <FormItem
          {...formAllItemLayout}
          label="说明"
          name="remark"
          rules={[
            {
              required: true,
              message: '请输入说明',
            },
          ]}
        >
          <Input placeholder="说明" />
        </FormItem>

        <FormItem {...formAllItemLayout} label="是否生成" name="isExecute" valuePropName="checked">
          <Switch />
        </FormItem>

        <FormItem
          {...formAllItemLayout}
          label="生成目录"
          name="outputFolder"
          rules={[
            {
              required: true,
              message: '请输入生成目录',
            },
          ]}
        >
          <Input placeholder="生成目录" />
        </FormItem>

        <FormItem
          {...formAllItemLayout}
          label="生成文件名"
          name="outputName"
          rules={[
            {
              required: true,
              message: '请输入生成文件名',
            },
          ]}
        >
          <Input placeholder="生成文件名" />
        </FormItem>
        <FormItem
          {...formAllItemLayout}
          label="项目文件"
          name="projectFile"
          rules={[
            {
              required: true,
              message: '请输入项目文件',
            },
          ]}
        >
          <Input placeholder="项目文件" />
        </FormItem>
      </Form>
    </Modal>
  );

  return (
    <PageHeaderWrapper extra={saveButton}>
      {Object.keys(project.project).length !== 0 && (
        <Form style={{ marginTop: 8 }} form={mainForm} initialValues={project.project} name="main">
          <Space direction="vertical" style={{ width: '100%' }}>
            <Card bordered={false} title="项目">
              <FormItem
                {...formItemLayout}
                label="名称"
                name="name"
                rules={[{ required: true, message: '请输入名称' }]}
              >
                <Input placeholder="名称" />
              </FormItem>
              <FormItem
                {...formItemLayout}
                label="版本"
                name="version"
                rules={[{ required: true, message: '请输入版本号' }]}
              >
                <Input placeholder="版本" />
              </FormItem>
            </Card>
            <Card bordered={false} title="作者">
              <FormItem
                {...formItemLayout}
                label="姓名"
                name={['author', 'name']}
                rules={[{ required: true, message: '请输入作者姓名' }]}
              >
                <Input placeholder="姓名" />
              </FormItem>
              <FormItem
                {...formItemLayout}
                label="Email"
                name={['author', 'email']}
                rules={[{ required: true, message: '请输入作者Email' }]}
              >
                <Input placeholder="Email" />
              </FormItem>
              <FormItem {...formItemLayout} label="说明" name={['author', 'remark']}>
                <Input placeholder="说明" />
              </FormItem>
            </Card>
            <Card bordered={false} title="模板">
              <div className={styles.tableList}>
                <div className={styles.tableListOperator}>
                  <Button icon={<PlusOutlined />} type="primary" onClick={handleAdd}>
                    新建
                  </Button>
                  <Button icon={<DeleteOutlined />} type="primary" danger onClick={handleSave}>
                    刪除
                  </Button>
                </div>
                <Divider />
                <Table
                  pagination={false}
                  columns={columns}
                  rowKey="file"
                  bordered
                  dataSource={project.project.templates}
                  //rowSelection={rowSelection}
                />
              </div>
            </Card>
          </Space>
        </Form>
      )}
      {editModelVisible && modelEdit}
    </PageHeaderWrapper>
  );
};

export default connect(({ project, loading }: { project: ProjectStateType; loading: Loading }) => ({
  project,
  submitting: loading.effects['project/updateProject'],
}))(Project);
