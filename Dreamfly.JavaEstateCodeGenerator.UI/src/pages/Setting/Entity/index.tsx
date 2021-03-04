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
  InputNumber,
} from 'antd';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import { ColumnProps } from 'antd/es/table';
import { PlusOutlined, DeleteOutlined } from '@ant-design/icons';
import { EntityItemType } from './model';
import styles from './index.less';
import {
  generatorCode,
  importEntityFromExcel,
  importEntityFromDb,
  removeCode,
  getEntity,
} from './service';
import request from '@/utils/request';

const FormItem = Form.Item;
const { TextArea } = Input;
const Option = Select.Option;

const Entity = () => {
  const [editModelForm] = Form.useForm();
  const [importModelForm] = Form.useForm();
  const [loadModelForm] = Form.useForm();
  const [mainForm] = Form.useForm();
  const [importFromDb] = Form.useForm();
  const [entityItems, setEntityItems] = useState<EntityItemType[]>([]);
  const [editModelVisible, setEditModelVisible] = useState<boolean>(false);
  const [importModelVisible, setImportModelVisible] = useState<boolean>(false);
  const [loadModelVisible, setLoadModelVisible] = useState<boolean>(false);
  const [importFromDbVisible, setImportFromDbVisible] = useState<boolean>(false);
  const [submitting, setSubmitting] = useState(false);
  const [isEdit, setIsEdit] = useState(false);
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
  const [selectedRows, setSelectedRows] = useState<any[]>([]);
  const [itemJson, setItemJson] = useState('');

  const handleAdd = () => {
    setIsEdit(false);
    editModelForm.setFieldsValue({
      name: '',
      columnName: '',
      type: '',
      length: null,
      fraction: null,
      hasTime: false,
      isRequired: false,
      description: '',
      inQuery: false,
      inCreate: true,
      inResponse: true,
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
      key: 'columnName',
      title: '列名',
      dataIndex: 'columnName',
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
      key: 'fraction',
      title: '小数精度',
      dataIndex: 'fraction',
    },
    {
      key: 'hasTime',
      title: '包含时间',
      dataIndex: 'hasTime',
      align: 'center',
      render: (value) => {
        return <Checkbox checked={value} disabled />;
      },
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
      key: 'inQuery',
      title: '查询包含',
      dataIndex: 'inQuery',
      align: 'center',
      render: (value) => {
        return <Checkbox checked={value} disabled />;
      },
    },
    {
      key: 'inCreate',
      title: '新建包含',
      dataIndex: 'inCreate',
      align: 'center',
      render: (value) => {
        return <Checkbox checked={value} disabled />;
      },
    },
    {
      key: 'inResponse',
      title: '响应包含',
      dataIndex: 'inResponse',
      align: 'center',
      render: (value) => {
        return <Checkbox checked={value} disabled />;
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
              editModelForm.resetFields();
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
      const editItem = entityItems.find(
        (t) => t.name.toLowerCase() === entityItem.name.toLowerCase(),
      );
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

  const handleImportModelOk = () => {
    importModelForm.validateFields().then((values) => {
      setSelectedRowKeys([]);
      importEntityFromExcel(values).then((response) => {
        mainForm.setFieldsValue(response);
        setEntityItems(response.entityItems);
      });
      setImportModelVisible(false);
    });
  };

  const handleImportFromDbCancel = () => {
    setImportFromDbVisible(false);
  };

  const handleImportFromDbOk = () => {
    importFromDb.validateFields().then((values) => {
      setSelectedRowKeys([]);
      console.log(values);
      importEntityFromDb(values).then((response) => {
        mainForm.setFieldsValue(response);
        setEntityItems(response.entityItems);
      });
      setImportFromDbVisible(false);
    });
  };

  const handleLoadModelOk = () => {
    loadModelForm.validateFields().then((values) => {
      setSelectedRowKeys([]);
      getEntity(values.entityName).then((response) => {
        mainForm.setFieldsValue(response);
        setEntityItems(response.entityItems);
      });
      setLoadModelVisible(false);
    });
  };

  const handleImportModelCancel = () => {
    setImportModelVisible(false);
  };

  const handleLoadModelCancel = () => {
    setLoadModelVisible(false);
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
        <FormItem {...formAllItemLayout} label="字段名" name="columnName">
          <Input placeholder="字段名" />
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
          <Select placeholder="变量类型" defaultValue="String" style={{ width: 150 }}>
            <Option value="String">String</Option>
            <Option value="Long">Long</Option>
            <Option value="Integer">Integer</Option>
            <Option value="Boolean">Boolean</Option>
            <Option value="Date">Date</Option>
            <Option value="Float">Float</Option>
            <Option value="Json">Json</Option>
            <Option value="BigDecimal">BigDecimal</Option>
          </Select>
        </FormItem>

        <FormItem {...formAllItemLayout} label="长度" name="length">
          <InputNumber placeholder="长度" style={{ width: 150 }} />
        </FormItem>
        <FormItem {...formAllItemLayout} label="小数精度" name="fraction">
          <InputNumber placeholder="小数精度" style={{ width: 150 }} />
        </FormItem>
        <FormItem {...formAllItemLayout} label="包含时间" name="hasTime" valuePropName="checked">
          <Switch />
        </FormItem>
        <FormItem {...formAllItemLayout} label="是否必填" name="isRequired" valuePropName="checked">
          <Switch />
        </FormItem>

        <FormItem {...formAllItemLayout} label="描述" name="description">
          <Input placeholder="描述" />
        </FormItem>
        <FormItem {...formAllItemLayout} label="查询包含" name="inQuery" valuePropName="checked">
          <Switch />
        </FormItem>
        <FormItem {...formAllItemLayout} label="新增包含" name="inCreate" valuePropName="checked">
          <Switch />
        </FormItem>
        <FormItem {...formAllItemLayout} label="响应包含" name="inResponse" valuePropName="checked">
          <Switch />
        </FormItem>
      </Form>
    </Modal>
  );

  const modelImport = (
    <Modal
      title="导入条件"
      destroyOnClose
      visible={importModelVisible}
      onOk={handleImportModelOk}
      okText="确定"
      onCancel={handleImportModelCancel}
    >
      <Form style={{ marginTop: 8 }} form={importModelForm} name="model">
        <FormItem
          {...formAllItemLayout}
          label="页签索引"
          name="tabIndex"
          rules={[
            {
              required: true,
              message: '请输入页签索引',
            },
          ]}
        >
          <InputNumber placeholder="页签索引" style={{ width: '80%' }} />
        </FormItem>
        <FormItem
          {...formAllItemLayout}
          label="起始行号"
          name="startRow"
          rules={[
            {
              required: true,
              message: '请输入起始行号',
            },
          ]}
        >
          <InputNumber placeholder="起始行号" style={{ width: '80%' }} />
        </FormItem>
        <FormItem
          {...formAllItemLayout}
          label="截止行号"
          name="endRow"
          rules={[
            {
              required: true,
              message: '请输入截止行号',
            },
          ]}
        >
          <InputNumber placeholder="截止行号" style={{ width: '80%' }} />
        </FormItem>
      </Form>
    </Modal>
  );

  const modelImportFromDb = (
    <Modal
      title="导入条件"
      destroyOnClose
      visible={importFromDbVisible}
      onOk={handleImportFromDbOk}
      okText="确定"
      onCancel={handleImportFromDbCancel}
    >
      <Form style={{ marginTop: 8 }} form={importFromDb} name="model">
        <FormItem
          {...formAllItemLayout}
          label="表名"
          name="tableName"
          rules={[
            {
              required: true,
              message: '请输入表名',
            },
          ]}
        >
          <Input placeholder="表名" style={{ width: '80%' }} />
        </FormItem>
      </Form>
    </Modal>
  );

  const modelLoad = (
    <Modal
      title="载入条件"
      destroyOnClose
      visible={loadModelVisible}
      onOk={handleLoadModelOk}
      okText="确定"
      onCancel={handleLoadModelCancel}
    >
      <Form style={{ marginTop: 8 }} form={loadModelForm} name="model">
        <FormItem
          {...formAllItemLayout}
          label="实体名称"
          name="entityName"
          rules={[
            {
              required: true,
              message: '请输入实体名称',
            },
          ]}
        >
          <Input placeholder="实体名称" />
        </FormItem>
      </Form>
    </Modal>
  );

  const handleImportFromDb = () => {
    importFromDb.resetFields();
    importFromDb.setFieldsValue({ tabIndex: 0 });
    setImportFromDbVisible(true);
  };

  const handleImport = () => {
    importModelForm.resetFields();
    importModelForm.setFieldsValue({ tabIndex: 0 });
    setImportModelVisible(true);
  };

  const handleLoad = () => {
    loadModelForm.resetFields();
    loadModelForm.setFieldsValue({ entityName: '' });
    setLoadModelVisible(true);
  };

  const handleTest = () => {
    var url = 'http://test.dreamfly.com:9201/auth/oauth/token';
    request(url, {
      method: 'post',
      data: {
        grant_type: 'password',
        client_id: 'newcity',
        client_secret: '23101680',
        username: 'admin',
        password: '123456',
      },
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    })
      .then((response) => {
        console.log(response);
      })
      .catch((err) => {
        console.log(err);
      });
  };

  const handleSave = () => {
    mainForm.validateFields().then((values) => {
      if (entityItems.length === 0) {
        message.error('明细项目必须有值！');
        return;
      }

      const para = {
        name: values.name,
        tableName: values.tableName,
        description: values.description,
        hasIHasCompany: values.hasIHasCompany,
        hasIHasTenant: values.hasIHasTenant,
        isSync: values.isSync,
        entityItems,
      };

      console.log(para);
      setSubmitting(true);
      generatorCode(para).then((response: Response) => {
        if (!response) {
          message.success('生成成功！');
        }
        setSubmitting(false);
      });
    });
  };

  const handleDelete = () => {
    mainForm.validateFields().then((values) => {
      if (entityItems.length === 0) {
        message.error('明细项目必须有值！');
        return;
      }

      const para = {
        name: values.name,
        tableName: values.tableName,
        description: values.description,
        entityItems,
      };

      console.log(para);
      setSubmitting(true);
      removeCode(para).then((response: Response) => {
        if (!response) {
          message.success('清除成功！');
        }
        setSubmitting(false);
      });
    });
  };

  const saveButton = (
    <>
      <Button type="primary" htmlType="submit" onClick={handleImportFromDb}>
        数据库导入
      </Button>
      <Button type="primary" htmlType="submit" onClick={handleImport}>
        Excel导入
      </Button>
      <Button type="primary" htmlType="submit" onClick={handleLoad}>
        现有载入
      </Button>
      <Button type="primary" htmlType="submit" onClick={handleTest}>
        测试
      </Button>
      <Button type="primary" htmlType="submit" onClick={handleSave} loading={submitting}>
        生成
      </Button>
      <Button type="primary" htmlType="submit" onClick={handleDelete} loading={submitting}>
        清除
      </Button>
    </>
  );

  const rowSelection = {
    selectedRowKeys: selectedRowKeys,
    onChange: (selectedRowKeys: React.Key[], selectedRows: any[]) => {
      setSelectedRowKeys(selectedRowKeys);
      setSelectedRows(selectedRows);
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
              name="name"
              rules={[{ required: true, message: '请输入名称' }]}
            >
              <Input placeholder="名称" />
            </FormItem>
            <FormItem {...formItemLayout} label="表名" name="tableName">
              <Input placeholder="表名" />
            </FormItem>
            <FormItem
              {...formItemLayout}
              label="描述"
              name="description"
              rules={[{ required: true, message: '请输入描述' }]}
            >
              <Input placeholder="描述" />
            </FormItem>
            <FormItem
              {...formItemLayout}
              label="过滤公司接口"
              name="hasIHasCompany"
              valuePropName="checked"
            >
              <Switch />
            </FormItem>
            <FormItem
              {...formItemLayout}
              label="过滤租户接口"
              name="hasIHasTenant"
              valuePropName="checked"
            >
              <Switch />
            </FormItem>
            <FormItem {...formItemLayout} label="同步状态" name="isSync" valuePropName="checked">
              <Switch disabled />
            </FormItem>
          </Card>
          <Card bordered={false} title="明细">
            <div className={styles.tableList}>
              <FormItem name="itemJson">
                <TextArea
                  placeholder="明细Json"
                  autoSize={{ minRows: 6, maxRows: 6 }}
                  value={itemJson}
                  onChange={(e) => {
                    setItemJson(e.target.value);
                  }}
                />
              </FormItem>
              <Button
                type="primary"
                onClick={() => {
                  if (itemJson && itemJson.length > 0) {
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
                        "columnName":"test_name",
                        "type": "String",
                        "length": 20,
                        "isRequired": true,
                        "description": "名称",
                        "inQuery":true,
                        "inCreate":true,
                        "inResponse":true
                    },
                    {
                        "name": "age",
                        "type": "Integer",
                        "description": "年龄",
                        "inQuery":true,
                        "inResponse":true
                    },
                    {
                        "name": "memo",
                        "type": "String",
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
                    const removedItems = entityItems.filter(
                      (t) => !selectedRowKeys.includes(t.name),
                    );
                    setEntityItems([...removedItems]);
                    setSelectedRowKeys([]);
                  }}
                  disabled={selectedRowKeys.length === 0}
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
      {importModelVisible && modelImport}
      {loadModelVisible && modelLoad}
      {importFromDbVisible && modelImportFromDb}
    </PageHeaderWrapper>
  );
};

export default Entity;
