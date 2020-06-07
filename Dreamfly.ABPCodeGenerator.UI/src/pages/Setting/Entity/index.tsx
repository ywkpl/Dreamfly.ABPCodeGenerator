import React, { useEffect, useState } from 'react';
import {
  Form,
  Card,
  Input,
  Button,
  Table,
  Divider,
  Space,
  Switch,
  Modal,
  message,
  Popconfirm,
} from 'antd';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import { ColumnProps } from 'antd/es/table';
import { ProjectType, ProjectTemplate } from '../Project/model';
import styles from './index.less';
import { getProject, updateProject } from '../Project/service';

const FormItem = Form.Item;

const Entity = () => {
  const [editModelForm] = Form.useForm();
  const [mainForm] = Form.useForm();
  const [project, setProject] = useState<ProjectType>({} as ProjectType);
  const [editModelVisible, setEditModelVisible] = useState<boolean>(false);
  const [submitting, setSubmitting] = useState(false);
  const [isEdit, setIsEdit] = useState(false);
  const [template, setTemplate] = useState<ProjectTemplate>();

  const handleAdd = () => {
    setIsEdit(false);
    setTemplate({
      file: 'Templates/',
      remark: '1',
      isExecute: true,
      outputFolder: '1',
      outputName: '1',
      projectFile: '1',
    });
    editModelForm.setFieldsValue({
      file: 'Templates/',
      remark: '1',
      isExecute: true,
      outputFolder: '1',
      outputName: '1',
      projectFile: '1',
    });
    setEditModelVisible(true);
  };

  useEffect(() => {
    getProject({}).then((va: ProjectType) => {
      setProject(va);
    });
  }, []);

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
    {
      title: '操作',
      render: (record: ProjectTemplate) => (
        <>
          <Button
            type="link"
            onClick={() => {
              setTemplate(record);
              setIsEdit(true);
              editModelForm.setFieldsValue(record);
              setEditModelVisible(true);
            }}
          >
            编辑
          </Button>
          <Divider type="vertical" />
          <Popconfirm
            title="确认删除吗?"
            onConfirm={() => {
              console.log(`删除${record.file}`);
            }}
          >
            <a>删除</a>
          </Popconfirm>
        </>
      ),
    },
  ];

  const handleEditModelOk = () => {
    editModelForm.validateFields().then((values) => {
      const templateItem = values as ProjectTemplate;
      if (isEdit) {
        const editItem = project.templates.find(
          (t: ProjectTemplate) => t.file === templateItem.file,
        );
        if (editItem && templateItem) {
          const index = project.templates.indexOf(editItem);
          project.templates.splice(index, 1, templateItem);
          project.templates = [...project.templates];
        }
      } else {
        project.templates = [...project.templates, values as ProjectTemplate];
      }
      setProject(project);
      setTemplate({} as ProjectTemplate);
      editModelForm.resetFields();
      setEditModelVisible(false);
    });
  };

  const handleEditModelCancel = () => {
    editModelForm.resetFields();
    setEditModelVisible(false);
  };

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

  const modelEdit = (
    <Modal
      title="模板明细"
      destroyOnClose
      visible={editModelVisible}
      onOk={handleEditModelOk}
      okText="保存"
      onCancel={handleEditModelCancel}
    >
      {template && Object.keys(template).length !== 0 && (
        <Form style={{ marginTop: 8 }} form={editModelForm} name="model">
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
            <Input placeholder="模板路径" disabled={isEdit} />
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

          <FormItem
            {...formAllItemLayout}
            label="是否生成"
            name="isExecute"
            valuePropName="checked"
          >
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
      )}
    </Modal>
  );

  const handleSave = () => {
    mainForm.validateFields().then((values) => {
      setSubmitting(true);
      console.log({
        ...values,
        templates: project.templates,
      });
      setTimeout(() => {
        setSubmitting(false);
      }, 3000);
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
    <Button type="primary" htmlType="submit" onClick={handleSave} loading={submitting}>
      保存
    </Button>
  );

  const rowSelection = {
    onChange: (selectedRowKeys: any[], selectedRows: ProjectTemplate[]) => {
      console.log(`selectedRowKeys: ${selectedRowKeys}`, 'selectedRows: ', selectedRows);
    },
  };

  return (
    <PageHeaderWrapper extra={saveButton}>
      {Object.keys(project).length !== 0 && (
        <Form style={{ marginTop: 8 }} form={mainForm} initialValues={project} name="main">
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
                  <Button icon="plus" type="primary" onClick={handleAdd}>
                    新建
                  </Button>
                </div>
                <Divider />
                <Table
                  pagination={false}
                  columns={columns}
                  rowKey="file"
                  bordered
                  dataSource={project.templates}
                  rowSelection={rowSelection}
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

export default Entity;
