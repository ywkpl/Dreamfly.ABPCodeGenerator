import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import * as antIcons from "@ant-design/icons-vue";
import { Button, Layout, Menu } from "ant-design-vue";

const app = createApp(App);
// 注册组件
Object.keys(antIcons).forEach((key) => {
  app.component(key, antIcons[key]);
});
// 添加到全局
app.config.globalProperties.$antIcons = antIcons;
app.use(Button).use(Layout).use(Menu).use(store).use(router).mount("#app");
