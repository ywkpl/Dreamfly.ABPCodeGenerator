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
import { PlusOutlined, DeleteOutlined } from '@ant-design/icons';
import { ProjectType, ProjectTemplate } from './model';
import styles from './index.less';
import { getProject, updateProject } from './service';

const FormItem = Form.Item;

const Project = () => {
  const [editModelForm] = Form.useForm();
  const [mainForm] = Form.useForm();
  const [templates, setTemplates] = useState<ProjectTemplate[]>([]);
  const [editModelVisible, setEditModelVisible] = useState<boolean>(false);
  const [submitting, setSubmitting] = useState(false);
  const [isEdit, setIsEdit] = useState(false);
  const [selectedKeys, setSelectedKeys] = useState<string[]>([]);

  const handleAdd = () => {
    setIsEdit(false);
    editModelForm.setFieldsValue({
      file: 'Templates/',
      remark: '',
      isExecute: true,
      outputFolder: '',
      outputName: '',
    });
    setEditModelVisible(true);
  };

  useEffect(() => {
    getProject({}).then((va: ProjectType) => {
      mainForm.setFieldsValue(va);
      setTemplates(va.templates);
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
      render: (value, row) => {
        return (
          <Switch
            checked={value}
            onChange={(v) => {
              const editItem = templates.find((t) => t.file === row.file);
              if (editItem) {
                const index = templates.indexOf(editItem);
                editItem.isExecute = v;
                templates.splice(index, 1, editItem);
                setTemplates([...templates]);
              }
            }}
          />
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
      title: '操作',
      render: (record: ProjectTemplate) => (
        <>
          <Button
            type="link"
            onClick={() => {
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
              const removedItems = templates.filter((t) => t.file !== record.file);
              setTemplates([...removedItems]);
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
      const editItem = templates.find((t) => t.file === templateItem.file);
      if (isEdit) {
        if (editItem) {
          const index = templates.indexOf(editItem);
          templates.splice(index, 1, templateItem);
          setTemplates([...templates]);
        }
      } else {
        if (editItem) {
          message.error('模板路径已存在，请确认！');
          return;
        }
        setTemplates([...templates, templateItem]);
      }
      setEditModelVisible(false);
    });
  };

  const handleEditModelCancel = () => {
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
      </Form>
    </Modal>
  );

  const handleSave = () => {
    mainForm.validateFields().then((values) => {
      setSubmitting(true);
      updateProject({
        ...values,
        templates,
      }).then((response: Request) => {
        if (!response) {
          message.success('保存成功！');
        }
        setSubmitting(false);
      });
    });
  };

  const saveButton = (
    <Button type="primary" htmlType="submit" onClick={handleSave} loading={submitting}>
      保存
    </Button>
  );

  const rowSelection = {
    onChange: (selectedRowKeys: any[]) => {
      setSelectedKeys(selectedRowKeys);
    },
  };

  return (
    <PageHeaderWrapper extra={saveButton}>
      <Form style={{ marginTop: 8 }} form={mainForm} name="main">
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
              label="输出目录"
              name="outputPath"
              rules={[{ required: true, message: '请输入输出目录' }]}
            >
              <Input placeholder="输出目录" />
            </FormItem>
            <FormItem
              {...formItemLayout}
              label="包路径"
              name="packagePath"
              rules={[{ required: true, message: '请输入包路径' }]}
            >
              <Input placeholder="包路径" />
            </FormItem>
            <FormItem
              {...formItemLayout}
              label="版本"
              name="version"
              rules={[{ required: true, message: '请输入版本号' }]}
            >
              <Input placeholder="版本" />
            </FormItem>
            <FormItem {...formItemLayout} label="包含API项目" name="hasApi" valuePropName="checked">
              <Switch />
            </FormItem>
            <FormItem {...formItemLayout} label="共享項目否" name="isShare" valuePropName="checked">
              <Switch />
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
                <Button
                  icon={<DeleteOutlined />}
                  type="primary"
                  danger
                  onClick={() => {
                    const removedItems = templates.filter((t) => !selectedKeys.includes(t.file));
                    setTemplates([...removedItems]);
                    setSelectedKeys([]);
                  }}
                  disabled={selectedKeys.length === 0}
                >
                  删除
                </Button>
              </div>
              <Divider />
              <Table
                pagination={false}
                columns={columns}
                rowKey="file"
                bordered
                dataSource={templates}
                rowSelection={rowSelection}
              />
            </div>
          </Card>
        </Space>
      </Form>
      {editModelVisible && modelEdit}
    </PageHeaderWrapper>
  );
};

export default Project;
