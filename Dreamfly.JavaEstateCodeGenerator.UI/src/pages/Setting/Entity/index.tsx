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
  Row,
  Select,
  InputNumber,
  Col,
} from 'antd';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import { ColumnProps } from 'antd/es/table';
import { PlusOutlined, DeleteOutlined } from '@ant-design/icons';
import { EntityItemType, SavedEntity } from './model';
import styles from './index.less';
import {
  generatorCode,
  importEntityFromExcel,
  importEntityFromDb,
  removeCode,
  getEntity,
  save,
  update,
  saveTest,
  syncEntityFromDb,
} from './service';
import request from '@/utils/request';

const FormItem = Form.Item;
const { TextArea } = Input;
const { Option } = Select;

const Entity = (): JSX.Element => {
  const [editModelForm] = Form.useForm();
  const [importModelForm] = Form.useForm();
  const [loadModelForm] = Form.useForm();
  const [mainForm] = Form.useForm();
  const [importFromDb] = Form.useForm();
  const [syncFromDb] = Form.useForm();
  const [entityItems, setEntityItems] = useState<EntityItemType[]>([]);
  const [editModelVisible, setEditModelVisible] = useState<boolean>(false);
  const [importModelVisible, setImportModelVisible] = useState<boolean>(false);
  const [loadModelVisible, setLoadModelVisible] = useState<boolean>(false);
  const [importFromDbVisible, setImportFromDbVisible] = useState<boolean>(false);
  const [syncFromDbVisible, setSyncFromDbVisible] = useState<boolean>(false);
  const [submitting, setSubmitting] = useState(false);
  const [isEdit, setIsEdit] = useState(false);
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
  const [selectedRows, setSelectedRows] = useState<any[]>([]);
  const [sql, setSql] = useState('');
  const [checkEntityName, setCheckEntityName] = useState(false);

  const handleAdd = () => {
    setIsEdit(false);
    editModelForm.setFieldsValue({
      id: null,
      index: entityItems.length,
      name: '',
      columnName: '',
      type: '',
      length: null,
      fraction: null,
      isRequired: false,
      description: '',
      inQuery: false,
      inCreate: true,
      inAllResponse: true,
      inResponse: false,
      relateType: null,
      cascadeType: null,
      fetchType: null,
      relateEntity: null,
      relateDirection: null,
      joinName: null,
      foreignKeyName: null,
      relateEntityInModule: false,
      order:
        entityItems.length > 0
          ? Math.max.apply(
              Math,
              entityItems.map((t) => t.order),
            ) + 10
          : 10,
    });
    setEditModelVisible(true);
  };

  const columns: ColumnProps<EntityItemType>[] = [
    // {
    //   key: 'id',
    //   title: '编号',
    //   dataIndex: 'id',
    // },
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
      key: 'isRequired',
      title: '必填',
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
      key: 'relateType',
      title: '关联关系',
      dataIndex: 'relateType',
    },
    {
      key: 'relateEntity',
      title: '关联实体',
      dataIndex: 'relateEntity',
    },
    {
      key: 'relateDirection',
      title: '关联方向',
      dataIndex: 'relateDirection',
    },
    {
      key: 'order',
      title: '排序',
      dataIndex: 'order',
      sorter: true,
    },
    {
      key: 'inQuery',
      title: '查询',
      dataIndex: 'inQuery',
      align: 'center',
      render: (value) => {
        return <Checkbox checked={value} disabled />;
      },
    },
    {
      key: 'inCreate',
      title: '新建',
      dataIndex: 'inCreate',
      align: 'center',
      render: (value) => {
        return <Checkbox checked={value} disabled />;
      },
    },
    {
      key: 'inAllResponse',
      title: '全部响应',
      dataIndex: 'inAllResponse',
      align: 'center',
      render: (value) => {
        return <Checkbox checked={value} disabled />;
      },
    },
    {
      key: 'inResponse',
      title: '一般响应',
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
              setCheckEntityName(record.relateType != null);
              setEditModelVisible(true);
            }}
          >
            编辑
          </Button>
          <Divider type="vertical" />
          <Popconfirm
            title="确认删除吗?"
            onConfirm={() => {
              const removedItems = entityItems.filter((t) => t.index !== record.index);
              setEntityItems([...removedItems]);
              setSelectedRowKeys([]);
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
      const editItem = entityItems?.find((t) => t.index === entityItem.index);
      if (isEdit) {
        if (editItem) {
          const index = entityItems.indexOf(editItem);
          entityItems.splice(index, 1, entityItem);
          setEntityItems([...entityItems]);
        }
      } else {
        const existsName = entityItems?.find(
          (t) => t.name.toLowerCase() === entityItem.name.toLowerCase(),
        );
        if (existsName) {
          message.error('实体项目已存在，请确认！');
          return;
        }
        setEntityItems([...entityItems, entityItem]);
      }
      setEditModelVisible(false);
    });
  };

  const handleEditModelCancel = () => {
    setCheckEntityName(false);
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

  const handleSyncFromDbOk = () => {
    syncFromDb.validateFields().then((values) => {
      setSelectedRowKeys([]);
      console.log(values);
      syncEntityFromDb(values).then((response) => {
        mainForm.setFieldsValue(response);
        const items = response.entityItems.map((item, index) => Object.assign({}, item, { index }));
        setEntityItems(items);
      });
      setSyncFromDbVisible(false);
    });
  };

  const handleSyncFromDbCancel = () => {
    setSyncFromDbVisible(false);
  };

  const handleImportFromDbOk = () => {
    importFromDb.validateFields().then((values) => {
      setSelectedRowKeys([]);
      console.log(values);
      importEntityFromDb(values).then((response) => {
        mainForm.setFieldsValue(response);
        const items = response.entityItems.map((item, index) => Object.assign({}, item, { index }));
        setEntityItems(items);
      });
      setImportFromDbVisible(false);
    });
  };

  const handleLoadModelOk = () => {
    loadModelForm.validateFields().then((values) => {
      setSelectedRowKeys([]);
      getEntity(values.entityName).then((response) => {
        mainForm.setFieldsValue(response);
        if (response.entityItems && response.entityItems.length > 0) {
          const items = response.entityItems.map((item, index) =>
            Object.assign({}, item, { index }),
          );
          setEntityItems(items);
        } else {
          setEntityItems([]);
        }
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

  const handleRelateTypeChange = (value: string) => {
    if (value) {
      setCheckEntityName(true);
      if (value === 'OneToOne' || value === 'ManyToOne') {
        editModelForm.setFieldsValue({
          fetchType: 'FetchType.LAZY',
        });
      } else {
        editModelForm.setFieldsValue({
          fetchType: null,
        });
      }
    } else {
      setCheckEntityName(false);
    }
  };

  const modelEdit = (
    <Modal
      title="实体明细"
      destroyOnClose
      visible={editModelVisible}
      onOk={handleEditModelOk}
      okText="保存"
      onCancel={handleEditModelCancel}
      width={900}
    >
      <Form style={{ marginTop: 8 }} form={editModelForm} name="model">
        <Row>
          <FormItem name="id">
            <Input type="hidden" />
          </FormItem>
          <FormItem name="index">
            <Input type="hidden" />
          </FormItem>
          <Col span={12}>
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
          </Col>
          <Col span={12}>
            <FormItem {...formAllItemLayout} label="列名/MappedBy" name="columnName">
              <Input placeholder="列名/MappedBy" />
            </FormItem>
          </Col>

          <Col span={12}>
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
                <Option value="Text">Text</Option>
                <Option value="Set">Set</Option>
              </Select>
            </FormItem>
          </Col>
          <Col span={12}>
            <FormItem {...formAllItemLayout} label="长度" name="length">
              <InputNumber placeholder="长度" style={{ width: 150 }} />
            </FormItem>
          </Col>
          <Col span={12}>
            <FormItem {...formAllItemLayout} label="小数精度" name="fraction">
              <InputNumber placeholder="小数精度" style={{ width: 150 }} />
            </FormItem>
          </Col>
          <Col span={12}>
            <FormItem
              {...formAllItemLayout}
              label="是否必填"
              name="isRequired"
              valuePropName="checked"
            >
              <Switch />
            </FormItem>
          </Col>

          <Col span={12}>
            <FormItem {...formAllItemLayout} label="关联关系" name="relateType">
              <Select
                placeholder="关联关系"
                style={{ width: 150 }}
                allowClear
                onChange={handleRelateTypeChange}
              >
                <Option value="OneToOne">一对一</Option>
                <Option value="OneToMany">一对多</Option>
                <Option value="ManyToOne">多对一</Option>
                <Option value="ManyToMany">多对多</Option>
              </Select>
            </FormItem>
          </Col>
          <Col span={12}>
            <FormItem {...formAllItemLayout} label="主维护属性" name="cascadeType">
              <Select placeholder="主维护属性" style={{ width: 150 }} allowClear>
                <Option value="CascadeType.ALL">ALL</Option>
                <Option value="CascadeType.MERGE">MERGE</Option>
                <Option value="CascadeType.DETACH">DETACH</Option>
                <Option value="CascadeType.REMOVE">REMOVE</Option>
              </Select>
            </FormItem>
          </Col>
          <Col span={12}>
            <FormItem {...formAllItemLayout} label="读取方式" name="fetchType">
              <Select placeholder="读取方式" style={{ width: 150 }} allowClear>
                <Option value="FetchType.LAZY">LAZY</Option>
                <Option value="FetchType.EAGER">EAGER</Option>
              </Select>
            </FormItem>
          </Col>
          <Col span={12}>
            <FormItem
              {...formAllItemLayout}
              label="关联实体名"
              name="relateEntity"
              rules={[
                {
                  required: checkEntityName,
                  message: '请输入关联实体名',
                },
              ]}
            >
              <Input placeholder="关联实体名" />
            </FormItem>
          </Col>
          <Col span={12}>
            <FormItem
              {...formAllItemLayout}
              label="本模块中"
              name="relateEntityInModule"
              valuePropName="checked"
            >
              <Switch />
            </FormItem>
          </Col>
          <Col span={12}>
            <FormItem
              {...formAllItemLayout}
              label="关联方向"
              name="relateDirection"
              rules={[
                {
                  required: checkEntityName,
                  message: '请选择关联方向',
                },
              ]}
            >
              <Select placeholder="关联方向" style={{ width: 150 }} allowClear>
                <Option value="Join">Join</Option>
                <Option value="MappedBy">MappedBy</Option>
              </Select>
            </FormItem>
          </Col>
          <Col span={12}>
            <FormItem {...formAllItemLayout} label="外键名称" name="foreignKeyName">
              <Input placeholder="外键名称" />
            </FormItem>
          </Col>
          <Col span={12}>
            <FormItem
              {...formAllItemLayout}
              label="描述"
              name="description"
              rules={[
                {
                  required: true,
                  message: '请输入描述',
                },
              ]}
            >
              <Input placeholder="描述" />
            </FormItem>
          </Col>
          <Col span={12}>
            <FormItem {...formAllItemLayout} label="排序" name="order">
              <InputNumber placeholder="排序" style={{ width: 150 }} step={10} />
            </FormItem>
          </Col>
          <Col span={12}>
            <FormItem
              {...formAllItemLayout}
              label="查询包含"
              name="inQuery"
              valuePropName="checked"
            >
              <Switch />
            </FormItem>
          </Col>
          <Col span={12}>
            <FormItem
              {...formAllItemLayout}
              label="新增包含"
              name="inCreate"
              valuePropName="checked"
            >
              <Switch />
            </FormItem>
          </Col>
          <Col span={12}>
            <FormItem
              {...formAllItemLayout}
              label="全部响应"
              name="inAllResponse"
              valuePropName="checked"
            >
              <Switch />
            </FormItem>
          </Col>
          <Col span={12}>
            <FormItem
              {...formAllItemLayout}
              label="一般响应"
              name="inResponse"
              valuePropName="checked"
            >
              <Switch />
            </FormItem>
          </Col>
        </Row>
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

  const modelSyncFromDb = (
    <Modal
      title="导入条件"
      destroyOnClose
      visible={syncFromDbVisible}
      onOk={handleSyncFromDbOk}
      okText="确定"
      onCancel={handleSyncFromDbCancel}
    >
      <Form style={{ marginTop: 8 }} form={syncFromDb} name="model">
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

  const handleSyncFromDb = () => {
    syncFromDb.resetFields();
    syncFromDb.setFieldsValue({ tabIndex: 0 });
    setSyncFromDbVisible(true);
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

  const handleSave = (onlySave: boolean = false) => {
    mainForm.validateFields().then((values) => {
      if (entityItems.length === 0) {
        message.error('明细项目必须有值！');
        return;
      }

      const param = { ...values, entityItems };
      console.log(param);
      setSubmitting(true);
      const callSave = onlySave ? saveTest(param) : generatorCode(param);
      callSave.then((response: SavedEntity) => {
        if (response) {
          message.success('执行成功！');
        }
        setSelectedRowKeys([]);
        console.log(response);
        if (response.entityDto && response.entityDto.entityItems) {
          mainForm.setFieldsValue(response.entityDto);
          const items = response.entityDto.entityItems.map((item, index) => ({ ...item, index }));
          setEntityItems(items);
        }
        console.log(response.sql);
        setSql(response.sql);
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
      <Button type="primary" htmlType="submit" onClick={handleSyncFromDb}>
        数据库同步
      </Button>
      <Button type="primary" htmlType="submit" onClick={handleImportFromDb}>
        数据库导入
      </Button>
      <Button type="primary" htmlType="submit" onClick={handleImport}>
        Excel导入
      </Button>
      <Button type="primary" htmlType="submit" onClick={handleLoad}>
        现有载入
      </Button>
      {/* <Button type="primary" htmlType="submit" onClick={handleTest}>
        测试
      </Button> */}
      <Button
        type="primary"
        htmlType="submit"
        onClick={() => handleSave(true)}
        loading={submitting}
      >
        保存修改
      </Button>
      <Button type="primary" htmlType="submit" onClick={() => handleSave()} loading={submitting}>
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
            <FormItem name="id">
              <Input type="hidden" />
            </FormItem>
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
            <FormItem {...formItemLayout} label="審計接口" name="hasAudit" valuePropName="checked">
              <Switch />
            </FormItem>
            <FormItem {...formItemLayout} label="同步状态" name="isSync" valuePropName="checked">
              <Switch disabled />
            </FormItem>
          </Card>
          <Card bordered={false} title="明细">
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
                    const removedItems = entityItems.filter(
                      (t) => !selectedRowKeys.includes(t.index),
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
                rowKey="index"
                bordered
                dataSource={entityItems}
                rowSelection={rowSelection}
              />
              <Divider />
              <TextArea
                placeholder="Sql语法"
                autoSize={{ minRows: 6, maxRows: 6 }}
                value={sql}
                // onChange={(e) => {
                //   setSql(e.target.value);
                // }}
              />
            </div>
          </Card>
        </Space>
      </Form>
      {editModelVisible && modelEdit}
      {importModelVisible && modelImport}
      {loadModelVisible && modelLoad}
      {importFromDbVisible && modelImportFromDb}
      {syncFromDbVisible && modelSyncFromDb}
    </PageHeaderWrapper>
  );
};

export default Entity;
