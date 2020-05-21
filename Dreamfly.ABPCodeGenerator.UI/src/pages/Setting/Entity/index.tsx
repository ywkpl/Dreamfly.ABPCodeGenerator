import React, { Component } from 'react';
import { Form, Card, Input, Button } from 'antd';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import { connect, Dispatch } from 'umi';

const FormItem = Form.Item;

interface Props {
  submitting: boolean;
  dispatch: Dispatch<any>;
}
interface State {}

class Entity extends Component<Props, State> {
  constructor(props: Props) {
    super(props);
    this.state = {};
  }

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
        {true && (
          <Form style={{ marginTop: 8 }}>
            <Card bordered={false} title="实体">
              <FormItem
                {...formItemLayout}
                label="名称："
                name="name"
                rules={[{ required: true, message: '请输入名称' }]}
              >
                <Input placeholder="名称" />
              </FormItem>
            </Card>
          </Form>
        )}
      </PageHeaderWrapper>
    );
  }
}

export default connect(
  ({ loading, entity }: { loading: { effects: { [key: string]: boolean } }; entity: any }) => ({
    submitting: loading.effects['entity/submit'],
    entity,
  }),
)(Entity);
