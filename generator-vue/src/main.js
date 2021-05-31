import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import {
  Button,
  Layout,
  Menu,
  Form,
  Input,
  Select,
  DatePicker,
  Switch,
  Checkbox,
  Radio,
  Table,
  Card,
} from "ant-design-vue";

const app = createApp(App);
app
  .use(Button)
  .use(Layout)
  .use(Menu)
  .use(Form)
  .use(Input)
  .use(Switch)
  .use(DatePicker)
  .use(Select)
  .use(Checkbox)
  .use(Radio)
  .use(Table)
  .use(Card)
  .use(store)
  .use(router)
  .mount("#app");
