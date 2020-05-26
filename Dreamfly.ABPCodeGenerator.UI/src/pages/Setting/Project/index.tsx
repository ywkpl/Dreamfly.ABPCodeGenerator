import React, { FC, useEffect } from 'react';
import { Form, Card, Input, Button, Table, Divider } from 'antd';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import { connect, Dispatch } from 'umi';
import { ProjectType } from './model';
import { FormInstance } from 'antd/lib/form';
import styles from './index.less';
import { ColumnProps } from 'antd/es/table';

const FormItem = Form.Item;

interface ProjectProps {
  project: ProjectType;
  submitting: boolean;
  dispatch: Dispatch<any>;
}

interface ProjectTemplate {
  name: string;
}

const Project: FC<ProjectProps> = (props) => {
  const { submitting, project } = props;
  const [form] = Form.useForm();

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
    },
    {
      key: 'folder',
      title: '生成目录',
      dataIndex: 'folder',
    },
    {
      key: 'name',
      title: '生成文件名',
      dataIndex: 'name',
    },
  ];

  const getProject = () => {
    props.dispatch({
      type: 'project/getProject',
    });
  };

  useEffect(() => {
    getProject();
  });

  const handleAdd = () => {
    props.dispatch({
      type: 'project/addTemplate',
    });
  };

  const saveButton = (
    <Button
      type="primary"
      htmlType="submit"
      onClick={() => {
        form.validateFields().then((values) => {
          console.log(values);
        });
      }}
      loading={submitting}
    >
      保存
    </Button>
  );

  return (
    <PageHeaderWrapper extra={saveButton} extraContent={<div>test</div>} content={<div>test</div>}>
      {Object.keys(project).length !== 0 && (
        <Form style={{ marginTop: 8 }} form={form} initialValues={project}>
          <Card bordered={false}>
            <FormItem
              {...formItemLayout}
              label="名称："
              name="name"
              rules={[{ required: true, message: '请输入名称' }]}
            >
              <Input placeholder="名称" />
            </FormItem>
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
                rowKey="index"
                bordered
                dataSource={project.templates}
                //rowSelection={rowSelection}
              />
            </div>
          </Card>
        </Form>
      )}
    </PageHeaderWrapper>
  );
};

export default connect(
  ({
    loading,
    project,
  }: {
    loading: { effects: { [key: string]: boolean } };
    project: ProjectType;
  }) => ({
    submitting: loading.effects['project/submit'],
    project,
  }),
)(Project);
