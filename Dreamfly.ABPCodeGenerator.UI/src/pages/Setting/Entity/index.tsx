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
import { EntityItemMapType, EntityItemType } from './model';
import styles from './index.less';
import { generatorCode } from './service';

const FormItem = Form.Item;
const { TextArea } = Input;

const Entity = () => {
  const [editModelForm] = Form.useForm();
  const [mainForm] = Form.useForm();
  const [entityItems, setEntityItems] = useState<EntityItemType[]>([]);
  const [editModelVisible, setEditModelVisible] = useState<boolean>(false);
  const [submitting, setSubmitting] = useState(false);
  const [isEdit, setIsEdit] = useState(false);
  const [selectedKeys, setSelectedKeys] = useState<string[]>([]);
  const [itemJson, setItemJson] = useState('');

  const handleAdd = () => {
    setIsEdit(false);
    editModelForm.setFieldsValue({
      name: '',
      type: '',
      length: null,
      isRequired: false,
      description: '',
      mapTypes: [],
    });
    setEditModelVisible(true);
  };

  const columns: ColumnProps<EntityItemType>[] = [
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
        return <Checkbox checked={value} disabled />;
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
      render: (record: EntityItemType) => (
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
              const removedItems = entityItems.filter((t) => t.name !== record.name);
              setEntityItems([...removedItems]);
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
      const entityItem = values as EntityItemType;
      const editItem = entityItems.find((t) => t.name === entityItem.name);
      if (isEdit) {
        if (editItem) {
          const index = entityItems.indexOf(editItem);
          entityItems.splice(index, 1, entityItem);
          setEntityItems([...entityItems]);
        }
      } else {
        if (editItem) {
          message.error('实体项目已存在，请确认！');
          return;
        }
        setEntityItems([...entityItems, entityItem]);
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

  const mapTypeItems = () => {
    const options = [];
    const mapTypes = Object.keys(EntityItemMapType);
    if (mapTypes && mapTypes.length > 0) {
      for (let i = 0; i < mapTypes.length / 2; i += 1) {
        options.push(
          <Select.Option value={mapTypes[i]} key={mapTypes[i]}>
            {mapTypes[i + mapTypes.length / 2]}
          </Select.Option>,
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
          label="变量类型"
          name="type"
          rules={[
            {
              required: true,
              message: '请输入变量类型',
            },
          ]}
        >
          <Input placeholder="变量类型" style={{ width: 250 }} />
        </FormItem>

        <FormItem {...formAllItemLayout} label="最大长度" name="length">
          <Input type="number" placeholder="最大长度" style={{ width: 150 }} />
        </FormItem>

        <FormItem {...formAllItemLayout} label="是否必填" name="isRequired" valuePropName="checked">
          <Switch />
        </FormItem>

        <FormItem {...formAllItemLayout} label="描述" name="description">
          <Input placeholder="描述" />
        </FormItem>

        <FormItem {...formAllItemLayout} label="输出类别" name="mapTypes">
          <Select mode="multiple" style={{ width: '100%' }} placeholder="输出类别">
            {mapTypeItems()}
          </Select>
        </FormItem>
      </Form>
    </Modal>
  );

  const handleSave = () => {
    mainForm.validateFields().then((values) => {
      setSubmitting(true);
      generatorCode(values).then(() => {
        message.success('生成成功！');
        setSubmitting(false);
      });
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
              <FormItem name="itemJson">
                <TextArea
                  placeholder="明细Json"
                  autoSize={{ minRows: 5, maxRows: 10 }}
                  value={itemJson}
                  onChange={(e) => {
                    setItemJson(e.target.value);
                  }}
                />
              </FormItem>
              <Button
                type="primary"
                onClick={() => {
                  const items = JSON.parse(itemJson) as EntityItemType[];
                  if (items && items.length > 0) {
                    const itemNames = items.map((t) => t.name);
                    const existsNames = entityItems.filter((t) => itemNames.includes(t.name));
                    if (existsNames.length > 0) {
                      message.error('已存在相同名称其次，请确认！');
                      return;
                    }
                    const newItmes = entityItems.concat(items);
                    setEntityItems([...newItmes]);
                  }
                }}
              >
                导入
              </Button>
              <Button
                type="primary"
                style={{ marginLeft: '10px' }}
                onClick={() => {
                  const data = `[
                    {
                        "name": "name",
                        "type": "string",
                        "length": 20,
                        "isRequired": true,
                        "description": "名称",
                        "mapTypes": [
                            0,
                            1,
                            2
                        ]
                    },
                    {
                        "name": "age",
                        "type": "int",
                        "description": "年龄",
                        "mapTypes": [
                            0,
                            2
                        ]
                    },
                    {
                        "name": "memo",
                        "type": "string",
                        "length": 400
                    }
                ]`;
                  setItemJson(data);
                  mainForm.setFieldsValue({ itemJson: data });
                }}
              >
                生成示范数据
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
                    const removedItems = entityItems.filter((t) => !selectedKeys.includes(t.name));
                    setSelectedKeys([]);
                    setEntityItems([...removedItems]);
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
                rowKey="name"
                bordered
                dataSource={entityItems}
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
