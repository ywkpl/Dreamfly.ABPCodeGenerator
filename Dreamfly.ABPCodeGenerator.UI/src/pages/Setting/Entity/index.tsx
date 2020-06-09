import React, { useState } from 'react';
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
  Checkbox,
  Tag,
  Select,
} from 'antd';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import { ColumnProps } from 'antd/es/table';
import { PlusOutlined, DeleteOutlined } from '@ant-design/icons';
import { ProjectType, ProjectTemplate } from '../Project/model';
import { EntityItemMapType, EntityType, EntityItemType } from './model';

import styles from './index.less';
import { generatorCode } from './service';

const FormItem = Form.Item;
const { TextArea } = Input;

const Entity = () => {
  const [editModelForm] = Form.useForm();
  const [mainForm] = Form.useForm();
  const [templates, setTemplates] = useState<ProjectTemplate[]>([]);
  const [editModelVisible, setEditModelVisible] = useState<boolean>(false);
  const [submitting, setSubmitting] = useState(false);
  const [isEdit, setIsEdit] = useState(false);
  const [selectedKeys, setSelectedKeys] = useState<string[]>([]);
  const [itemJson, setItemJson] = useState('');

  const handleAdd = () => {
    setIsEdit(false);
    editModelForm.setFieldsValue({
      file: 'Templates/',
      isExecute: true,
    });
    setEditModelVisible(true);
    console.log(Object.keys(EntityItemMapType));
  };

  const columns: ColumnProps<ProjectTemplate>[] = [
    {
      key: 'name',
      title: '名称',
      dataIndex: 'name',
    },
    {
      key: 'type',
      title: '类型',
      dataIndex: 'type',
    },
    {
      key: 'length',
      title: '长度',
      dataIndex: 'length',
    },
    {
      key: 'isRequired',
      title: '是否必填',
      dataIndex: 'isRequired',
      align: 'center',
      render: (value) => {
        return <Checkbox checked={value} />;
      },
    },
    {
      key: 'description',
      title: '描述',
      dataIndex: 'description',
    },
    {
      key: 'mapTypes',
      title: '映射类型',
      dataIndex: 'mapTypes',
      render: (value: EntityItemMapType[]) => {
        return (
          <div>
            {value &&
              value.map((t: EntityItemMapType) => {
                return <Tag color="processing">{EntityItemMapType[t]}</Tag>;
              })}
          </div>
        );
      },
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

  const childrens: JSX.Element[] = () => {
    const options = [];
    const mapTypes = Object.keys(EntityItemMapType);
    if (mapTypes && mapTypes.length > 0) {
      for (let i = 0; i < mapTypes.length / 2; i++) {
        options.push(
          <Select.Option value={mapTypes[i]}>{mapTypes[i + mapTypes.length / 2]}</Select.Option>,
        );
      }
    }
    return options;
  };

  const modelEdit = (
    <Modal
      title="实体明细"
      destroyOnClose
      visible={editModelVisible}
      onOk={handleEditModelOk}
      okText="保存"
      onCancel={handleEditModelCancel}
    >
      <Form style={{ marginTop: 8 }} form={editModelForm} name="model">
        <FormItem
          {...formAllItemLayout}
          label="名称"
          name="name"
          rules={[
            {
              required: true,
              message: '请输入名称',
            },
          ]}
        >
          <Input placeholder="名称" />
        </FormItem>
        <FormItem
          {...formAllItemLayout}
          label="类型"
          name="type"
          rules={[
            {
              required: true,
              message: '请输入类型',
            },
          ]}
        >
          <Input placeholder="类型" />
        </FormItem>

        <FormItem {...formAllItemLayout} label="长度" name="length">
          <Input placeholder="长度" />
        </FormItem>

        <FormItem {...formAllItemLayout} label="是否必填" name="isRequired" valuePropName="checked">
          <Switch />
        </FormItem>

        <FormItem {...formAllItemLayout} label="描述" name="description">
          <Input placeholder="描述" />
        </FormItem>

        <FormItem {...formAllItemLayout} label="输出类别" name="mapTypes">
          <Select mode="multiple" style={{ width: '100%' }} placeholder="输出类别">
            {childrens()}
            {/* {Object.keys(EntityItemMapType).map(t=>(<Option key={t.} ></Option>))} */}
          </Select>
          {/* <Input placeholder="输出类别" /> */}
        </FormItem>
      </Form>
    </Modal>
  );

  const handleSave = () => {
    mainForm.validateFields().then((values) => {
      setSubmitting(true);
      console.log(values);
      // generatorCode({
      //   ...values,
      //   templates,
      // }).then(() => {
      //   message.success('产生成功！');
      //   setSubmitting(false);
      // });
      setTimeout(() => {
        setSubmitting(false);
      }, 3000);
    });
  };

  const saveButton = (
    <Button type="primary" htmlType="submit" onClick={handleSave} loading={submitting}>
      生成
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
          <Card bordered={false} title="实体">
            <FormItem
              {...formItemLayout}
              label="名称"
              name="entity"
              rules={[{ required: true, message: '请输入名称' }]}
            >
              <Input placeholder="名称" />
            </FormItem>
            <FormItem
              {...formItemLayout}
              label="目录"
              name="Module"
              rules={[{ required: true, message: '请输入目录' }]}
            >
              <Input placeholder="目录" />
            </FormItem>
          </Card>
          <Card bordered={false} title="明细">
            <div className={styles.tableList}>
              <TextArea
                placeholder="明细Json"
                autoSize={{ minRows: 5 }}
                value={itemJson}
                onChange={(e) => {
                  setItemJson(e.target.value);
                }}
              />
              <Button
                type="primary"
                style={{ marginTop: '10px' }}
                onClick={() => {
                  const items = JSON.parse(itemJson) as EntityItemMapType[];
                  console.log(items);
                }}
              >
                导入
              </Button>
              <Divider />
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

export default Entity;
