import React, { useEffect } from 'react';
import { Form, Card, Input, Button, Table, Divider, Space, Switch, Checkbox } from 'antd';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import { useDispatch, useSelector, Loading } from 'umi';
import { ProjectType, ProjectTemplate } from './model';
import styles from './index.less';
import { ColumnProps } from 'antd/es/table';

const FormItem = Form.Item;

interface ProjectPageState {
  loading: Loading;
  project: ProjectType;
}

const Project = () => {
  const [form] = Form.useForm();
  const dispatch = useDispatch();
  const { project, submitting } = useSelector((state: ProjectPageState) => ({
    project: state.project,
    submitting: state.loading.effects['project/updateProject'],
  }));

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
      render: (value, row) => {
        return (
          <>
            <Checkbox
              checked={value}
              value={row.file}
              onChange={(v) => {
                const { templates } = project;
                const template = templates?.find((t: ProjectTemplate) => t.file === v.target.value);
                if (template) {
                  template.isExecute = v.target.checked;
                }
                console.log(v);
                console.log(template);
              }}
            />
            <Switch
              checked={value}
              onChange={(v) => {
                dispatch({
                  type: 'project/test',
                  payload: {
                    ...project,
                    name: 'test',
                  },
                });
                const { templates } = project;
                const template = templates?.find((t) => t.file === row.file);
                if (template) {
                  console.log('test');
                  template.isExecute = v;
                }
                console.log(v);
                console.log(row);
              }}
            />
          </>
        );
      },
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

  useEffect(() => {
    getProject();
  }, []);

  const handleAdd = () => {
    dispatch({
      type: 'project/addTemplate',
    });
  };

  const saveButton = (
    <Button
      type="primary"
      htmlType="submit"
      onClick={() => {
        form.validateFields().then((values) => {
          // console.log(submitting)
          // setTimeout(() => {}, 5000);
          console.log(values);
          //dispatch({ type: 'project/updateProject', payload: values });
        });
      }}
      loading={submitting}
    >
      保存
    </Button>
  );

  return (
    <PageHeaderWrapper extra={saveButton}>
      {Object.keys(project).length !== 0 && (
        <Form style={{ marginTop: 8 }} form={form} initialValues={project}>
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
              {/* <FormItem {...submitFormLayout} style={{ marginTop: 32 }}>
                <Button type="primary" htmlType="submit" loading={submitting}>
                  保存
                </Button>
              </FormItem> */}
            </Card>
            <Card bordered={false} title="模板">
              <div className={styles.tableList}>
                <div className={styles.tableListOperator}>
                  <Button icon="plus" type="primary" onClick={handleAdd}>
                    新建
                  </Button>

                  <Button icon="danger" type="primary" onClick={handleAdd}>
                    刪除
                  </Button>
                </div>
                <Divider />
                <Table
                  pagination={false}
                  columns={columns}
                  rowKey="file"
                  bordered
                  dataSource={project.templates}
                  //rowSelection={rowSelection}
                />
              </div>
            </Card>
          </Space>
        </Form>
      )}
    </PageHeaderWrapper>
  );
};

export default Project;
