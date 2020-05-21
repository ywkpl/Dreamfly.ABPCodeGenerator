import React, { Component } from 'react';
import { Form, Card, Input, Button } from 'antd';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import { connect, Dispatch } from 'umi';
import { ProjectType } from './model';

const FormItem = Form.Item;

interface ProjectProps {
  project: ProjectType;
  submitting: boolean;
  dispatch: Dispatch<any>;
}

interface State {}

class Project extends Component<ProjectProps, State> {
  constructor(props: ProjectProps) {
    super(props);
    this.state = {};
  }

  componentDidMount = () => {
    this.getProject();
  };

  getProject = () => {
    this.props.dispatch({
      type: 'project/getProject',
    });
  };

  onFinish = (values: ProjectType) => {
    this.props.dispatch({
      type: 'project/updateProject',
      payload: values,
    });
  };

  render() {
    const { submitting } = this.props;
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

    const submitFormLayout = {
      wrapperCol: {
        xs: { span: 24, offset: 0 },
        sm: { span: 12, offset: 10 },
      },
    };

    return (
      <PageHeaderWrapper>
        {Object.keys(this.props.project).length !== 0 && (
          <Form
            style={{ marginTop: 8 }}
            initialValues={this.props.project}
            onFinish={this.onFinish}
          >
            <Card bordered={false} title="项目">
              <FormItem
                {...formItemLayout}
                label="名称："
                name="name"
                rules={[{ required: true, message: '请输入名称' }]}
              >
                <Input placeholder="名称" />
              </FormItem>
            </Card>
            <Card bordered={false} title="作者" style={{ marginTop: 8 }}>
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
              <FormItem {...submitFormLayout} style={{ marginTop: 32 }}>
                <Button type="primary" htmlType="submit" loading={submitting}>
                  保存
                </Button>
              </FormItem>
            </Card>
          </Form>
        )}
      </PageHeaderWrapper>
    );
  }
}

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
